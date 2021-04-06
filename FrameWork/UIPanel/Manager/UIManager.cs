using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //单例模式
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    //Canvas信息
    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
        set
        {
            canvasTransform = value;
        }
    }

    /// <summary>
    /// 存储解析到的面板路径
    /// </summary>
    private Dictionary<UIPanelType, string> UIPanelPathDic;
    /// <summary>
    /// 存储已实例化的的Panel身上的BasePanel组件
    /// </summary>
    private Dictionary<UIPanelType, BasePanel> basePanelDic;
    /// <summary>
    /// 存储已显示的Panel身上的BasePanel组件
    /// </summary>
    private Stack<BasePanel> basePanelSta;

    //私有的构造函数，禁止在外界构造此类
    private UIManager()
    {
        ParseUIPanelInfoJson();
    }

    /// <summary>
    /// 场景被销毁时清空字典和堆
    /// </summary>
    public void DestroyScence()
    {
        if(basePanelDic!=null)
            basePanelDic.Clear();
        if(basePanelSta!=null)
            basePanelSta.Clear();
    }

    /// <summary>
    /// 把需要显示的页面入栈
    /// </summary>
    /// <param name="panelType"></param>
    public void PushPanel(UIPanelType panelType)
    {
        if (basePanelSta == null)
            basePanelSta = new Stack<BasePanel>();
        //如果栈中已有页面，把它暂停
        if (basePanelSta.Count > 0)
            basePanelSta.Peek().OnPause();

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        basePanelSta.Push(panel);
    }
    /// <summary>
    /// 出栈，把页面从界面上移除
    /// </summary>
    public void PopPanel()
    {
        if (basePanelSta == null)
            basePanelSta = new Stack<BasePanel>();

        //栈中界面数不为0，则取出顶部界面，把它关闭
        if (basePanelSta.Count <= 0) return;
        BasePanel toppanel = basePanelSta.Pop();
        toppanel.OnExit();

        //如果栈中还有界面，把它恢复
        if (basePanelSta.Count > 0)
            basePanelSta.Peek().OnResume();
    }

    /// <summary>
    /// 根据UIPanelType获取Panel身上的BasePanel组件
    /// </summary>
    /// <returns>返回Panel身上的BasePanel组件</returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (basePanelDic == null)
            basePanelDic = new Dictionary<UIPanelType, BasePanel>();

        BasePanel panel = basePanelDic.TryGet(panelType);
        if (panel == null)
        {
            string path = UIPanelPathDic.TryGet(panelType);
            GameObject panelGo = Object.Instantiate(Resources.Load(path)) as GameObject;
            panelGo.transform.SetParent(CanvasTransform, false);
            panel = panelGo.GetComponent<BasePanel>();

            basePanelDic.Add(panelType, panel);
        }

        return panel;
    }

    /// <summary>
    /// 加载json配置文件并存储到字典
    /// </summary>
    private void ParseUIPanelInfoJson()
    {
        //加载资源文件夹下的json文件
        TextAsset InfoJson = Resources.Load<TextAsset>("UIPanelJson");
        //实例化有个新的空字典
        UIPanelPathDic = new Dictionary<UIPanelType, string>();
        //读取json文件，把读取到的信息赋值给字典
        UIPanelInfo uIPanelInfo = JsonUtility.FromJson<UIPanelInfo>(InfoJson.text);
        foreach (var info in uIPanelInfo.PanelInfoList)
        {
            UIPanelPathDic.Add(info.PanelType, info.Path);
        }
    }

}
