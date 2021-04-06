using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 读取UIPanel路径信息的json使用到的可序列化对象
/// </summary>
[Serializable]
public class UIPanelInfo
{
    public List<PanelInfo> PanelInfoList = new List<PanelInfo>();
}

[Serializable]
public class PanelInfo:ISerializationCallbackReceiver
{
    //不能反序列化的元素
    [NonSerialized]
    public UIPanelType PanelType;

    //反序列化的元素
    public string PanelTypelString;
    public string Path;

    //在反序列化之后调用，转换读取到的string为UIPanelType
    public void OnAfterDeserialize()
    {
        UIPanelType type = (UIPanelType)Enum.Parse(typeof(UIPanelType),PanelTypelString);
        PanelType = type;
    }

    public void OnBeforeSerialize()
    {
        
    }
}
