using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoxButton : MonoBehaviour
{
    private Button button;
    private int[] prizeNum = new int[4];
    private float rotateSpeed = 0.1f;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(BoxButtonOnClick);
        button.enabled = false;
    }

    private void BoxButtonOnClick()
    {
        string name = gameObject.name;
        string[] names = name.Split('0');
        int index = int.Parse(names[1]) - 1;

        Debug.Log(index);
        //掉落碎片：0代表白色碎片，1代表绿色碎片，2代表蓝色碎片，3代表紫色碎片，4代表金色碎片，5代表粉红色碎片
        if (MainManager.Instance.knapsackModel.BoxInventory[index] != null) 
        {
            TreasureBox box = MainManager.Instance.knapsackModel.BoxInventory[index];
            MainManager.Instance.UpdateKnapsackMsg(box, null, false);

            int a = 0 + box.Quality - 1;
            int b = 3 + box.Quality - 1;
            if (b > 6) b = 6;

            prizeNum[0] = Random.Range(a, b);
            prizeNum[1] = Random.Range(a, b);
            prizeNum[2] = Random.Range(a, b);
            prizeNum[3] = Random.Range(a, b);
        }

        StartCoroutine(ShakeOpen());
    }

    IEnumerator ShakeOpen()
    {
        int shakeNum = 10;
        for(int i = 0; i < 20; i++)
        {
            if (shakeNum == 10)
            {
                shakeNum = -10;
                gameObject.transform.DOLocalRotate(new Vector3(0, 0, shakeNum), rotateSpeed);
            }
            else
            {
                shakeNum = 10;
                gameObject.transform.DOLocalRotate(new Vector3(0, 0, shakeNum), rotateSpeed);
            }
            yield return new WaitForSeconds(rotateSpeed);
        }

        button.enabled = false;
        GetComponentInChildren<Text>().text = "";
        GetComponent<Image>().color = Color.white;
        gameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.01f);

        EventCenter.Broadcast(EventDefine.OpenBox, prizeNum);
    }
}
