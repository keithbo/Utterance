using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Utterance.Rewrite
{
    internal class ExpressionComposer : ComposerBase, IExpressionComposer, IParameterHost
    {
        private ParameterNode _head;

        private LambdaExpression _original;

        private Expression _body;

        public ExpressionComposer(LambdaExpression original)
        {
            _original = original;
        }

        public IExpressionComposer Parameter(int index, Action<IParameterComposer> compose)
        {
            Extract();

            var node = GetOrAdd(index);
            compose(node.Value);

            return this;
        }

        private ParameterNode GetOrAdd(int index)
        {
            if (!TryGetAt(index, out var nodeBefore, out var nodeAfter))
            {
                throw new IndexOutOfRangeException();
            }

            if (nodeAfter != null)
            {
                return nodeAfter;
            }

            var node = new ParameterNode
            {
                Value = new ParameterComposer(this, null)
            };
            InsertParameter(node, nodeBefore, nodeAfter);

            return node;
        }

        private void InsertParameter(int index, ParameterNode node)
        {
            if (!TryGetAt(index, out var nodeBefore, out var nodeAfter))
            {
                throw new IndexOutOfRangeException();
            }

            InsertParameter(node, nodeBefore, nodeAfter);
        }

        private void InsertParameter(ParameterNode node, ParameterNode nodeBefore, ParameterNode nodeAfter)
        {
            node.Prev = nodeBefore;
            node.Next = nodeAfter;
            if (nodeBefore != null)
            {
                nodeBefore.Next = node;
            }

            if (nodeAfter != null)
            {
                nodeAfter.Prev = node;
            }

            if (_head == null)
            {
                _head = node;
            }
        }

        private ParameterNode Find(ExpressionRef reference)
        {
            var nav = new ParameterNodeNavigator(_head);
            do
            {
                if (Equals(nav.Current?.Value.Reference, reference))
                {
                    return nav.Current;
                }
            } while (nav.CanMove(Direction.Forward));

            return null;
        }

        private bool TryGetAt(int index, out ParameterNode nodeBefore, out ParameterNode nodeAfter)
        {
            if (index < 0) throw new IndexOutOfRangeException();

            var i = 0;
            var nav = new ParameterNodeNavigator(_head);
            ParameterNode prev;
            ParameterNode current = null;
            var found = false;
            do
            {
                prev = current;
                current = nav.Current;
                if (i++ == index)
                {
                    found = true;
                    break;
                }
            } while (nav.Move(Direction.Forward));

            nodeBefore = prev;
            nodeAfter = current;
            return found;
        }

        IParameterComposer IParameterHost.Sibling(ExpressionRef reference, Direction direction)
        {
            var nav = new ParameterNodeNavigator(Find(reference));
            nav.Move(direction);
            return nav.Current?.Value;
        }

        public LambdaExpression Build()
        {
            if (_original != null) return _original;

            return Expression.Lambda(_body, EnumerateParameters()
                .Select(parameter => parameter.Build())
                .ToArray());
        }

        private IEnumerable<ParameterComposer> EnumerateParameters()
        {
            var nav = new ParameterNodeNavigator(_head);
            do
            {
                if (nav.Current != null)
                {
                    yield return nav.Current.Value;
                }
            } while (nav.Move(Direction.Forward));
        }

        private void Extract()
        {
            if (_original == null) return;

            ParameterNode tail = null;

            foreach (var parameter in _original.Parameters)
            {
                var node = new ParameterNode
                {
                    Value = new ParameterComposer(this, parameter),
                    Prev = tail

                };

                if (tail != null)
                {
                    tail.Next = node;
                }
                if (_head == null)
                {
                    _head = node;
                }

                tail = node;
            }

            _body = _original.Body;

            _original = null;
        }

        internal class ParameterNode
        {
            public ParameterNode Next { get; set; }
            public ParameterNode Prev { get; set; }

            public ParameterComposer Value { get; set; }
        }

        internal class ParameterNodeNavigator
        {
            private ParameterNode _last;
            private Direction _lastDirection;

            public ParameterNode Current { get; private set; }

            public ParameterNodeNavigator(ParameterNode node)
            {
                Current = node;
            }

            public bool CanMove(Direction direction)
            {
                if (Current != null)
                {
                    return true;
                }

                if (_last == null)
                {
                    return false;
                }

                // we can move back only
                return direction != _lastDirection;
            }

            public bool Move(Direction direction)
            {
                if (Current == null)
                {
                    if (direction == _lastDirection)
                    {
                        return false;
                    }

                    if (_last == null)
                    {
                        return false;
                    }

                    Current = _last;
                    _last = null;
                    _lastDirection = direction;
                }
                else
                {
                    _last = Current;
                    _lastDirection = direction;
                    Current = direction == Direction.Forward ? Current.Next : Current.Prev;
                }

                return true;
            }

        }
    }
}
