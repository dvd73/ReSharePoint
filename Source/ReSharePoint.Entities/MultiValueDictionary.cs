using System;
using System.Collections.Generic;

namespace ReSharePoint.Entities
{
    public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            if (!TryAdd(key, value))
            {
                throw new Exception(String.Format("Value [{0}] for key [{1}] already exists", value, key));
            }
        }

        public bool TryAdd(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                base.Add(key, new List<TValue>() { value });
            else
            {
                if (this[key].Contains(value))
                    return false;
                else
                    this[key].Add(value);
            }
            return true;
        }

        public List<TValue> GetValues(TKey key)
        {
            if (ContainsKey(key))
                return this[key];
            else
            {
                return new List<TValue>();
            }
        }

        public IEnumerable<TValue> GetAllValues()
        {
            foreach (KeyValuePair<TKey, List<TValue>> keyValPair in this)
                foreach (TValue val in keyValPair.Value)
                    yield return val;
        }
    }
}
