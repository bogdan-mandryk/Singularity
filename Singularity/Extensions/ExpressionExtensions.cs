﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace Singularity
{
	internal static class ExpressionExtensions
	{
		public static ParameterExpression[] GetParameterExpressions(this Expression expression)
		{
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            switch (expression)
			{
				case ConstantExpression _:
                case DefaultExpression _:
                    return new ParameterExpression[0];
                case LambdaExpression lambdaExpression:
					return lambdaExpression.Parameters.ToArray();
				case NewExpression newExpression:
					return newExpression.Arguments.OfType<ParameterExpression>().ToArray();
				case BlockExpression blockExpression:
					return blockExpression.Variables.ToArray();
				case MethodCallExpression methodCallExpression:
					return methodCallExpression.Arguments.OfType<ParameterExpression>().ToArray();
                default:
					throw new NotSupportedException($"The expression of type {expression.GetType()} is not supported");
			}
		}
	}
}
