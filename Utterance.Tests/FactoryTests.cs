using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Utterance.Tests
{
	public class FactoryTests
	{
		protected ITestOutputHelper Output { get; private set; }

		public FactoryTests(ITestOutputHelper output)
		{
			Output = output;
		}

		[Fact]
		public void CreateAnonymousTest()
		{
			var create = Factory.New(typeof(TestType));
			var instance = create();
			var castInstance = Assert.IsType<TestType>(instance);
			Assert.Equal(1, castInstance.ConstructorUsed);

			create = Factory.New(typeof(TestType), typeof(string));
			instance = create("value");
			castInstance = Assert.IsType<TestType>(instance);
			Assert.Equal(2, castInstance.ConstructorUsed);
		}

		[Fact]
		public void CreateGenericTest()
		{
			var create = Factory.New<TestType>();
			var instance = create();
			Assert.IsType<TestType>(instance);
			Assert.Equal(1, instance.ConstructorUsed);

			create = Factory.New<TestType>(typeof(string));
			instance = create("value");
			Assert.IsType<TestType>(instance);
			Assert.Equal(2, instance.ConstructorUsed);

			var iCreate = Factory.New<ITestType>(typeof(TestType), new Type[0]);
			var iInstance = iCreate();
			Assert.IsType<TestType>(iInstance);
			Assert.Equal(1, iInstance.ConstructorUsed);

			iCreate = Factory.New<ITestType>(typeof(TestType), new Type[] { typeof(string) });
			iInstance = iCreate("value");
			Assert.IsType<TestType>(iInstance);
			Assert.Equal(2, iInstance.ConstructorUsed);
		}

		[Fact]
		public void CreateRepeatedTest()
		{
			Stopwatch watch = new Stopwatch();
			int n = 100000;
			ObjectFactory<TestType> create;

			watch.Start();
			create = Factory.New<TestType>();
			watch.Stop();
			decimal initial = Convert.ToDecimal(watch.ElapsedMilliseconds);

			watch.Start();
			for (int i = 1; i < n; i++)
			{
				create = Factory.New<TestType>();
			}
			watch.Stop();
			decimal per = Convert.ToDecimal(watch.ElapsedMilliseconds) / Convert.ToDecimal(n);

			Output.WriteLine($"Iterations: {n}");
			Output.WriteLine($"Initial (ms): {initial:G}");
			Output.WriteLine($"Total (ms): {watch.ElapsedMilliseconds:G}");
			Output.WriteLine($"Per-Call (ms): {per:G}");
		}
	}

	public interface ITestType
	{
		int ConstructorUsed { get; }
	}

	public class TestType : ITestType
	{
		public int ConstructorUsed { get; private set; }

		public TestType()
		{
			ConstructorUsed = 1;
		}

		public TestType(string value)
		{
			ConstructorUsed = 2;
		}
	}
}
