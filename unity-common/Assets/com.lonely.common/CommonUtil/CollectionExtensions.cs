using System;
using System.Collections.Generic;
using System.Linq;

namespace com.lonely.common.CommonUtil
{
  public static class CollectionExtensions
  {
    public static IEnumerable<U> Select<T, U>(this T source, Func<T, IEnumerable<U>> selector, out U[] collected)
    {
      collected = selector(source).ToArray();
      return collected;
    }
    
    public static IEnumerable<U> Select<T, U>(this IEnumerable<T> source, Func<T, U> selector, out U[] collected)
    {
      collected = source.Select(selector).ToArray();
      return collected;
    }
    
    public static IEnumerable<U> SelectMany<T, U>(this IEnumerable<T> source, Func<T, IEnumerable<U>> selector, out U[] collected)
    {
      collected = source.SelectMany(selector).ToArray();
      return collected;
    }
    
    public static IEnumerable<U> SelectNotNull<T, U>(this IEnumerable<T> source, Func<T, U> selector)
    {
      return source.SelectNotNull(selector, out _);
    }
    
    public static IEnumerable<U> SelectNotNull<T, U>(this IEnumerable<T> source, Func<T, U> selector, out U[] collected)
    {
      collected = source.Select(selector).Where(x => x != null).ToArray();
      return collected;
    }
    
    public static IEnumerable<U> SelectManyNotNull<T, U>(this IEnumerable<T> source, Func<T, IEnumerable<U>> selector)
    {
      return source.SelectManyNotNull(selector, out _);
    }
    
    public static IEnumerable<U> SelectManyNotNull<T, U>(this IEnumerable<T> source, Func<T, IEnumerable<U>> selector, out U[] collected)
    {
      collected = source.SelectMany(selector).Where(x => x != null).ToArray();
      return collected;
    }
    
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> filter, out T[] collected)
    {
      collected = source.Where(filter).ToArray();
      return collected;
    }
    
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (var item in source)
      {
        action(item);
      }
    }
  }
}