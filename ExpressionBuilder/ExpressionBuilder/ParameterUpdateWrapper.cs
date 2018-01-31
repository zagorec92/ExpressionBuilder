using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace ExpressionBuilder.Expressions
{
	/// <summary>
	/// Contains logic for updating expression parameters.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ParametersUpdateWrapper<T>
	{
		private ReadOnlyCollection<ParameterExpression> _parameters;
		private ReadOnlyCollection<ParameterExpression> _newParameters;
		private Expression<T> _expression;

		public ParametersUpdateWrapper(Expression<T> expression, ReadOnlyCollection<ParameterExpression> parameters, ReadOnlyCollection<ParameterExpression> newParameters)
		{
			_expression = expression;
			_parameters = parameters;
			_newParameters = newParameters;

			CheckValidity();
		}

		private void CheckValidity()
		{
			if (_parameters.Count != _newParameters.Count)
				throw new Exception("Parameter count does not match.");
		}

		public Expression<T> Update()
		{
			for (int i = 0; i < _parameters.Count; i++)
			{
				ParameterUpdateVisitor visitor = new ParameterUpdateVisitor(_newParameters[i], _parameters[i]);
				_expression = (Expression<T>)visitor.Visit(_expression);
			}

			return _expression;
		}
	}
}
