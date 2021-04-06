using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text m_DamageInt;
    public Slider m_ExpSlider;
    public Image m_HeadImage;
    public Text m_AddExpText;
    public Text m_ResultText;
    public Text m_TreasureBox;

    private Button m_ContinueButton;
    private Button m_BackButton;

    /// <summary>
    /// 进入界面后执行的方法
    /// </summary>
    public override void OnEnter()
    {
        m_ContinueButton = transform.Find("ContinueButton").GetComponent<Button>();
        m_ContinueButton.onClick.AddListener(OnClickContinueButton);

        m_BackButton = transform.Find("BackButton").GetComponent<Button>();
        m_BackButton.onClick.AddListener(OnClickBackButton);

        int exping = MainManager.Instance.ExpToLv(0);
        int expLv = 35 * (MainManager.Instance.roleModel.R_Lv + 1) * (MainManager.Instance.roleModel.R_Lv + 1) - 113 * (MainManager.Instance.roleModel.R_Lv + 1) + 300;
        m_ExpSlider.value = (float)exping / expLv;

        EventCenter.AddListener<int, bool>(EventDefine.GameOver, GameOver);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<int, bool>(EventDefine.GameOver, GameOver);
    }

    private void GameOver(int damage, bool isWin)
    {
        if (!isWin)
        {
            m_ResultText.text = "再接再厉！";
        }
        else
        {
            m_ResultText.text = "胜利！";
            MainManager.Instance.roleModel.R_Barriers++;
        }

        DropBox();
        m_DamageInt.text = damage.ToString();

        int GetExp = damage / 10;
        m_AddExpText.text = GetExp.ToString();

        int exping= MainManager.Instance.ExpToLv(GetExp);
        int Lv = MainManager.Instance.roleModel.R_Lv;
        float valuetemp = m_ExpSlider.value;

        while ((float)exping / MainManager.Instance.FuncLvToExp(Lv + 1) >= 1) 
        {
            exping -= MainManager.Instance.FuncLvToExp(Lv + 1);
            StartCoroutine(UpdateSlider(valuetemp, 1));
            Lv++;
        }

        StartCoroutine(UpdateSlider(valuetemp, (float)exping / MainManager.Instance.FuncLvToExp(Lv + 1)));
        MainManager.Instance.UpdateExp(GetExp);
    }

    IEnumerator UpdateSlider(float valuetemp,float index)
    {
        for (float t = 0; t <= 2f; t += Time.fixedDeltaTime) 
        {
            m_ExpSlider.value = Mathf.Lerp(valuetemp, index, t / 0.3f);
            yield return 0;
        }
    }

    /// <summary>
    /// 随机掉落宝箱
    /// </summary>
    private void DropBox()
    {
        if (MainManager.Instance.knapsackModel.BoxInventory.Count < 3)
        {
            TreasureBox treasure = new TreasureBox
            {
                Quality = UnityEngine.Random.Range(1, 5),
                StartTime = DateTime.Now
            };

            MainManager.Instance.UpdateKnapsackMsg(treasure, null, true);

            m_TreasureBox.text = "宝箱余额不足，已给您充值一个！";
        }
        else
        {
            m_TreasureBox.text = "宝箱充足，去背包开启它吧！";
        }
    }

    /// <summary>
    /// 继续游戏按钮
    /// </summary>
    private void OnClickContinueButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// 返回主页按钮
    /// </summary>
    private void OnClickBackButton()
    {
        SceneManager.LoadScene("MainScene");
    }
}
