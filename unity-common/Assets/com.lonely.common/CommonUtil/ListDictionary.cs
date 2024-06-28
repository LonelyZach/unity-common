using System;
using System.Collections.Generic;
using System.Linq;

namespace com.lonely.common.CommonUtil
{
public class ListDictionary<TKey, TValue>
  {
    private readonly IDictionary<TKey, IList<TValue>> _inner;

    public ListDictionary()
    {
      _inner = new Dictionary<TKey, IList<TValue>>();
    }
    
    private ListDictionary(IDictionary<TKey, IList<TValue>> inner)
    {
      _inner = inner;
    }

    public void AddToList(TKey key, TValue value)
    {
      EnsureKey(key);
      _inner[key].Add(value);
    }
    
    public void RemoveFromList(TKey key, TValue value)
    {
      EnsureKey(key);
      _inner[key].Remove(value);
      if (_inner[key].Count < 1)
      {
        Remove(key);
      }
    }

    public void Remove(TKey key)
    {
      _inner.Remove(key);
    }

    public IList<TValue> GetList(TKey key)
    {
      return _inner.TryGetValue(key, out var value)
        ? value
        : new List<TValue>();
    }
    
    public IList<TValue> GetAllFlattened()
    {
      return _inner.Values.SelectMany(x => x).ToList();
    }

    public void ReplaceInList(TKey key, TValue original, TValue replacement)
    {
      EnsureKey(key);
      var list = _inner[key];
      var index = list.IndexOf(original);

      if (index == -1)
      {
        throw new Exception($"{key} to replace not found.");
      }

      list[index] = replacement;
    }

    public ICollection<TKey> Keys => _inner.Keys;

    /// <summary>
    /// Copies the data structures, but does not copy the contents.
    /// </summary>
    public ListDictionary<TKey, TValue> ShallowCopy()
    {
      var keyValues = _inner.Select(x => new KeyValuePair<TKey, IList<TValue>>(x.Key, x.Value.ToList()));
      var copiedDictionary = new Dictionary<TKey, IList<TValue>>(keyValues);
      return new ListDictionary<TKey, TValue>(copiedDictionary);
    }
    
    public ListDictionary<TKey, TValue> Copy(Func<TValue, TValue> itemCopier)
    {
      var keyValues = _inner.Select(x => new KeyValuePair<TKey, IList<TValue>>(x.Key, x.Value.Select(itemCopier).ToList()));
      var copiedDictionary = new Dictionary<TKey, IList<TValue>>(keyValues);
      return new ListDictionary<TKey, TValue>(copiedDictionary);
    }

    public void EnsureKey(TKey key)
    {
      if (!_inner.ContainsKey(key))
      {
        _inner.Add(key, new List<TValue>());
      }
    }
  }
  
  public class TypedListDictionary<TValue>
  {
    private readonly ListDictionary<Type, TValue> _inner;

    public TypedListDictionary()
    {
      _inner = new ListDictionary<Type, TValue>();
    }
    
    private TypedListDictionary(ListDictionary<Type, TValue> inner)
    {
      _inner = inner;
    }

    public void AddToList<TKey>(TValue value)
    {
      _inner.AddToList(typeof(TKey), value);
    }
    
    public void AddToList(Type key, TValue value)
    {
      _inner.AddToList(key, value);
    }

    public void EnsureKey(Type key)
    {
      _inner.EnsureKey(key);
    }
    
    public IList<TValue> GetList<TKey>()
    {
      return _inner.GetList(typeof(TKey));
    }
    
    public IList<TValue> GetList(Type key)
    {
      return _inner.GetList(key);
    }
    
    public IList<TValue> GetAllFlattened()
    {
      return _inner.GetAllFlattened();
    }

    public void ReplaceInList(Type key, TValue original, TValue replacement)
    {
      _inner.ReplaceInList(key, original, replacement);
    }
    
    public TypedListDictionary<TValue> ShallowCopy()
    {
      return new TypedListDictionary<TValue>(_inner.ShallowCopy());
    }
    
    public TypedListDictionary<TValue> Copy(Func<TValue, TValue> itemCopier)
    {
      return new TypedListDictionary<TValue>(_inner.Copy(itemCopier));
    }
  }
}