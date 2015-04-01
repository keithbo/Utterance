using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Utterance.Tests
{
	public class IdentityExpressionVisitorTests
	{
		protected class MyIdentityExpressionVisitor : IdentityExpressionVisitor
		{
			public byte[] GeneratedBytes
			{
				get { return this.Bytes; }
			}

			public bool PropertyNames
			{
				get { return this.IncludePropertyNames; }
				set { this.IncludePropertyNames = value; }
			}
		}

		[Fact]
		public void ByteStreamGenerationTest()
		{
			Expression<Func<IEnumerable<string>, string>> ex1 = (source) => source.First();
			Expression<Func<IEnumerable<string>, string>> ex2 = (value) => value.First();
			Expression<Func<IEnumerable<string>, string>> ex3 = (source) => source.First();
			var visitor = new MyIdentityExpressionVisitor();
			visitor.PropertyNames = true;

			visitor.Visit(ex1);
			var bytes1 = visitor.GeneratedBytes;
			visitor.Visit(ex2);
			var bytes2 = visitor.GeneratedBytes;
			visitor.Visit(ex3);
			var bytes3 = visitor.GeneratedBytes;

			var hash1 = new MD5CryptoServiceProvider().ComputeHash(bytes1);
			var hashString1 = Encoding.Default.GetString(hash1);

			var hash2 = new MD5CryptoServiceProvider().ComputeHash(bytes2);
			var hashString2 = Encoding.Default.GetString(hash2);

			var hash3 = new MD5CryptoServiceProvider().ComputeHash(bytes3);
			var hashString3 = Encoding.Default.GetString(hash3);

			Assert.True(bytes1.SequenceEqual(bytes3));
			Assert.False(bytes1.SequenceEqual(bytes2));

			Assert.Equal(hashString1, hashString3);
			Assert.NotEqual(hashString1, hashString2);
		}

		[Fact]
		public void TypesTest()
		{
			Expression<Func<string, string>> ex1 = (source) => source;
			Expression<Func<string, object>> ex2 = (source) => source;
			Expression<Func<string, object>> ex3 = (source) => source;

			var visitor = new MyIdentityExpressionVisitor();
			visitor.Visit(ex1);
			var bytes1 = visitor.GeneratedBytes;
			visitor.Visit(ex2);
			var bytes2 = visitor.GeneratedBytes;
			visitor.Visit(ex3);
			var bytes3 = visitor.GeneratedBytes;

			Assert.False(bytes1.SequenceEqual(bytes2));
			Assert.True(bytes2.SequenceEqual(bytes3));
		}

		[Fact]
		public void ConstantTest()
		{
			Expression<Func<int>> ex1 = () => 1;
			Expression<Func<int>> ex2 = () => 4;
			Expression<Func<string>> ex3 = () => "value";
			Expression<Func<string>> ex4 = () => "tools";
			Expression<Func<int>> ex5 = () => 1;
			Expression<Func<object>> ex6 = () => 1;
			Expression<Func<object>> ex7 = () => "value";

			var visitor = new MyIdentityExpressionVisitor();
			visitor.Visit(ex1);
			var bytes1 = visitor.GeneratedBytes;
			visitor.Visit(ex2);
			var bytes2 = visitor.GeneratedBytes;
			visitor.Visit(ex3);
			var bytes3 = visitor.GeneratedBytes;
			visitor.Visit(ex4);
			var bytes4 = visitor.GeneratedBytes;
			visitor.Visit(ex5);
			var bytes5 = visitor.GeneratedBytes;
			visitor.Visit(ex6);
			var bytes6 = visitor.GeneratedBytes;
			visitor.Visit(ex7);
			var bytes7 = visitor.GeneratedBytes;

			Assert.False(bytes1.SequenceEqual(bytes2));
			Assert.False(bytes3.SequenceEqual(bytes4));
			Assert.True(bytes1.SequenceEqual(bytes5));

			Assert.False(bytes1.SequenceEqual(bytes6));
			Assert.False(bytes6.SequenceEqual(bytes7));
			Assert.False(bytes3.SequenceEqual(bytes7));
		}

		[Fact]
		public void ClosureTest()
		{
			var instance = "a value";
			Expression<Func<object>> ex1 = () => instance;
			var string1 = ex1.ToString();
			var eval1 = ex1.PartialEval();

			var bodyExType1 = ((MemberExpression)ex1.Body).Expression.Type;

			var visitor = new MyIdentityExpressionVisitor();
			visitor.Visit(ex1);
			var bytes1 = visitor.GeneratedBytes;

			var other = "a value";
			Expression<Func<object>> ex2 = () => other;
			var string2 = ex2.ToString();
			visitor.Visit(ex2);
			var bytes2 = visitor.GeneratedBytes;

			Assert.False(bytes1.SequenceEqual(bytes2));
		}
	}
}
