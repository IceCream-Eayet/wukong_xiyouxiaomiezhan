using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class MainManager
{
    //单例模式
    private static MainManager instance;
    public static MainManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MainManager();
            }
            return instance;
        }
        set => instance = value;
    }

    /// <summary>
    /// 怪物信息存储字典
    /// </summary>
    public Dictionary<int, Monster> monsterDic;

    /// <summary>
    /// 角色信息存储类
    /// </summary>
    public RoleModel roleModel;
    /// <summary>
    /// 背包信息存储类
    /// </summary>
    public KnapsackModel knapsackModel;
    /// <summary>
    /// 图鉴信息存储的字典
    /// </summary>
    public Dictionary<int, Illustrate> illustrateDic;

    /// <summary>
    /// 图鉴信息存储类
    /// </summary>
    private IllustrateModel illustrateModel;

    /// <summary>
    /// 存档信息存储的路径
    /// </summary>
    private readonly string persistentPath = Application.persistentDataPath;

    private MainManager()
    {
        knapsackModel = new KnapsackModel();
        illustrateModel = new IllustrateModel();
        roleModel = new RoleModel();
    }

    /// <summary>
    /// 更新背包存储信息
    /// </summary>
    public void UpdateKnapsackMsg(TreasureBox treasure, GoodsItem goods, bool isAdd)
    {
        if (isAdd)
        {
            if (treasure != null)
            {
                knapsackModel.BoxInventory.Add(treasure);
            }

            if (goods != null)
            {
                bool isContains = false;
                knapsackModel.GoodsInventory.ForEach(c =>
                {
                    if (c.Name == goods.Name)
                    {
                        isContains = true;
                        c.Number += goods.Number;
                    }
                });
                if (!isContains) 
                    knapsackModel.GoodsInventory.Add(goods);
            }
        }
        else
        {
            if (treasure != null)
                knapsackModel.BoxInventory.Remove(treasure);
            if (goods != null) 
            {
                knapsackModel.GoodsInventory.ForEach(c =>
                {
                    if (c.Name == goods.Name && c.Number - goods.Number >= 0)
                    {
                        c.Number -= goods.Number;
                    }
                });
            }
        }

        SaveknapsackModel();
    }

    /// <summary>
    /// 更新图鉴解锁信息
    /// </summary>
    public void UpdateIsUnlockIll(int id, string isUnlock)
    {
        illustrateDic[id].IsUnlock = isUnlock;
        SaveIllustrateModel();
        roleModel.AttributeSet();
    }

    /// <summary>
    /// 更新主角经验值，并判断是否可以升级
    /// </summary>
    public void UpdateExp(int Exp)
    {
        int exping = ExpToLv(Exp);

        //更新主角等级
        while (exping >= FuncLvToExp(roleModel.R_Lv + 1))
        {
            roleModel.R_Lv++;
            exping -= FuncLvToExp(roleModel.R_Lv + 1);
        }
        roleModel.AttributeSet();
    }

    /// <summary>
    /// 通过获得的经验值计算当前剩余的经验值
    /// </summary>
    public int ExpToLv(int Exp)
    {
        roleModel.R_Exp += Exp;

        //当前等级已消耗的经验值
        int expForLv = Func(roleModel.R_Lv);

        //当前剩余经验值
        int exping = roleModel.R_Exp - expForLv;
        return exping;
    }
    /// <summary>
    /// 计算当前等级已消耗的总经验值
    /// </summary>
    private int Func(int lved)
    {
        if (lved <= 0)
        {
            return 0;
        }

        int temp = Func(lved - 1) + FuncLvToExp(lved);

        return temp;
    }
    /// <summary>
    /// 计算当前等级升级所需要的经验值
    /// </summary>
    public int FuncLvToExp(int lv)
    {
        return 35 * lv * lv - 113 * lv + 300;
    }
    /// <summary>
    /// 初始化怪物存储字典
    /// </summary>
    public void InitMonsterDic()
    {
        //从json文件读取monster信息
        TextAsset text = Resources.Load<TextAsset>("MonsterJson");
        JsonReader reader = new JsonReader(text.text);
        MonsterModel monsterModel = JsonMapper.ToObject<MonsterModel>(reader);

        monsterDic = new Dictionary<int, Monster>();
        foreach (var monster in monsterModel.monsters)
        {
            monsterDic.Add(monster.M_Id, monster);
        }
    }
    /// <summary>
    /// 初始化背包信息存储类
    /// </summary>
    public void InitknapsackModel()
    {
        if (!File.Exists(persistentPath + "/KnapsackJson.json"))
        {
            File.Create(persistentPath + "/KnapsackJson.json").Close();
            SaveknapsackModel();
        }
        StreamReader sr = new StreamReader(persistentPath + "/KnapsackJson.json");
        string str = sr.ReadToEnd();

        JsonReader reader = new JsonReader(str);
        knapsackModel = JsonMapper.ToObject<KnapsackModel>(reader);

        sr.Close();
    }

    /// <summary>
    /// 存储背包信息
    /// </summary>
    private void SaveknapsackModel()
    {
        StreamWriter sw = new StreamWriter(persistentPath + "/KnapsackJson.json");
        string text = JsonMapper.ToJson(knapsackModel);

        sw.Write(text);
        sw.Close();
    }

    //读取json文件中图鉴元素的信息初始化图鉴类
    public void InitillustrateDic()
    {
        //从json文件读取monster信息
        StreamReader sr = new StreamReader(persistentPath + "/IllustratedJson.json");
        string str = sr.ReadToEnd();

        JsonReader reader = new JsonReader(str);
        illustrateModel = JsonMapper.ToObject<IllustrateModel>(reader);

        illustrateDic = new Dictionary<int, Illustrate>();
        foreach (var ill in illustrateModel.illustrates)
        {
            illustrateDic.Add(ill.Id, ill);
        }

        sr.Close();
    }

    //存储图鉴元素的信息到json文件
    private void SaveIllustrateModel()
    {
        StreamWriter sw = new StreamWriter(persistentPath + "/IllustratedJson.json");
        string text = JsonMapper.ToJson(illustrateModel);

        sw.Write(text);
        sw.Close();
    }

    //从streaming文件夹copy存档文件到相关目录
    public IEnumerator CopyStreamingAssetsFileIE()
    {
        string streamingPath = GetRightstreamingPath();
        string fileTemp = "";

        UnityWebRequest request = UnityWebRequest.Get(streamingPath + "/IllustratedJson.json");
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error); //打印错误原因
        }
        else //请求成功
        {
            Debug.Log("Get:请求成功");
            fileTemp = request.downloadHandler.text;
        }

        if (Directory.Exists(persistentPath))
        {
            File.Create(persistentPath + "/IllustratedJson.json").Close();
            File.AppendAllText(persistentPath + "/IllustratedJson.json", fileTemp);
        }
        else
        {
            Debug.Log(persistentPath + "目标路径不存在!");
        }

        InitillustrateDic();
    }

    private string GetRightstreamingPath()
    {
        string streamingPath = Application.streamingAssetsPath;

#if UNITY_Editor
        streamingPath = Application.streamingAssetsPath;
#endif

#if UNITY_IOS
        Debug.Log("Iphone");
        streamingPath = Application.dataPath + "/Raw";
#endif

#if UNITY_Android
        Debug.Log("UNITY_ANDROID");
        streamingPath = "jar:file:/" + Application.dataPath + "!/assets";
#endif
        return streamingPath;
    }
}
