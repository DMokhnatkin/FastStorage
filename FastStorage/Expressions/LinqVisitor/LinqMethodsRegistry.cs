using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace FastStorage.Expressions.LinqVisitor
{
    static class LinqMethodsRegistry
    {
        public static MethodInfo Where = FindLinqMethod(
            nameof(Queryable.Where),
            Parameter.ExpressionOfType(typeof(Func<,>)));

        public static MethodInfo Select = FindLinqMethod(
            nameof(Queryable.Select),
            Parameter.ExpressionOfType(typeof(Func<,>)));

        public static MethodInfo SelectMany = FindLinqMethod(
            nameof(Queryable.SelectMany),
            Parameter.ExpressionOfType(typeof(Func<,>)));

        public static MethodInfo Join = FindLinqMethod(
            nameof(Queryable.Join),
            Parameter.OfType(typeof(IEnumerable<>)),
            Parameter.ExpressionOfType(typeof(Func<,>)),
            Parameter.ExpressionOfType(typeof(Func<,>)),
            Parameter.ExpressionOfType(typeof(Func<,,>)));

        public static MethodInfo GroupJoin = FindLinqMethod(
            nameof(Queryable.GroupJoin),
            Parameter.OfType(typeof(IEnumerable<>)),
            Parameter.ExpressionOfType(typeof(Func<,>)),
            Parameter.ExpressionOfType(typeof(Func<,>)),
            Parameter.ExpressionOfType(typeof(Func<,,>)));

        public static MethodInfo Zip = FindLinqMethod(
            nameof(Queryable.Zip),
            Parameter.OfType(typeof(IEnumerable<>)),
            Parameter.ExpressionOfType(typeof(Func<,,>)));

        /// <summary>
        /// Find linq method by specifed name and parameter types (first parameter is ignored. It is always IQuerable)
        /// </summary>
        private static MethodInfo FindLinqMethod(string name, params Parameter[] methodParams)
        {
            var res = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(x =>
                x.Name == name &&
                (methodParams == null ||
                    x.GetParameters()
                        .Skip(1)
                        .Select((info, i) => methodParams[i].IsSatisfied(info))
                        .All(y => y)));
            if (res == null)
                throw new InvalidOperationException($"\"{name}\" linq method with ({string.Join(",", methodParams.Select(x => x.DebugView))}) non self params wasn't found. ");
            return res;
        }

        private class Parameter
        {
            public string DebugView { get; }

            public Func<ParameterInfo, bool> IsSatisfied { get; }

            private Parameter([NotNull] string debugView, [NotNull] Func<ParameterInfo, bool> isSatisfied)
            {
                IsSatisfied = isSatisfied;
                DebugView = debugView;
            }

            public static Parameter ExpressionOfType(Type expressionInnerType)
            {
                return new Parameter($"Expression<{expressionInnerType}>",
                    info => info.ParameterType.IsGenericType &&
                        info.ParameterType.GetGenericTypeDefinition() == typeof(Expression<>) &&
                        info.ParameterType.GetGenericArguments().Length > 0 && 
                        info.ParameterType.GetGenericArguments()[0].Name == expressionInnerType.Name);
            }

            public static Parameter OfType(Type type)
            {
                return new Parameter($"{type}",
                    info => 
                        info.ParameterType.Name == type.Name);
            }
        }
    }
}
