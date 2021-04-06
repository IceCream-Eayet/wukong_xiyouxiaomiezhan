using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipsPanel : MonoBehaviour
{
    private Text tips;
    private float timer = 0;
    private bool isStarTime = false;

    private void Awake()
    {
        tips = transform.Find("Tips").GetComponent<Text>();
        transform.DOScale(Vector3.zero, 0f);
        EventCenter.AddListener<string>(EventDefine.TipsPanel, CallTips);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.TipsPanel, CallTips);
    }

    private void Update()
    {
        if (isStarTime)
        {
            timer += Time.deltaTime;
        }

        if (timer > 1.5f)
        {
            transform.DOScale(Vector3.zero, 1.5f);
        }
    }

    private void CallTips(string tips)
    {
        this.tips.text = tips;
        isStarTime = true;
        transform.DOScale(Vector3.one, 0.1f);
    }

}
