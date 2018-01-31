using System.Linq.Expressions;

namespace ExpressionBuilder.Expressions
{
	/// <summary>
	/// Encapsulates logic for expression parameter update
	/// </summary>
	internal class ParameterUpdateVisitor : ExpressionVisitor
	{
		#region Fields

		private ParameterExpression _oldParameter;
		private ParameterExpression _newParameter;

		#endregion

		public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
		{
			_oldParameter = oldParameter;
			_newParameter = newParameter;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			if (ReferenceEquals(node, _oldParameter))
				return _newParameter;

			return base.VisitParameter(node);
		}
	}
}
