using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private void Awake()
    {
        //开启主页面
        UIManager.Instance.PushPanel(UIPanelType.MainPanel);

        //若第一次运行，复制必要文件到存档目录
        if (!File.Exists(Application.persistentDataPath + "/IllustratedJson.json"))
        {
            StartCoroutine(MainManager.Instance.CopyStreamingAssetsFileIE());
        }
        else
        {
            //初始化图鉴信息
            MainManager.Instance.InitillustrateDic();
        }

        //初始化怪物信息
        MainManager.Instance.InitMonsterDic();
        //初始化背包信息
        MainManager.Instance.InitknapsackModel();
        //重计算玩家属性数值
        MainManager.Instance.roleModel.AttributeSet();
    }
}
