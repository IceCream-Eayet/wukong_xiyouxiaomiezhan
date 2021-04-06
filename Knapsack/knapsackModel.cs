using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class KnapsackModel 
{
    public List<TreasureBox> BoxInventory = new List<TreasureBox>();
    public List<GoodsItem> GoodsInventory = new List<GoodsItem>();
}

[Serializable]
public class TreasureBox
{
    //品质从1-4，可以开出不同图鉴的碎片，解锁时间Quality^2*10分钟
    public int Quality { get; set; }
    public DateTime StartTime;

}

[Serializable]
public class GoodsItem 
{
    public string Name { get; set; }
    public string ColorStr { get; set; }
    public int Number { get; set; }
}

