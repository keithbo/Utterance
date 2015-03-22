using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Utterance.Tests
{
	public class HashCodeExpressionVisitorTests
	{
		[Fact]
		public void HashCodeExpressionVisitorTest()
		{
			Expression<Func<IEnumerable<string>, string>> ex1 = (source) => source.First();
			Expression<Func<IEnumerable<string>, string>> ex2 = (value) => value.First();
			ParameterExpression param = Expression.Parameter(typeof(IEnumerable<string>), "source");
			Expression ex3 = ex2.Replace(ex2.Parameters[0], param);

			var visitor = new HashCodeExpressionVisitor();

			visitor.Visit(ex1);
			var hashCode1 = visitor.HashCode;
			visitor.Visit(ex2);
			var hashCode2 = visitor.HashCode;
			visitor.Visit(ex1);
			var hashCode3 = visitor.HashCode;
			visitor.Visit(ex2);
			var hashCode4 = visitor.HashCode;
			visitor.Visit(ex3);
			var hashCode5 = visitor.HashCode;

			Assert.Equal(hashCode1, hashCode3);
			Assert.NotEqual(hashCode1, hashCode2);
			Assert.Equal(hashCode2, hashCode4);
			Assert.NotEqual(hashCode3, hashCode4);
			Assert.Equal(hashCode1, hashCode5);
		}

	}
}
