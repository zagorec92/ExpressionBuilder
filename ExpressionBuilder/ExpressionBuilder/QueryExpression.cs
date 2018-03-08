using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionBuilder
{
	/// <summary>
	/// Common expressions used for querying.
	/// </summary>
	public static class QueryExpression
	{
		/// <summary>
		/// Combines multiple predicates with 'and' operator.
		/// </summary>
		/// <typeparam name="TEntity">Entity type.</typeparam>
		/// <param name="predicates">Predicates to combine.</param>
		/// <returns>Predicate expression made by combining the given <paramref name="predicates"/> with 'and' operator.</returns>
		public static Expression<Func<TEntity, bool>> Filter<TEntity>(params Expression<Func<TEntity, bool>>[] predicates)
		{
			if (predicates == null)
				throw new ArgumentNullException(nameof(predicates));

			return predicates.Aggregate((p1, p2) => p1.And(p2));
		}
	}
}
