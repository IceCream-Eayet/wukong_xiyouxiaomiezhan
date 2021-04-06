using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnackPanel : BasePanel
{
    private CanvasGroup canvasGroup;
    private GameObject boxOPenPanel;

    public List<GameObject> Boxes;
    public List<GameObject> Slots;

    public bool isOpenBox = false;

    private void Awake()
    {
        gameObject.AddComponent<CanvasGroup>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        boxOPenPanel = transform.Find("BoxOpenPanel").gameObject;
        boxOPenPanel.SetActive(false);

        EventCenter.AddListener<int[]>(EventDefine.OpenBox, OpenBox);

        UpdateGoodsInventoryMessage();
    }
    private void OnDestroy()
    {
        EventCenter.AddListener<int[]>(EventDefine.OpenBox, OpenBox);
    }

    private void Update()
    {
        UpdateBoxInventoryMessage();
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
        UpdateBoxInventoryMessage();
        UpdateGoodsInventoryMessage();
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

    /// <summary>
    /// 背包页面初始化宝箱部分
    /// </summary>
    private void UpdateBoxInventoryMessage()
    {
        if (isOpenBox)
        {
            foreach (GameObject boxGo in Boxes)
            {
                boxGo.GetComponent<Button>().enabled = false;
                boxGo.GetComponentInChildren<Text>().text = "";
                boxGo.GetComponent<Image>().color = Color.white;
            }
        }

        isOpenBox = false;

        for (int i = 0; i < MainManager.Instance.knapsackModel.BoxInventory.Count; i++)
        {
            if (MainManager.Instance.knapsackModel.BoxInventory[i] != null)
            {
                TreasureBox box = MainManager.Instance.knapsackModel.BoxInventory[i];

                if (box != null)
                {
                    int UnlockTime = box.Quality * box.Quality * 10;
                    DateTime dateTime = box.StartTime.AddMinutes(UnlockTime);
                    Boxes[i].GetComponent<Image>().color = new Color(0.2f, 0.1f, 0.06f) * box.Quality;

                    if (dateTime <= DateTime.Now)
                    {
                        Boxes[i].GetComponentInChildren<Text>().text = "可开启";
                        Boxes[i].GetComponent<Button>().enabled = true;
                    }
                    else
                    {
                        Boxes[i].GetComponent<Button>().enabled = false;
                        int hours = (dateTime - DateTime.Now).Hours;
                        int minutes = (dateTime - DateTime.Now).Minutes;
                        int seconds = (dateTime - DateTime.Now).Seconds;
                        Boxes[i].GetComponentInChildren<Text>().text = string.Format("{0:D2}：{1:D2}:{2:D2}", hours, minutes, seconds);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 背包页面初始化物品部分
    /// </summary>
    private void UpdateGoodsInventoryMessage()
    {
        for(int i = 0; i < MainManager.Instance.knapsackModel.GoodsInventory.Count; i++)
        {
            if (MainManager.Instance.knapsackModel.GoodsInventory[i] != null)
            {
                GoodsItem item = MainManager.Instance.knapsackModel.GoodsInventory[i];
                ColorUtility.TryParseHtmlString("#" + item.ColorStr, out Color prizeColor);

                Slots[i].GetComponent<Image>().color = prizeColor;
                Slots[i].GetComponentInChildren<Text>().text = item.Name + "-" + item.Number.ToString();
            }
        }
    }

    /// <summary>
    /// 开启宝箱
    /// </summary>
    private void OpenBox(int[] num)
    {
        boxOPenPanel.SetActive(true);
        List<GoodsItem> itmeTemps = new List<GoodsItem>();

        foreach (int n in num) 
        {
            GoodsItem item = new GoodsItem();
            switch (n)
            {
                case 0:
                    item.Name = "白色碎片";
                    item.ColorStr = ColorUtility.ToHtmlStringRGBA(Color.white);
                    item.Number = 1;
                    MainManager.Instance.UpdateKnapsackMsg(null, item, true);
                    itmeTemps.Add(item);
                   break;
                case 1:
                    item.Name = "绿色碎片";
                    item.ColorStr = ColorUtility.ToHtmlStringRGBA(Color.green);
                    item.Number = 1;
                    MainManager.Instance.UpdateKnapsackMsg(null, item, true);
                    itmeTemps.Add(item);
                    break;
                case 2:
                    item.Name = "蓝色碎片";
                    item.ColorStr = ColorUtility.ToHtmlStringRGBA(Color.blue);
                    item.Number = 1;
                    MainManager.Instance.UpdateKnapsackMsg(null, item, true);
                    itmeTemps.Add(item);
                    break;
                case 3:
                    item.Name = "紫色碎片";
                    item.ColorStr = ColorUtility.ToHtmlStringRGBA(Color.magenta);
                    item.Number = 1;
                    MainManager.Instance.UpdateKnapsackMsg(null, item, true);
                    itmeTemps.Add(item);
                    break;
                case 4:
                    item.Name = "金色碎片";
                    item.ColorStr = ColorUtility.ToHtmlStringRGBA(Color.yellow);
                    item.Number = 1;
                    MainManager.Instance.UpdateKnapsackMsg(null, item, true);
                    itmeTemps.Add(item);
                    break;
                case 5:
                    item.Name = "红色碎片";
                    item.ColorStr = ColorUtility.ToHtmlStringRGBA(Color.red);
                    item.Number = 1;
                    MainManager.Instance.UpdateKnapsackMsg(null, item, true);
                    itmeTemps.Add(item);
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < itmeTemps.Count; i++) 
        {
            GameObject prize = boxOPenPanel.transform.Find("Prize").Find("Prize_0" + (i+1).ToString()).gameObject;

            ColorUtility.TryParseHtmlString("#" + itmeTemps[i].ColorStr, out Color prizeColor);

            prize.GetComponent<Image>().color = prizeColor;
            prize.GetComponentInChildren<Text>().text = itmeTemps[i].Name;
        }

        isOpenBox = true;
        UpdateGoodsInventoryMessage();
    }

}
