using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionBuilder.Test
{
	[TestClass]
	public class QueryExpressionTests
	{
		[TestMethod]
		public void QueryExpression_StringPropertyContainsSinglePredicate()
		{
			// arrange
			ICollection<Student> students = new List<Student>
			{
				new Student { FirstName = "John", LastName = "Doe" },
				new Student { FirstName = "Jane", LastName = "Doe" }
			};
			Expression<Func<Student, bool>> predicate = ExpressionHelper.StringPropertyContains<Student>(s => s.LastName, "D");
			Expression<Func<Student, bool>> query = QueryExpression.Filter(predicate);

			// act
			ICollection<Student> result = students.Where(query.Compile()).ToList();

			// assert
			CollectionAssert.AreEquivalent((ICollection)result, (ICollection) students);
		}

		[TestMethod]
		public void QueryExpression_StringPropertyContainsMultiplePredicates()
		{
			// arrange
			ICollection<Student> students = new List<Student>
			{
				new Student { FirstName = "John", LastName = "Doe" },
				new Student { FirstName = "Jane", LastName = "Doe" }
			};
			Expression<Func<Student, bool>> predicateFirstName = ExpressionHelper.StringPropertyContains<Student>(s => s.FirstName, "John");
			Expression<Func<Student, bool>> predicateLastName = ExpressionHelper.StringPropertyContains<Student>(s => s.LastName, "Do");
			Expression<Func<Student, bool>> query = QueryExpression.Filter(predicateFirstName, predicateLastName);

			// act
			ICollection<Student> result = students.Where(query.Compile()).ToList();

			// assert
			Assert.IsTrue(result.Single() == students.Single(s => s.FirstName == "John" && s.LastName == "Doe"));
		}
	}
}
