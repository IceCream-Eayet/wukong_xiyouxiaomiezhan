using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IllustrateModel
{
    public List<Illustrate> illustrates = new List<Illustrate>();
}

[Serializable]
public class Illustrate
{
    public string Name { get; set; }
    public int Id { get; set; }

    //-1表示不能解锁，0表示可以解锁但是未解锁，1表示已经解锁
    public string IsUnlock { get; set; }
    public int BonusPercent { get; set; }
    public int HpAddition { get; set; }
    public string IllustrateType { get; set; }
}
