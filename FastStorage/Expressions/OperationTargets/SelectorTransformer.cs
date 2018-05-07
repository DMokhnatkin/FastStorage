using System.Linq.Expressions;

namespace FastStorage.Expressions.OperationTargets
{
    internal class SelectorTransformer
    {
        /// <summary>
        /// Transform selector expression to form which we can use in operation targets
        /// Transform steps :
        /// 1) If expression is <see cref="ExpressionType.Quote"/> we will return inner node. 
        /// </summary>
        public Expression TransformToNormalForm(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Quote)
                return ((UnaryExpression) expression).Operand;

            return expression;
        }
    }
}
