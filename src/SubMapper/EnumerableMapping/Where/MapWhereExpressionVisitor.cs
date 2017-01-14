using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SubMapper.EnumerableMapping.Where
{
    public class MapWhereExpressionVisitor : ExpressionVisitor
    {
        public List<Tuple<string, object>> WhereEqualsKeyValues { get; private set; } = new List<Tuple<string, object>>();

        public override Expression Visit(Expression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.Constant:
                case ExpressionType.Lambda:
                case ExpressionType.MemberAccess:
                case ExpressionType.Parameter:
                    break;
                case ExpressionType.Equal:

                    var binaryExpression = node as BinaryExpression;
                    MemberExpression memberExpression;
                    ConstantExpression constantExpression;

                    if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess
                        && binaryExpression.Right.NodeType == ExpressionType.Constant)
                    {
                        memberExpression = binaryExpression.Left as MemberExpression;
                        constantExpression = binaryExpression.Right as ConstantExpression;
                    }
                    else if (binaryExpression.Right.NodeType == ExpressionType.MemberAccess
                        && binaryExpression.Left.NodeType == ExpressionType.Constant)
                    {
                        memberExpression = binaryExpression.Right as MemberExpression;
                        constantExpression = binaryExpression.Left as ConstantExpression;
                    }
                    else throw new NotImplementedException();

                    WhereEqualsKeyValues.Add(Tuple.Create(
                        memberExpression.Member.Name,
                        constantExpression.Value));

                    break;
                default:
                    throw new NotImplementedException();
            }

            return base.Visit(node);
        }
    }
}
