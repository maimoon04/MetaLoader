using UnityEngine;
using System.Collections.Generic;

namespace ProjectCore.JsonSerializer
{
    
    public static class JsonHelper 
    {

        // Serialize a single object to JSON
        public static string Serialize<T>(T obj, bool prettyPrint = false)
        {
            return JsonUtility.ToJson(obj, prettyPrint);
        }

        // Deserialize JSON to a single object
        public static T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        // Serialize a list of objects to JSON
        public static string SerializeList<T>(List<T> list, bool prettyPrint = false)
        {
            Wrapper<T> wrapper = new Wrapper<T> { Items = list };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static string SerializeDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict, bool prettyPrint = false)
        {
            var keyValueList = new List<KeyValue<TKey, TValue>>();
            foreach (var kvp in dict)
            {
                keyValueList.Add(new KeyValue<TKey, TValue>(kvp.Key, kvp.Value));
            }

            var wrapper = new WrapperDic<TKey, TValue> { Items = keyValueList };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }
        public static string NewtonSerializeList<T>(List<T> list)
        {
            Wrapper<T> wrapper = new Wrapper<T> { Items = list };
            return Newtonsoft.Json.JsonConvert.SerializeObject(wrapper.Items);
        }

        public static List<T> NewtonDeserializeList<T>(string json)
        {
            Wrapper<T> wrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<Wrapper<T>>(json);
            return wrapper.Items;
        }

        // Deserialize JSON to a list of objects
        public static List<T> DeserializeList<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }
        public static Dictionary<TKey, TValue> DeserializeDictionary<TKey, TValue>(string json)
        {
            var wrapper = JsonUtility.FromJson<WrapperDic<TKey, TValue>>(json);
            var dict = new Dictionary<TKey, TValue>();
            foreach (var kv in wrapper.Items)
            {
                dict[kv.Key] = kv.Value;
            }
            return dict;
        }

        // Internal wrapper class for list serialization
        [System.Serializable]
        private class Wrapper<T>
        {
            public List<T> Items;
        }

        [System.Serializable]
        public class WrapperDic<TKey, TValue>
        {
            public List<KeyValue<TKey, TValue>> Items;
        }
        [System.Serializable]
        public class KeyValue<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;

            public KeyValue(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}