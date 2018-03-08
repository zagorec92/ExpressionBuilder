using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder
{
	/// <summary>
	/// Helper for common expressions and expression operations.
	/// </summary>
	internal static class ExpressionHelper
	{
		/// <summary>
		/// Checks if the property evaluates to the given value.
		/// </summary>
		/// <typeparam name="TEntity">Entity type.</typeparam>
		/// <typeparam name="TProperty">Property type.</typeparam>
		/// <param name="property">Property selector.</param>
		/// <param name="value">Value.</param>
		/// <returns>Predicate expression which checks if the <paramref name="property"/> evaluates to <paramref name="value"/>.</returns>
		public static Expression<Func<TEntity, bool>> PropertyEquals<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property, TProperty value)
		{
			if (property == null)
				throw new ArgumentNullException(nameof(property));

			return PropertyEquals(property, Expression.Constant(value));
		}

		/// <summary>
		/// Checks if the property evaluates to the given value.
		/// </summary>
		/// <typeparam name="TEntity">Entity type.</typeparam>
		/// <typeparam name="TProperty">Property type.</typeparam>
		/// <param name="property">Property selector.</param>
		/// <param name="value">Value.</param>
		/// <returns>Predicate expression which checks if the <paramref name="property"/> evaluates to <paramref name="value"/>.</returns>
		public static Expression<Func<TEntity, bool>> PropertyEquals<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property, Expression value)
		{
			if (property == null)
				throw new ArgumentNullException(nameof(property));

			return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(property.Body, value), property.Parameters.Single());
		}

		/// <summary>
		/// Checks if the string property contains the given value.
		/// </summary>
		/// <typeparam name="TEntity">Entity type.</typeparam>
		/// <param name="property">Property selector.</param>
		/// <param name="value">Value.</param>
		/// <returns>Predicate expression which checks if the <paramref name="property"/> contains the <paramref name="value"/>.</returns>
		public static Expression<Func<TEntity, bool>> StringPropertyContains<TEntity>(Expression<Func<TEntity, string>> property, string value)
		{
			if (property == null)
				throw new ArgumentNullException(nameof(property));

			return StringPropertyContains(property, Expression.Constant(value));
		}


		/// <summary>
		/// Checks if the string property contains the given value.
		/// </summary>
		/// <typeparam name="TEntity">Entity type.</typeparam>
		/// <param name="property">Property selector.</param>
		/// <param name="value">Value.</param>
		/// <returns>Predicate expression which checks if the <paramref name="property"/> contains the <paramref name="value"/>.</returns>
		public static Expression<Func<TEntity, bool>> StringPropertyContains<TEntity>(Expression<Func<TEntity, string>> property, Expression value)
		{
			if (property == null)
				throw new ArgumentNullException(nameof(property));

			MethodInfo method = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });

			return Expression.Lambda<Func<TEntity, bool>>(Expression.Call(property.Body, method, value), property.Parameters.Single());
		}

		/// <summary>
		/// Converts the given lambda return value type.
		/// </summary>
		/// <typeparam name="TEntity">Entity type.</typeparam>
		/// <typeparam name="TProperty">Return type to convert to.</typeparam>
		/// <param name="lambda">Tha lambda expression.</param>
		/// <returns>Lambda expression with the converted return type.</returns>
		public static Expression<Func<TEntity, TProperty>> ConvertLambdaReturnValue<TEntity, TProperty>(Expression<Func<TEntity, object>> lambda)
		{
			if (lambda == null)
				throw new ArgumentNullException(nameof(lambda));

			UnaryExpression convertedExpressionBody = Expression.Convert(lambda.Body, typeof(TProperty));

			return Expression.Lambda<Func<TEntity, TProperty>>(convertedExpressionBody, lambda.Parameters);
		}
	}
}
