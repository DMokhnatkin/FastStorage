using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace FastStorage.Core
{
    /// <summary>
    /// A generic object comparerer that would only use object's reference, 
    /// ignoring any <see cref="IEquatable{T}"/> or <see cref="object.Equals(object)"/>  overrides.
    /// </summary>
    [PublicAPI]
    public class ObjectReferenceEqualityComparer<T> : EqualityComparer<T>
        where T : class
    {
        private static IEqualityComparer<T> _defaultComparer;

        public new static IEqualityComparer<T> Default => _defaultComparer ?? (_defaultComparer = new ObjectReferenceEqualityComparer<T>());

        #region IEqualityComparer<T> Members

        public override bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }

        #endregion
    }
}