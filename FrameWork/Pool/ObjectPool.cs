using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;
    public static ObjectPool Instance {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
        set => instance = value;
    }

    //资源目录
    public string ResourceDir = "Prefabs";

    public Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();

    //取物体
    public GameObject Spawn(string name,Transform trans)
    {
        if (!m_pools.ContainsKey(name))
        {
            RedieterNew(name,trans);
        }
        SubPool pool = m_pools[name];
        return pool.Spawn();
    }

    //回收物体
    public void UnSpawn(GameObject go)
    {
        SubPool pool = null;
        foreach(var p in m_pools.Values)
        {
            if (p.Contain(go))
            {
                pool = p;
            }
        }
        pool.UnSpawn(go);
    }

    //回收所有
    public void UnSpawnAll()
    {
        foreach(var p in m_pools.Values)
        {
            p.UnSpawnAll();
        }
    }

    //新建一个池子
    void RedieterNew(string names,Transform trans)
    {
        //资源目录
        string path = ResourceDir + "/" + names;
        //生成预制体
        GameObject go = Resources.Load<GameObject>(path);
        //新建一个池子
        SubPool pool = new SubPool(trans,go);
        m_pools.Add(pool.Name, pool);
    }
}
