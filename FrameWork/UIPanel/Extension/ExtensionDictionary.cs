using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 字典的扩展方法
/// </summary>
public static class ExtensionDictionary 
{
    /// <summary>
    /// 扩展的字典根据key获取value的方法
    /// </summary>
    /// <returns>返回获取到的字典的value，可能为null</returns>
    public static Tvalue TryGet<Tkey,Tvalue>(this Dictionary<Tkey, Tvalue> dict,Tkey key)
    {
        Tvalue value;
        dict.TryGetValue(key, out value);
        return value;
    }

}
