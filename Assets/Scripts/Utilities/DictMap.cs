using System;
using System.Linq;
using System.Collections.Generic;
using ElysiumAttributes;
namespace ElysiumUtilities
{
	[Serializable]
	[ExcludeFromNodeBuild]
	public class Dict<TKey, TValue>
	{
		[UnityEngine.SerializeField]
		private TKey key;
		[UnityEngine.SerializeField]
		private TValue value;

		public TKey Key
		{
			get => key;
			set => key = value;
		}

		public TValue Value
		{
			get => value;
			set => this.value = value;
		}

		public Dict(TKey newKey, TValue newValue)
		{
			Key = newKey;
			Value = newValue;
		}
	}

	[Serializable]
	[ExcludeFromNodeBuild]
	public class Map<TKey, TValue>
	{
		[UnityEngine.SerializeField]
		public List<Dict<TKey, TValue>> dictionary = new List<Dict<TKey, TValue>>();

		public TValue this[TKey key]
		{
			get
			{
				int index = dictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
				if (index != -1)
				{
					return dictionary[index].Value;
				}
				return default;
			}
			set
			{
				int index = dictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
				if (index != -1)
				{
					dictionary[index].Value = value;
				}
			}
		}
		public bool TryGetValue(TKey key, out TValue value)
		{
			int index = dictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
			if (index != -1)
			{
				value = dictionary[index].Value;
				return true;
			}
			value = default;
			return false;
		}

		public void Add(TKey key, TValue value)
		{
			dictionary.Add(new Dict<TKey, TValue>(key, value));
		}

		public bool Remove(TKey key)
		{
			int index = dictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
			if (index != -1)
			{
				dictionary.RemoveAt(index);
				return true;
			}
			return false;
		}

		public bool Exists(TKey key)
		{
			return dictionary.Exists(dict => dict.Key.ToString() == key.ToString());
		}

		public void Clear()
		{
			dictionary.Clear();
		}

		public void TrimExcess()
		{
			dictionary.TrimExcess();
		}

	}


	/// Special variant of Map where the TValue is a List of TValues, Useful when you have 1 key holding multiple values
	[Serializable]
	[ExcludeFromNodeBuild]
	public class MultiMap<TKey, TValue>
	{
		[UnityEngine.SerializeField]
		public List<Dict<TKey, List<TValue>>> multiDictionary = new List<Dict<TKey, List<TValue>>>();

		public List<TValue> this[TKey key]
		{
			get
			{
				int index = multiDictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
				if (index != -1)
				{
					return multiDictionary[index].Value;
				}
				return default;
			}
			set
			{
				int index = multiDictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
				if (index != -1)
				{
					multiDictionary[index].Value = value;
				}
			}
		}
		public bool TryGetValue(TKey key, out List<TValue> value)
		{
			int index = multiDictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
			if (index != -1)
			{
				value = multiDictionary[index].Value;
				return true;
			}
			value = default;
			return false;
		}

		//Adds a new key reference to the dictionary
		public void Add(TKey key, List<TValue> value)
		{
			multiDictionary.Add(new Dict<TKey, List<TValue>>(key, value));
		}

		//Adds an item in an already existing key, this does not check if the key is valid or not
		public void AddInExistingKey(TKey key, TValue value)
		{
			this[key].Add(value);
		}

		public void RemoveInExistingKey(TKey key, TValue value)
		{
			this[key].Remove(value);
		}

		public bool Remove(TKey key)
		{
			int index = multiDictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
			if (index != -1)
			{
				multiDictionary.RemoveAt(index);
				return true;
			}
			return false;
		}

		public void OrderBy(TKey key,Comparison<TValue> match)
		{
			this[key].Sort(match);
		}

		public bool Exists(TKey key)
		{
			return multiDictionary.Exists(dict => dict.Key.ToString() == key.ToString());
		}

		public void Clear()
		{
			multiDictionary.Clear();
		}

		public void TrimExcess()
		{
			multiDictionary.TrimExcess();
		}

	}

	/// Special variant of Map where the TValue is a List of TValues, Useful when you have 1 key holding multiple values
	[Serializable]
	[ExcludeFromNodeBuild]
	public class MultiHashMap<TKey, TValue>
	{
		[UnityEngine.SerializeField]
		public List<Dict<TKey, HashSet<TValue>>> multiDictionary = new List<Dict<TKey, HashSet<TValue>>>();

		public HashSet<TValue> this[TKey key]
		{
			get
			{
				int index = multiDictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
				if (index != -1)
				{
					return multiDictionary[index].Value;
				}
				return default;
			}
			set
			{
				int index = multiDictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
				if (index != -1)
				{
					multiDictionary[index].Value = value;
				}
			}
		}
		public bool TryGetValue(TKey key, out HashSet<TValue> value)
		{
			int index = multiDictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
			if (index != -1)
			{
				value = multiDictionary[index].Value;
				return true;
			}
			value = default;
			return false;
		}

		//Adds a new key reference to the dictionary
		public void Add(TKey key, HashSet<TValue> value)
		{
			multiDictionary.Add(new Dict<TKey, HashSet<TValue>>(key, value));
		}

		//Adds an item in an already existing key, this does not check if the key is valid or not
		public void AddInExistingKey(TKey key, TValue value)
		{
			this[key].Add(value);
		}

		public void RemoveInExistingKey(TKey key, TValue value)
		{
			this[key].Remove(value);
		}

		public bool Remove(TKey key)
		{
			int index = multiDictionary.FindIndex(dict => key.ToString() == dict.Key.ToString());
			if (index != -1)
			{
				multiDictionary.RemoveAt(index);
				return true;
			}
			return false;
		}

		public bool Exists(TKey key)
		{
			return multiDictionary.Exists(dict => dict.Key.ToString() == key.ToString());
		}

		public void Clear()
		{
			multiDictionary.Clear();
		}

		public void TrimExcess()
		{
			multiDictionary.TrimExcess();
		}

	}

}
