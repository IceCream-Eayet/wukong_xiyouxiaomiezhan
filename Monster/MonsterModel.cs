using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MonsterModel 
{
    public List<Monster> monsters = new List<Monster>();
}

[Serializable]
public struct Monster
{
    public string M_Name { get; set; }
    public int M_Id { get; set; }
    public int M_Hp { get; set; }
    public int M_Damage { get; set; }
    public int M_AttackInterVal { get; set; }
}
