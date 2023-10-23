using UnityEngine;
using System;
using System.Collections.Generic;

// Json writing helper class, since Unity's JsonUtility does not write list of objects as top level in json
public class JsonHelper : MonoBehaviour
{
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>
        {
            Items = list
        };
        return JsonUtility.ToJson(wrapper);
    }

    // top level wrapper for list of objects to serialize to json
    [Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}