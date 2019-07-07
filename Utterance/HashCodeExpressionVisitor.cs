namespace Utterance
{
    using Utterance.Hashing;

    /// <summary>
    ///     ExpressionVisitor implementation based on IdentityExpressionVisitor. This class will
    ///     construct an FNV1a hash representation of an Expression tree for use as a hashcode impementation.
    ///     HashCode output is repeatable given identical input.
    ///     This impementation includes parameter names in uniqueness calculations by default.
    /// </summary>
    public class HashCodeExpressionVisitor : IdentityExpressionVisitor
    {
        private readonly FNV1aHash _hash;
        private bool _needsHash;
        private bool _valueExists;

        /// <summary>
        ///     Gets the calculated HashCode value for the most recent Expression tree visited.
        /// </summary>
        public int HashCode
        {
            get
            {
                if (!_valueExists) return 0;
                if (_needsHash)
                {
                    _needsHash = false;
                    _hash.Step(Bytes);
                }

                return _hash.Value.GetInt();
            }
        }

        public HashCodeExpressionVisitor()
        {
            _hash = new FNV1aHash();
            IncludePropertyNames = true;
        }

        protected override void Reset()
        {
            base.Reset();
            _hash.Reset();
            _valueExists = true;
            _needsHash = true;
        }
    }
}