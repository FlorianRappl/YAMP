using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// A custom ReadOnlyDictionary for obvious reasons.
    /// </summary>
    /// <typeparam name="TKey">The key type to use.</typeparam>
    /// <typeparam name="TValue">The value type to use.</typeparam>
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Fields

        IDictionary<TKey, TValue> _dictionary;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new read only dictionary.
        /// </summary>
        public ReadOnlyDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Creates a new read only dictionary from another dictionary.
        /// </summary>
        /// <param name="dictionary">The existing dictionary.</param>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// Cannot add an entry... Caution: Will give you an exception.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException("This dictionary is read-only.");
        }

        /// <summary>
        /// Does the dictionary contain a certain key?
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>The search result.</returns>
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gives you a collection of keys.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _dictionary.Keys; }
        }

        /// <summary>
        /// Cannot remove an entry... Caution: Will give you an exception.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            throw new NotSupportedException("This dictionary is read-only.");
        }

        /// <summary>
        /// Tries to get a value!
        /// </summary>
        /// <param name="key">The key for the value.</param>
        /// <param name="value">The value or default(TValue) if nothing was found.</param>
        /// <returns>The status of the search.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gives you a collection of values.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }

        /// <summary>
        /// Gets a value of a certain key. Caution: Setting is not possible (Exception).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                throw new NotSupportedException("This dictionary is read-only.");
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Cannot add an entry... Caution: Will give you an exception.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("This dictionary is read-only.");
        }

        /// <summary>
        /// Cannot clear the dictionary.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException("This dictionary is read-only.");
        }

        /// <summary>
        /// Does the dictionary contain a certain key?
        /// </summary>
        /// <param name="item">The key value pair to search for.</param>
        /// <returns>The search result.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        /// <summary>
        /// Copies the dictionary to an array of key value pairs.
        /// </summary>
        /// <param name="array">Th target to copy to.</param>
        /// <param name="arrayIndex">The index to start.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of entries in the dictionary.
        /// </summary>
        public int Count
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// Gets the status of the dictionary - YES: it is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Cannot remove an entry... Caution: Will give you an exception.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("This dictionary is read-only.");
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Gets the enumerator for this dictionary.
        /// </summary>
        /// <returns>The IEnumerator.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (_dictionary as System.Collections.IEnumerable).GetEnumerator();
        }

        #endregion
    }
}
