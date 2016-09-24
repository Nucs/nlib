#if !NET45 
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    /// <summary>Represents a read-only collection of elements that can be accessed by index. </summary>
    /// <typeparam name="T">The type of elements in the read-only list. This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam>
    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
    {
        /// <summary>Gets the element at the specified index in the read-only list.</summary>
        /// <returns>The element at the specified index in the read-only list.</returns>
        /// <param name="index">The zero-based index of the element to get. </param>
        T this[int index] { get; }
    }
}
// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.IReadOnlyCollection`1
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: C01B8778-61B7-4ADA-87B9-EF136D91DF75
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll


namespace System.Collections.Generic
{
  /// <summary>Represents a strongly-typed, read-only collection of elements.</summary>
  /// <typeparam name="T">The type of the elements.This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam>
  public interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable
  {
    /// <summary>Gets the number of elements in the collection.</summary>
    /// <returns>The number of elements in the collection. </returns>
    int Count { get; }
  }
}

#endif