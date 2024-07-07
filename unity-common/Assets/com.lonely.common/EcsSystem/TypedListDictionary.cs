using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.lonely.common.CommonUtil;

namespace com.lonely.common.EcsSystem
{
  public class TypedListDictionary<T>
  {
    private readonly IDictionary<Type, ArrayList> _inner;

    public TypedListDictionary()
    {
      _inner = new Dictionary<Type, ArrayList>();
    }

    public TypedListDictionary(IDictionary<Type, ArrayList> inner)
    {
      _inner = inner;
    }

    public IEnumerable<U> Get<U>()
      where U : T
    {
      return Get(typeof(U)).Select(x => (U)x);
    }
    
    public IEnumerable<U> Get<U>(Func<U, bool> selector)
      where U : T
    {
      return Get<U>().Where(selector);
    }

    public IEnumerable<T> Get(Type type)
    {
      return _inner.TryGetValue(type, out var list)
        ? list.Cast<T>()
        : Enumerable.Empty<T>();
    }
    
    public IEnumerable<T> All()
    {
      return _inner.Values.SelectMany(x => x.ToArray().Select(i => (T)i));
    }
    
    public U GetSingle<U>()
      where U : T
    {
      return Get<U>().Single();
    }
    
    public U GetSingle<U>(Func<U, bool> selector)
      where U : T
    {
      return Get<U>().Single(selector);
    }
    
    public U GetSingleOrDefault<U>()
      where U : T
    {
      return Get<U>().SingleOrDefault();
    }
    
    public U GetSingleOrDefault<U>(Func<U, bool> selector)
      where U : T
    {
      return Get<U>().SingleOrDefault(selector);
    }
    
    public U GetFirstOrDefault<U>()
      where U : T
    {
      return Get<U>().FirstOrDefault();
    }
    
    public U GetFirstOrDefault<U>(Func<U, bool> selector)
      where U : T
    {
      return Get<U>().FirstOrDefault(selector);
    }
    
    public bool TryGetFirst<U>(out U item)
      where U : T
    {
      item = Get<U>().FirstOrDefault();
      return !Equals(item, default(U));
    }
    
    public bool TryGetFirst<U>(Func<U, bool> selector, out U item)
      where U : T
    {
      item = Get<U>().FirstOrDefault(selector);
      return !Equals(item, default(U));
    }
    
    public U GetLastOrDefault<U>()
      where U : T
    {
      return Get<U>().LastOrDefault();
    }
    
    public U GetLastOrDefault<U>(Func<U, bool> selector)
      where U : T
    {
      return Get<U>().LastOrDefault(selector);
    }
    
    public bool TryGetLast<U>(out U item)
      where U : T
    {
      item = Get<U>().LastOrDefault();
      return !Equals(item, default(U));
    }
    
    public bool TryGetLast<U>(Func<U, bool> selector, out U item)
      where U : T
    {
      item = Get<U>().LastOrDefault(selector);
      return !Equals(item, default(U));
    }

    public void Add<U>(U item)
      where U : T
    {
      Add(typeof(U), item);
    }

    public void Add(Type type, object item)
    {
      if (!_inner.ContainsKey(type))
      {
        _inner.Add(type, new ArrayList());
      }

      _inner[type].Add(item);
    }

    public void Remove<U>(Func<U, bool> predicate)
      where U : T
    {
      if (!_inner.ContainsKey(typeof(U)))
      {
        return;
      }

      var match = _inner[typeof(U)].ToArray().FirstOrDefault(x => predicate((U)x));
      if (match != null)
      {
        Remove((U)match);
      }
    }

    public void Remove<U>(U item)
      where U : T
    {
      Remove(typeof(U), item);
    }

    public void Remove(Type type, object item)
    {
      if (!_inner.ContainsKey(type))
      {
        return;
      }

      _inner[type].Remove(item);
    }

    public void Remove<U>()
      where U : T
    {
      Remove(typeof(U));
    }

    public void Remove(Type type)
    {
      _inner[type] = new ArrayList();
    }
    
    public bool  HasAny<U>()
      where U : T
    {
      return Get<U>().FirstOrDefault() != null;
    }

    public TypedListDictionary<T> DeepCopy(Func<T, T> copier)
    {
      return new TypedListDictionary<T>(_inner.ToDictionary(x => x.Key,
        x => new ArrayList(x.Value.ToArray().Select(y => copier((T)y)).ToArray())));
    }
  }
}