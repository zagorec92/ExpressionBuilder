using System;
using System.Linq.Expressions;
using ExpressionBuilder.Expressions;

namespace ExpressionBuilder
{
	/// <summary>
	/// Builder for predicates (expressions returning bool).
	/// </summary>
	internal static class PredicateBuilder
	{
		/// <summary>
		/// Predicate which evaluates to true.
		/// </summary>
		/// <typeparam name="T">Entity type.</typeparam>
		/// <returns>True.</returns>
		public static Expression<Func<T, bool>> True<T>() { return f => true; }

		/// <summary>
		/// Predicate which evaluates to false.
		/// </summary>
		/// <typeparam name="T">Entity type.</typeparam>
		/// <returns>False.</returns>
		public static Expression<Func<T, bool>> False<T>() { return f => false; }

		/// <summary>
		/// Predicate which combines two predicates with and operator.
		/// </summary>
		/// <typeparam name="T">Entity type.</typeparam>
		/// <param name="first">First expression to combine.</param>
		/// <param name="second">Second expression to combine.</param>
		/// <returns>Conjunction of expressions.</returns>
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			if (first == null)
				throw new ArgumentNullException(nameof(first));
			if (second == null)
				throw new ArgumentNullException(nameof(second));

			ParameterExpression parameter = Expression.Parameter(typeof(T));

			ParameterUpdateVisitor leftVisitor = new ParameterUpdateVisitor(first.Parameters[0], parameter);
			Expression left = leftVisitor.Visit(first.Body);

			ParameterUpdateVisitor rightVisitor = new ParameterUpdateVisitor(second.Parameters[0], parameter);
			Expression right = rightVisitor.Visit(second.Body);

			return Expression.Lambda<Func<T, bool>>(Expression.And(left, right), parameter);
		}
	}
}
