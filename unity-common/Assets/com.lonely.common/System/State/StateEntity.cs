using System.Collections.Generic;
using System.Display;
using System.Linq;

namespace System.State
{
  public abstract class StateEntity<TStateEntity> : IStateEntity where TStateEntity : StateEntity<TStateEntity>
  {
    public abstract string Key { get; }

    public abstract TStateEntity DeepCopy();

    public abstract IEnumerable<IStateEntity> GetChildren();

    public virtual IEnumerable<object> GetDisplays()
    {
      return new [] { this };
    }
  }

  public static class StateEntityExtensions
  {
    public static IList<T> DeepCopy<T>(this IList<T> list) where T : StateEntity<T>
    {
      return list.Select(x => x == null ? default : x.DeepCopy()).ToList();
    }

    public static T[] DeepCopy<T>(this T[] list) where T : StateEntity<T>
    {
      return list.Select(x => x == null ? default : x.DeepCopy()).ToArray();
    }

    public static IDictionary<K,V> DeepCopy<K, V>(this IDictionary<K, V> dictionary)
      where K : struct
      where V : StateEntity<V>
    {
      var newDictionary = new Dictionary<K, V>();
      foreach (var pair in dictionary)
      {
        newDictionary.Add(pair.Key, pair.Value.DeepCopy());
      }

      return newDictionary;
    }
    
    public static IDictionary<string, V> DeepCopy<V>(this IDictionary<string, V> dictionary)
      where V : StateEntity<V>
    {
      var newDictionary = new Dictionary<string, V>();
      foreach (var pair in dictionary)
      {
        newDictionary.Add(pair.Key, pair.Value.DeepCopy());
      }

      return newDictionary;
    }
    
    public static IDictionary<Type, V> DeepCopy<V>(this IDictionary<Type, V> dictionary)
      where V : StateEntity<V>
    {
      var newDictionary = new Dictionary<Type, V>();
      foreach (var pair in dictionary)
      {
        newDictionary.Add(pair.Key, pair.Value.DeepCopy());
      }

      return newDictionary;
    }
  }
}
