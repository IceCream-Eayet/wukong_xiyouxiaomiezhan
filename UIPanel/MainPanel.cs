using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public Button Story_Btn;
    public Button Knack_Btn;
    public Button Illustrate_Btn;
    public Button Start_Btn;
    public Button Shop_Btn;

    private Text HP;
    private Text Damage;
    private Text LvText;

    private CanvasGroup canvasGroup;
    
    void Awake()
    {
        Time.timeScale = 1;
        gameObject.AddComponent<CanvasGroup>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        HP = transform.Find("Role").Find("HPText").GetComponent<Text>();
        Damage = transform.Find("Role").Find("DamageText").GetComponent<Text>();
        LvText = transform.Find("Role").Find("LvText").GetComponent<Text>();

        Story_Btn.onClick.AddListener(OnClickStoryButton);
        Knack_Btn.onClick.AddListener(OnClickKnackButton);
        Illustrate_Btn.onClick.AddListener(OnClickIllustrateButton);
        Start_Btn.onClick.AddListener(OnClickStartButton);
        Shop_Btn.onClick.AddListener(OnClickShopButton);
    }

    #region 继承至父类框架的方法
    /// <summary>
    /// 进入界面后执行的方法
    /// </summary>
    public override void OnEnter()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        Init();
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
        Init();
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

    private void Init()
    {
        HP.text = "血量：" + MainManager.Instance.roleModel.R_Hp.ToString();
        Damage.text = "伤害：" + MainManager.Instance.roleModel.R_Damege.ToString();
        LvText.text = "等级：" + MainManager.Instance.roleModel.R_Lv.ToString();
    }

    private void OnDestroy()
    {
        UIManager.Instance.DestroyScence();
    }

    private void OnClickStoryButton()
    {

    }

    private void OnClickKnackButton()
    {
        UIManager.Instance.PushPanel(UIPanelType.KnackPanel);
    }

    private void OnClickIllustrateButton()
    {
        UIManager.Instance.PushPanel(UIPanelType.IllustratePanel);
    }

    private void OnClickStartButton()
    {
        UIManager.Instance.PushPanel(UIPanelType.CinematicsPanel);
    }

    private void OnClickShopButton()
    {

    }
}
