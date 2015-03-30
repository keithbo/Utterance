using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utterance.Linq;
using Xunit;

namespace Utterance.Tests
{
	public class LinqTests
	{
		[Fact]
		public void Test()
		{
			var factory = Access.Create(Access.MethodGroup.Aggregate, typeof(int));
			var instance = factory();

			System.Console.WriteLine();
		}

		[Fact]
		public void StaticPropertyTest()
		{
			var type = typeof(TempClass<>);
			var prop = type.GetField("First");

			var value = prop.GetValue(null);

			System.Console.WriteLine();
		}

		[Fact]
		public void CombineTest()
		{
			var selectEx = Select.Cache<string, string>.SourceSelector;
		}

		public static class TempClass<TSource>
		{
			public static readonly Expression<Func<IEnumerable<TSource>, TSource>> First = (source) => source.First();
		}
	}
}
