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
			Assert.IsType(typeof(TestType), instance);
			var castInstance = (TestType)instance;
			Assert.Equal(1, castInstance.ConstructorUsed);

			create = Factory.New(typeof(TestType), typeof(string));
			instance = create("value");
			Assert.IsType(typeof(TestType), instance);
			castInstance = (TestType)instance;
			Assert.Equal(2, castInstance.ConstructorUsed);
		}

		[Fact]
		public void CreateGenericTest()
		{
			var create = Factory.New<TestType>();
			var instance = create();
			Assert.IsType(typeof(TestType), instance);
			Assert.Equal(1, instance.ConstructorUsed);

			create = Factory.New<TestType>(typeof(string));
			instance = create("value");
			Assert.IsType(typeof(TestType), instance);
			Assert.Equal(2, instance.ConstructorUsed);

			var iCreate = Factory.New<ITestType>(typeof(TestType), new Type[0]);
			var iInstance = iCreate();
			Assert.IsType(typeof(TestType), iInstance);
			Assert.Equal(1, iInstance.ConstructorUsed);

			iCreate = Factory.New<ITestType>(typeof(TestType), new Type[] { typeof(string) });
			iInstance = iCreate("value");
			Assert.IsType(typeof(TestType), iInstance);
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

			Output.WriteLine(string.Format("Iterations: {0}", n));
			Output.WriteLine(string.Format("Initial (ms): {0:G}", initial));
			Output.WriteLine(string.Format("Total (ms): {0:G}", watch.ElapsedMilliseconds));
			Output.WriteLine(string.Format("Per-Call (ms): {0:G}", per));
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
