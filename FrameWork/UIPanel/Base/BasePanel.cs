using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BasePanel : MonoBehaviour
{

    /// <summary>
    /// 进入界面后执行的方法
    /// </summary>
    public virtual void OnEnter()
    {

    }
    /// <summary>
    /// 暂停页面后执行的方法
    /// </summary>
    public virtual void OnPause()
    {

    }
    /// <summary>
    /// 恢复页面后执行的方法
    /// </summary>
    public virtual void OnResume()
    {

    }
    /// <summary>
    /// 关闭界面后执行的方法
    /// </summary>
    public virtual void OnExit()
    {

    }
}
