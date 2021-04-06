using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustrateElement : MonoBehaviour
{
    private Image i_Elementimage;
    private Button i_UnlockButton;
    private Text i_NameText;
    private int i_IllustrateId;
    private string i_IsUnlock;
    private ParticleSystem i_UnlockParticle;

    private void Awake()
    {
        i_Elementimage = transform.Find("Role").GetComponent<Image>();
        i_UnlockButton = transform.Find("Unlock_Btn").GetComponent<Button>();
        i_NameText = transform.Find("Name_Txt").GetComponent<Text>();
        i_UnlockParticle = transform.Find("UnlockParticle").GetComponent<ParticleSystem>();

        i_UnlockButton.onClick.AddListener(OnClickUnLock);
        EventCenter.AddListener(EventDefine.OnClickUnLock, IsCanUnlock);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.OnClickUnLock, IsCanUnlock);
    }

    public void InitElement(string name, int illustrateId , string isUnlock)
    {
        switch (isUnlock)
        {
            case "-1":
                i_UnlockButton.gameObject.SetActive(true);
                i_UnlockButton.interactable = false;
                i_UnlockParticle.Stop();
                i_Elementimage.color = Color.grey;
                break;
            case "0":
                i_UnlockButton.gameObject.SetActive(true);
                i_UnlockButton.interactable = true;
                i_UnlockParticle.Play();
                i_Elementimage.color = Color.grey;
                break;
            case "1":
                i_UnlockButton.gameObject.SetActive(false);
                i_UnlockButton.interactable = false;
                i_UnlockParticle.Stop();
                i_Elementimage.color = Color.white;
                break;
        }

        i_IllustrateId = illustrateId;
        i_NameText.text = name;
        i_IsUnlock = isUnlock;

        IsCanUnlock();
    }

    private void IsCanUnlock()
    {
        //需碎片：白色：3倍序号，绿色：2倍序号，蓝色：1倍序号，紫色：1倍序号，金色：1倍序号，红色：待定
        int tempNum = 0;
        foreach(var item in MainManager.Instance.knapsackModel.GoodsInventory)
        {
            switch (item.Name)
            {
                case "白色碎片":
                    if (item.Number >= i_IllustrateId * 3) tempNum++;
                    break;
                case "绿色碎片":
                    if (item.Number >= i_IllustrateId * 2) tempNum++;
                    break;
                case "蓝色碎片":
                    if (item.Number >= i_IllustrateId * 1) tempNum++;
                    break;
                case "紫色碎片":
                    if (item.Number >= i_IllustrateId * 1) tempNum++;
                    break;
                case "金色碎片":
                    if (item.Number >= i_IllustrateId * 1) tempNum++;
                    break;
                case "红色碎片":
                    break;
            }
        }

        if (tempNum == 5 && i_IsUnlock == "-1") 
        {
            i_UnlockButton.interactable = true;
            i_UnlockParticle.Play();
        }
        else if(i_IsUnlock != "0")
        {
            i_UnlockButton.interactable = false;
            i_UnlockParticle.Stop();
        }
    }

    private void OnClickUnLock()
    {
        i_UnlockButton.gameObject.SetActive(false);
        i_UnlockButton.interactable = false;
        i_Elementimage.color = Color.white;

        string[] goodNames = { "白色碎片", "绿色碎片", "蓝色碎片", "紫色碎片", "金色碎片", "红色碎片" };
        int[] multiple = { 3, 2, 1, 1, 1, 0 };
        for(int i = 0; i < 5; i++)
        {
            GoodsItem goods = new GoodsItem
            {
                Name = goodNames[i],
                Number = i_IllustrateId * multiple[i]
            };

            MainManager.Instance.UpdateKnapsackMsg(null, goods, false);
        }

        i_UnlockParticle.Stop();
        MainManager.Instance.UpdateIsUnlockIll(i_IllustrateId, "1");
        EventCenter.Broadcast(EventDefine.OnClickUnLock);
    }
}
