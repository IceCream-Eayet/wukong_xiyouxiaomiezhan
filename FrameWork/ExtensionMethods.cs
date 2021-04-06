using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    /// <summary>
    /// list扩展方法，添加时判断对象是否已存在，已存在则不做操作
    /// </summary>
    public static void AddExcept(this List<StarItem> gameList , StarItem starItem)
    {
        if (!gameList.Contains(starItem))
        {
            gameList.Add(starItem);
        }
    }

    /// <summary>
    /// list扩展方法，添加时判断对象是否已存在，已存在则不做操作
    /// </summary>
    public static void EnqueueExcept(this Queue<StarItem> gameQueue, StarItem starItem)
    {
        if (!gameQueue.Contains(starItem))
        {
            gameQueue.Enqueue(starItem);
        }
    }

}
