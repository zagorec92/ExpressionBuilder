/*
 * Copyright © zagorec92 2018
 * Licensed under MIT license.
 */

using System;
using System.Linq.Expressions;

namespace ExpressionBuilder.Expressions
{
	public class ExpressionBuilder<T>
	{
		#region Fields

		private bool _cacheEnabled = true;
		private bool _shouldRecompile = false;

		private T _compiledDelegate;
		private Expression<T> _expression;

		#endregion

		#region Properties

		public bool CacheEnabled
		{
			get { return _cacheEnabled; }
			set { _cacheEnabled = value; }
		}

		public T Function
		{
			get
			{
				if(_cacheEnabled && (_compiledDelegate == null || _shouldRecompile))
				{
					if (_expression != null)
						_compiledDelegate = _expression.Compile();
					else
						throw new ArgumentNullException("Expression must not have a null or default value", nameof(_expression));

					_shouldRecompile = false;
				}

				return _cacheEnabled ? _compiledDelegate : _expression.Compile();
			}
		}

		#endregion

		#region Constructor

		public ExpressionBuilder() { }

		public ExpressionBuilder(Expression<T> expression)
		{
			_expression = expression;
		}

		#endregion

		#region Methods

		#region Conditional

		public void And(Expression<T> expression)
		{
			if (_expression == null)
			{
				_expression = expression;
			}
			else
			{
				expression = UpdateParameters(expression);
				UpdateExpression(expression, Expression.And(_expression.Body, expression.Body));
			}
		}

		public void Or(Expression<T> expression)
		{
			if (_expression == null)
			{
				_expression = expression;
			}
			else
			{
				expression = UpdateParameters(expression);
				UpdateExpression(expression, Expression.Or(_expression.Body, expression.Body));
			}
		}

		#endregion

		#region Update

		private Expression<T> UpdateParameters(Expression<T> expression)
		{
			ParametersUpdateWrapper<T> wrapper = new ParametersUpdateWrapper<T>(expression, _expression.Parameters, expression.Parameters);
			expression = wrapper.Update();

			return expression;
		}

		private void UpdateExpression(Expression<T> expression, BinaryExpression binaryExpression)
		{
			_expression = Expression.Lambda<T>(binaryExpression, expression.Parameters);
			_shouldRecompile = true;
		}

		#endregion

		#endregion
	}
}
