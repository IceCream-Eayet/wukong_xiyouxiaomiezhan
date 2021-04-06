using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SubPool 
{
    //集合
    List<GameObject> m_objecs = new List<GameObject>();

    //预设
    GameObject m_prefab;

    //名字
    public string Name => m_prefab.name;

    //父物体的位置
    Transform m_parent;

    public SubPool(Transform parent, GameObject go)     //构造函数
    {
        m_prefab = go;
        m_parent = parent;
    }

    //取出物体
    public GameObject Spawn()
    {
        GameObject go = null;
        foreach(var obj in m_objecs)
        {
            if (!obj.activeSelf)
            {
                go = obj;
            }
        }

        if (go == null)
        {
            go = GameObject.Instantiate(m_prefab, m_parent,true);
            //go.transform.SetParent(m_parent,true);
            //go.transform.parent = m_parent;
            m_objecs.Add(go);
        }
        go.SetActive(true);
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        return go;
    }

    //回收物体
    public void UnSpawn(GameObject go)
    {
        if (Contain(go))
        {
            go.SendMessage("OnUnSpawn", SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }

    //回收所有
    public void UnSpawnAll()
    {
        foreach(var obj in m_objecs)
        {
            if (obj.activeSelf)
            {
                UnSpawn(obj);
            }
        }
    }

    //判断是否属于list里边
    public bool Contain(GameObject go)
    {
        return m_objecs.Contains(go);
    }
}
