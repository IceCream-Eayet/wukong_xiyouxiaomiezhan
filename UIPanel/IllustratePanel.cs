using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllustratePanel : BasePanel
{
    private CanvasGroup canvasGroup;
    public Transform i_comment;
    private List<GameObject> illustrateGo;
    private List<Illustrate> illustrateList;

    private void Awake()
    {
        gameObject.AddComponent<CanvasGroup>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();

        illustrateGo = new List<GameObject>();
        illustrateList = new List<Illustrate>();

        CreatElement();
    }

    #region 继承至父类框架的方法
    /// <summary>
    /// 进入界面后执行的方法
    /// </summary>
    public override void OnEnter()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }
    /// <summary>
    /// 暂停页面后执行的方法
    /// </summary>
    public override void OnPause()
    {
        //弹出新的页面时，禁止当前页面的交互
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// 恢复页面后执行的方法
    /// </summary>
    public override void OnResume()
    {
        //恢复当前页面时，恢复当前页面的交互
        canvasGroup.blocksRaycasts = true;
        InitElement();
    }
    /// <summary>
    /// 关闭界面后执行的方法
    /// </summary>
    public override void OnExit()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
    #endregion

    private void CreatElement()
    {
       foreach(var ill in MainManager.Instance.illustrateDic)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Element"), i_comment);

            string name = ill.Value.Name;
            int id = ill.Value.Id;
            string isUnlock = ill.Value.IsUnlock;
            go.GetComponent<IllustrateElement>().InitElement(name, id, isUnlock);

            illustrateGo.Add(go);
        }
    }

    private void InitElement()
    {
        foreach (var ill in MainManager.Instance.illustrateDic)
        {
            illustrateList.Clear();
            illustrateList.Add(ill.Value);
        }

        for (int i = 0; i < illustrateGo.Count; i++) 
        {
            string name = illustrateList[i].Name;
            int id = illustrateList[i].Id;
            string isUnlock = illustrateList[i].IsUnlock;

            illustrateGo[i].GetComponent<IllustrateElement>().InitElement(name, id, isUnlock);
        }
    }
}
