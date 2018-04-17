﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Dime.Expressions.Tests.Mock;
using Dime.Utilities.Expressions.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Expressions.Tests.Internals
{
    [TestClass]
    public class MemberExpressionUtilitiesTests
    {
        [TestMethod]
        [TestCategory("Filter")]
        public void MemberExpressionUtilities_GetExpression_FieldIsNavigationProperty_HasDefaultDisplay_TakesCategory_ShouldReturnOne()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Characteristic = new Characteristic {Category = "Hello world"}},
                new Person { Characteristic = new Characteristic {Category = "No hello world"}},
            };

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser());

            Expression<Func<Person, bool>> expr = expressionBuilder.GetExpression<Person>("Characteristic", "like", "Hello world");

            var items = persons.Where(expr.Compile());
            Assert.IsTrue(items.Count() == 1);
        }

        [TestMethod]
        [TestCategory("Filter")]
        public void MemberExpressionUtilities_GetExpression_FieldIsScalarPropertyInsideNavigationProperty_ShouldThrowArgumentException()
        {
            List<Person> persons = new List<Person>
            {
                new Person { Characteristic = new Characteristic {Category = "Hello world"}},
                new Person { Characteristic = new Characteristic {Category = "No hello world"}},
            };

            IFilterExpressionBuilder expressionBuilder = new ExpressionBuilder()
                .WithDateTimeParser(new DateTimeParser("Europe/Paris", new CultureInfo("nl-BE")))
                .WithDoubleParser(new DoubleParser());

            Assert.ThrowsException<ArgumentException>(
                () => expressionBuilder.GetExpression<Person>("Characteristic.Category", "like", "Hello world"));
        }
    }
}