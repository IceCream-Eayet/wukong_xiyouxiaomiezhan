using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml.Linq;

public class RoleModel 
{
    /// <summary>
    /// 昵称
    /// </summary>
    private string r_Name;
    public string R_Name
    {
        get => r_Name;
        set
        {
            r_Name = value;
            WriteRoleMsgXml();
        }
    }
    /// <summary>
    /// 等级
    /// </summary>
    private int r_Lv;
    public int R_Lv
    {
        get => r_Lv;
        set
        {
            r_Lv = value;
            WriteRoleMsgXml();
        }
    }
    /// <summary>
    /// 经验值
    /// </summary>
    private int r_Exp;
    public int R_Exp
    {
        get => r_Exp;
        set
        {
            r_Exp = value;
            WriteRoleMsgXml();
        }
    }
    /// <summary>
    /// 血量
    /// </summary>
    private int r_Hp;
    public int R_Hp
    {
        get => r_Hp;
        set
        {
            r_Hp = value;
            WriteRoleMsgXml();
        }
    }
    /// <summary>
    /// 伤害加成
    /// </summary>
    private float r_BonusPercent;
    public float R_BonusPercent
    {
        get => r_BonusPercent;
        set
        {
            r_BonusPercent = value;
            WriteRoleMsgXml();
        }
    }
    /// <summary>
    /// 基础伤害值
    /// </summary>
    private int r_Damege;
    public int R_Damege
    {
        get => r_Damege;
        set
        {
            r_Damege = value;
            WriteRoleMsgXml();
        }
    }

    /// <summary>
    /// 当前挑战关卡
    /// </summary>
    private int r_Barriers;
    public int R_Barriers
    {
        get => r_Barriers;
        set
        {
            r_Barriers = value;
            WriteRoleMsgXml();
        }
    }

    /// <summary>
    /// 角色信息存储的路径
    /// </summary>
    private readonly string path = Application.persistentDataPath  + "/RoleMsg.Xml";


    public RoleModel()
    {
        Debug.Log(path);
        LoadRoleMsgByXml();
    }
    /// <summary>
    /// 角色信息初始化
    /// </summary>
    private void Init(string R_Name, int R_Lv,int R_Exp, int R_Hp,float R_BonusPercent, int R_Damege, int R_Barriers)
    {
        this.R_Name = R_Name;
        this.R_Lv = R_Lv;
        this.R_Exp = R_Exp;
        this.R_BonusPercent = R_BonusPercent;
        this.R_Damege = R_Damege;
        this.R_Hp = R_Hp;
        this.R_Barriers = R_Barriers;
    }
    /// <summary>
    /// 属性值更新计算
    /// </summary>
    public void AttributeSet()
    {
        R_Hp = 2000;
        R_BonusPercent = 0;
        R_Damege = 10;

        foreach (var ill in MainManager.Instance.illustrateDic)
        {
            if (ill.Value.IsUnlock == "1")
            {
                R_Hp += ill.Value.HpAddition;
                R_BonusPercent += ill.Value.BonusPercent;
            }
        }

        R_Damege += R_Lv * 2;
    }

    /// <summary>
    /// 从xml文件读取角色存档
    /// </summary>
    public void LoadRoleMsgByXml()
    {
        if (!File.Exists(path))
        {
            WriteRoleMsgXml();
        }

        XElement root = XElement.Load(path);
        R_Name = root.Element("R_Name").Value;
        R_Lv = (int)root.Element("R_Lv");
        R_Exp = (int)root.Element("R_Exp");
        R_BonusPercent = (float)root.Element("R_BonusPercent");
        R_Damege = (int)root.Element("R_Damege");
        R_Hp = (int)root.Element("R_Hp");
        R_Barriers = (int)root.Element("R_Barriers");
    }

    /// <summary>
    /// 写入角色信息到存档文件
    /// </summary>
    private void WriteRoleMsgXml()
    {
        if (!File.Exists(path))
        {
            File.Create(path).Close();
            Init("xiaoleng", 0, 0, 2000, 0, 10, 1);
            WriteXml();
        }

        WriteXml();
    }
    /// <summary>
    /// 创建XML对象并写入文件
    /// </summary>
    private void WriteXml()
    {
        XDocument element = new XDocument(
            new XElement("Role",
                new XElement("R_Name",R_Name),
                new XElement("R_Lv", R_Lv),
                new XElement("R_Exp", R_Exp),
                new XElement("R_Hp", R_Hp),
                new XElement("R_BonusPercent", R_BonusPercent),
                new XElement("R_Damege", R_Damege),
                new XElement("R_Barriers", R_Barriers))
            );

        element.Save(path);
    }
}
