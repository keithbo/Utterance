using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utterance
{
	public abstract class IdentityExpressionVisitor : ResettingExpressionVisitor
	{
		protected byte[] Bytes
		{
			get { return Stream != null ? Stream.ToArray() : new byte[0]; }
		}

		protected bool IncludePropertyNames
		{
			get;
			set;
		}

		private MemoryStream Stream
		{
			get;
			set;
		}
		private BinaryWriter Writer
		{
			get;
			set;
		}

		protected IdentityExpressionVisitor()
		{
		}

		protected override void Reset()
		{
			base.Reset();

			Stream = new MemoryStream();
			Writer = new BinaryWriter(Stream, Encoding.Default);
		}

		protected void WriteBytes(params byte[][] steps)
		{
			Writer.Write(steps.SelectMany(s => s).ToArray());
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.IsLifted), ToBytes(node.IsLiftedToNull), ToBytes(node.Method));
			return base.VisitBinary(node);
		}

		protected override Expression VisitBlock(BlockExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitBlock(node);
		}

		protected override CatchBlock VisitCatchBlock(CatchBlock node)
		{
			WriteBytes(ToBytes(node.GetType()), ToBytes(node.Test));
			return base.VisitCatchBlock(node);
		}

		protected override Expression VisitConditional(ConditionalExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitConditional(node);
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Type), ToBytes(node.Value));
			return base.VisitConstant(node);
		}

		protected override Expression VisitDebugInfo(DebugInfoExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Document));
			return base.VisitDebugInfo(node);
		}

		protected override Expression VisitDefault(DefaultExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitDefault(node);
		}

		protected override Expression VisitDynamic(DynamicExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitDynamic(node);
		}

		protected override ElementInit VisitElementInit(ElementInit node)
		{
			WriteBytes(ToBytes(node.AddMethod));
			return base.VisitElementInit(node);
		}

		protected override Expression VisitExtension(Expression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitExtension(node);
		}

		protected override Expression VisitGoto(GotoExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Kind));
			return base.VisitGoto(node);
		}

		protected override Expression VisitIndex(IndexExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Indexer));
			return base.VisitIndex(node);
		}

		protected override Expression VisitInvocation(InvocationExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitInvocation(node);
		}

		protected override Expression VisitLabel(LabelExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitLabel(node);
		}

		protected override LabelTarget VisitLabelTarget(LabelTarget node)
		{
			WriteBytes(ToBytes(node.Type), ToBytes(node.Name));
			return base.VisitLabelTarget(node);
		}

		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Name), ToBytes(node.ReturnType));
			return base.VisitLambda<T>(node);
		}

		protected override Expression VisitListInit(ListInitExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitListInit(node);
		}

		protected override Expression VisitLoop(LoopExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitLoop(node);
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Member));
			return base.VisitMember(node);
		}

		protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
		{
			WriteBytes(ToBytes(node.BindingType), ToBytes(node.Member));
			return base.VisitMemberAssignment(node);
		}

		protected override MemberBinding VisitMemberBinding(MemberBinding node)
		{
			WriteBytes(ToBytes(node.BindingType), ToBytes(node.Member));
			return base.VisitMemberBinding(node);
		}

		protected override Expression VisitMemberInit(MemberInitExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitMemberInit(node);
		}

		protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
		{
			WriteBytes(ToBytes(node.BindingType), ToBytes(node.Member));
			return base.VisitMemberListBinding(node);
		}

		protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
		{
			WriteBytes(ToBytes(node.BindingType), ToBytes(node.Member));
			return base.VisitMemberMemberBinding(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Method));
			return base.VisitMethodCall(node);
		}

		protected override Expression VisitNew(NewExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Constructor));
			if (node.Members.Count > 0)
			{
				WriteBytes(node.Members.SelectMany(m => ToBytes(m)).ToArray());
			}
			return base.VisitNew(node);
		}

		protected override Expression VisitNewArray(NewArrayExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitNewArray(node);
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Type), ToBytes(node.IsByRef), IncludePropertyNames ? ToBytes(node.Name) : NullValue);
			return base.VisitParameter(node);
		}

		protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitRuntimeVariables(node);
		}

		protected override Expression VisitSwitch(SwitchExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.Comparison));
			return base.VisitSwitch(node);
		}

		protected override SwitchCase VisitSwitchCase(SwitchCase node)
		{
			WriteBytes(ToBytes(node.GetType()));
			return base.VisitSwitchCase(node);
		}

		protected override Expression VisitTry(TryExpression node)
		{
			WriteBytes(ToBytes(node.NodeType));
			return base.VisitTry(node);
		}

		protected override Expression VisitTypeBinary(TypeBinaryExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.TypeOperand));
			return base.VisitTypeBinary(node);
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			WriteBytes(ToBytes(node.NodeType), ToBytes(node.IsLifted), ToBytes(node.IsLiftedToNull), ToBytes(node.Method));
			return base.VisitUnary(node);
		}

		protected static byte[] ToBytes(object value)
		{
			if (value == null) return NullValue;

			var valueType = value.GetType();
			switch (Type.GetTypeCode(valueType))
			{
				case TypeCode.SByte:
				case TypeCode.Byte:
					return new byte[] { (byte)value };

				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
				case TypeCode.String:
					Func<object, byte[]> converter;
					if (Converters.BasicConverters.TryGetValue(valueType, out converter))
					{
						return converter(value);
					}
					break;
			}

			return BitConverter.GetBytes(value.GetHashCode());
		}

		protected static byte[] ToBytes(string value)
		{
			return Encoding.Default.GetBytes(value ?? string.Empty);
		}

		protected static byte[] ToBytes(Enum value)
		{
			Type underlyingType = Enum.GetUnderlyingType(value.GetType());
			Func<Enum, byte[]> converter;
			if (Converters.EnumConverters.TryGetValue(underlyingType, out converter))
			{
				return converter(value);
			}
			return new byte[0];
		}

		protected static readonly byte[] NullValue = new byte[] { (byte)0 };

		private static class Converters
		{
			public static readonly IDictionary<Type, Func<Enum, byte[]>> EnumConverters = new Dictionary<Type, Func<Enum, byte[]>>
			{
				{typeof(bool), (input) => BitConverter.GetBytes((bool)Convert.ChangeType(input, typeof(bool)))},
				{typeof(char), (input) => BitConverter.GetBytes((char)Convert.ChangeType(input, typeof(char)))},
				{typeof(double), (input) => BitConverter.GetBytes((double)Convert.ChangeType(input, typeof(double)))},
				{typeof(Int16), (input) => BitConverter.GetBytes((Int16)Convert.ChangeType(input, typeof(Int16)))},
				{typeof(Int32), (input) => BitConverter.GetBytes((Int32)Convert.ChangeType(input, typeof(Int32)))},
				{typeof(Int64), (input) => BitConverter.GetBytes((Int64)Convert.ChangeType(input, typeof(Int64)))},
				{typeof(Single), (input) => BitConverter.GetBytes((Single)Convert.ChangeType(input, typeof(Single)))},
				{typeof(UInt16), (input) => BitConverter.GetBytes((UInt16)Convert.ChangeType(input, typeof(UInt16)))},
				{typeof(UInt32), (input) => BitConverter.GetBytes((UInt32)Convert.ChangeType(input, typeof(UInt32)))},
				{typeof(UInt64), (input) => BitConverter.GetBytes((UInt64)Convert.ChangeType(input, typeof(UInt64)))},
			};

			public static readonly IDictionary<Type, Func<object, byte[]>> BasicConverters = new Dictionary<Type, Func<object, byte[]>>
			{
				{typeof(bool), (input) => BitConverter.GetBytes((bool)input)},
				{typeof(double), (input) => BitConverter.GetBytes((double)input)},
				{typeof(Int16), (input) => BitConverter.GetBytes((Int16)input)},
				{typeof(Int32), (input) => BitConverter.GetBytes((Int32)input)},
				{typeof(Int64), (input) => BitConverter.GetBytes((Int64)input)},
				{typeof(Single), (input) => BitConverter.GetBytes((Single)input)},
				{typeof(UInt16), (input) => BitConverter.GetBytes((UInt16)input)},
				{typeof(UInt32), (input) => BitConverter.GetBytes((UInt32)input)},
				{typeof(UInt64), (input) => BitConverter.GetBytes((UInt64)input)},
				{typeof(char), (input) => BitConverter.GetBytes((char)input)},
				{typeof(string), (input) => Encoding.Default.GetBytes((string)input)},
			};
		}
	}
}
