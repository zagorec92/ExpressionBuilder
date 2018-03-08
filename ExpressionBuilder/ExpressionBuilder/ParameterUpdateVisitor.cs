/*
 * Copyright © zagorec92 2018
 * Licensed under MIT license.
 */

using System.Linq.Expressions;

namespace ExpressionBuilder.Expressions
{
	/// <summary>
	/// Encapsulates logic for expression parameter update
	/// </summary>
	internal class ParameterUpdateVisitor : ExpressionVisitor
	{
		#region Fields

		private readonly ParameterExpression _oldParameter;
		private readonly ParameterExpression _newParameter;

		#endregion

		#region Constructor

		public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
		{
			_oldParameter = oldParameter;
			_newParameter = newParameter;
		}

		#endregion

		#region Methods

		protected override Expression VisitParameter(ParameterExpression node)
		{
			if (ReferenceEquals(node, _oldParameter))
				return _newParameter;

			return base.VisitParameter(node);
		}

		#endregion
	}
}
