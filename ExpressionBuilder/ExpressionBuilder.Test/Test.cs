using ExpressionBuilder.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace ExpressionBuilder.Test
{
	[TestClass]
	public class Test
	{
		[TestMethod]
		public void ExpressionNullTest()
		{
			ExpressionBuilder<Func<int, bool>> expression = new ExpressionBuilder<Func<int, bool>>();
			Assert.ThrowsException<ArgumentNullException>(() => expression.Function);
		}

		[TestMethod]
		public void ExpressionConstructorEmptyTest()
		{
			ExpressionBuilder<Func<int, bool>> expression = new ExpressionBuilder<Func<int, bool>>();
			expression.And(x => x % 2 == 0);
			Assert.AreEqual(true, expression.Function.Invoke(2));
			Assert.AreEqual(false, expression.Function.Invoke(1));

			expression = new ExpressionBuilder<Func<int, bool>>();
			expression.Or(x => x % 2 == 0);
			Assert.AreEqual(true, expression.Function.Invoke(2));
			Assert.AreEqual(false, expression.Function.Invoke(1));
		}

		[TestMethod]
		public void ExpressionConstructorConditionTest()
		{
			ExpressionBuilder<Func<int, bool>> expression = new ExpressionBuilder<Func<int, bool>>(x => x % 2 == 0);
			Assert.AreEqual(true, expression.Function.Invoke(2));
			Assert.AreEqual(false, expression.Function.Invoke(1));
		}

		[TestMethod]
		public void ExpressionConstructorAndConditionTest()
		{
			ExpressionBuilder<Func<int, bool>> expression = new ExpressionBuilder<Func<int, bool>>(x => x % 2 == 0);
			expression.And(x => x % 4 == 0);

			Assert.AreEqual(true, expression.Function.Invoke(4));
			Assert.AreEqual(false, expression.Function.Invoke(2));
		}

		[TestMethod]
		public void ExpressionConstructorOrConditionTest()
		{
			ExpressionBuilder<Func<int, bool>> expression = new ExpressionBuilder<Func<int, bool>>(x => x % 2 == 0);
			expression.Or(x => x % 4 == 0);

			Assert.AreEqual(true, expression.Function.Invoke(4));
			Assert.AreEqual(true, expression.Function.Invoke(2));
		}

		[TestMethod]
		public void ExpressionAndConditionTest()
		{
			ExpressionBuilder<Func<int, bool>> expression = new ExpressionBuilder<Func<int, bool>>();
			expression.And(x => x % 2 == 0);
			expression.And(x => x % 3 == 0);

			Assert.AreEqual(true, expression.Function.Invoke(6));
			Assert.AreEqual(false, expression.Function.Invoke(2));
		}

		[TestMethod]
		public void ExpressionOrConditionTest()
		{
			ExpressionBuilder<Func<int?, bool>> expression = new ExpressionBuilder<Func<int?, bool>>();
			expression.And(x => x % 2 == 0);
			expression.Or(x => x % 3 == 0);

			Assert.AreEqual(true, expression.Function.Invoke(2));
			Assert.AreEqual(true, expression.Function.Invoke(3));
		}

		[TestMethod]
		public void ExpressionRecompileTest()
		{
			ExpressionBuilder<Func<int, bool>> expression = new ExpressionBuilder<Func<int, bool>>(x => x == 1);
			expression.Function.Invoke(1);
			expression.Or(x => x < 3);
			Assert.AreEqual(true, expression.Function.Invoke(2));
		}

		[TestMethod]
		public void ExpressionCachePerformanceTest()
		{
			ExpressionBuilder<Func<int, bool>> expressionNoCache = new ExpressionBuilder<Func<int, bool>>(x => x == 1);
			expressionNoCache.CacheEnabled = false;
			ExpressionBuilder<Func<int, bool>> expressionCache = new ExpressionBuilder<Func<int, bool>>(x => x == 1);

			Stopwatch sw = new Stopwatch();
			sw.Start();
			expressionNoCache.Function.Invoke(1);
			expressionNoCache.Function.Invoke(2);
			sw.Stop();

			long ticksNoCache = sw.ElapsedTicks;

			sw.Reset();
			sw.Start();
			expressionCache.Function.Invoke(1);
			expressionCache.Function.Invoke(2);
			sw.Stop();

			long ticksCache = sw.ElapsedTicks;

			Assert.IsTrue((ticksCache * 2) < ticksNoCache);
		}
	}
}
