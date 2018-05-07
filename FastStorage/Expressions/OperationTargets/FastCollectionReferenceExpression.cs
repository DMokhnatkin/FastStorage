using System;
using System.Linq.Expressions;
using FastStorage.Core;
using JetBrains.Annotations;

namespace FastStorage.Expressions.OperationTargets
{
    internal class FastCollectionReferenceExpression : Expression
    {
        [NotNull]
        public IFastCollection FastCollection { get; }
        
        /// <inheritdoc />
        public override ExpressionType NodeType => ExpressionType.Extension;

        /// <inheritdoc />
        public override Type Type => FastCollection.DataType;

        /// <inheritdoc />
        public override bool CanReduce => true;

        /// <inheritdoc />
        public override Expression Reduce()
        {
            // We need return fake fast collection item here (otherwise visitor will fail)
            // (you can perceive it as one item from fast collection enumeration)
            if (Type.IsValueType)
            {
                return Constant(Activator.CreateInstance(Type));
            }
            return Constant(null, Type);
        }

        public FastCollectionReferenceExpression(IFastCollection fastCollection)
        {
            FastCollection = fastCollection ?? throw new ArgumentNullException(nameof(fastCollection));
        }
    }
}