using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class GameManager : MonoBehaviour
{ 
   
    public Transform parent;
    public int xColumn = 7;
    public int yRow = 12;

    private StarItem[,] HeroStar;
    private string[] starColor;

    private Monster monster;

    private bool isFilledFinished = true;
    private bool isMonsterAttrack = true;

    private List<StarItem> starOutList;
    private List<StarItem> starList_1;

    private int timer;


    private void Awake()
    {
        starColor = new string[] 
        {
            "HeroStar", "GreenStar", "OrangeStar", "PurpleStar", "RedStar", "WhiteStar"
        };

        starOutList = new List<StarItem>();
        starList_1 = new List<StarItem>();

        Debug.Log(MainManager.Instance.roleModel.R_Barriers);

        monster = MainManager.Instance.monsterDic[MainManager.Instance.roleModel.R_Barriers];

        EventCenter.AddListener<GameObject>(EventDefine.RemoveStarItem, RemoveStar);
        CreatStarFill();
        
    }
    private void FixedUpdate()
    {
        timer += (int)(Time.fixedDeltaTime * 100);
        if (isMonsterAttrack)
        {
            MonsterAI();
        }

        //如果没有可以消除的格子，则收回下面5行，重新填充
        if (!IsCanEliminate())
        {
            EventCenter.Broadcast(EventDefine.TipsPanel, "没有可以消除的格子了~");
            isFilledFinished = true;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    ObjectPool.Instance.UnSpawn(HeroStar[i, j].gameObject);
                    StarItem starItem = CreatStar(HeroStar[i, j].X, HeroStar[i, j].Y, StarEnum.Empty);
                    HeroStar[i, j] = starItem;
                }
            }
            AllFillStar();
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<GameObject>(EventDefine.RemoveStarItem, RemoveStar);
        ObjectPool.Instance.m_pools.Clear();
    }

    /// <summary>
    /// 星星填充初始化
    /// </summary>
    private void CreatStarFill()
    {
        HeroStar = new StarItem[10, 20];

        for (int i = 0; i < xColumn; i++)
        {
            for (int j = 0; j < yRow; j++) 
            {
                int index = UnityEngine.Random.Range(0, 6);
                StarEnum starType = (StarEnum)Enum.Parse(typeof(StarEnum), starColor[index]);

                StarItem starItem = CreatStar(i, j, starType);
                HeroStar[i, j] = starItem;
            }
        }
    }
    /// <summary>
    /// 生成星星的方法
    /// </summary>
    private StarItem CreatStar(int x, int y, StarEnum starType)
    {
        GameObject go = ObjectPool.Instance.Spawn(starType.ToString(), parent);
        go.transform.position = new Vector2(x - 3, y - 5) * 0.75f;
        StarItem starItem = go.GetComponent<StarItem>();
        starItem.Init(x, y, starType);

        return starItem;
    }
    /// <summary>
    /// 消除符合条件的集合内的对象并重新填充
    /// </summary>
    private void RemoveStar(GameObject go)
    {
        starOutList = new List<StarItem>();
        starList_1 = new List<StarItem>();

        List<StarItem> temp = FindStar(go);

        if (temp.Count >= 2)
        {
            for (int s = 0; s < temp.Count; s++)
            {

                if (temp[s]._explodable != null)
                {
                    temp[s]._explodable.explode();
                    ObjectPool.Instance.UnSpawn(temp[s].gameObject);

                    ExplosionForce ef = FindObjectOfType<ExplosionForce>();
                    ef.doExplosion(temp[s].transform.position);
                }

                StarItem starItem = CreatStar(temp[s].X, temp[s].Y, StarEnum.Empty);
                HeroStar[temp[s].X, temp[s].Y] = starItem;

                isFilledFinished = true;
            }

            //产生伤害，通知场景做出反应
            int damage = temp.Count * ((int)temp[0].starType + 1);
            EventCenter.Broadcast(EventDefine.RoleAttack, damage);
            //EventCenter.Broadcast(EventDefine.MonsterAttacked, damage);

            AllFillStar();
        }
    }

    /// <summary>
    /// 判断是否还有能消除的格子
    /// </summary>
    private bool IsCanEliminate()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                starOutList.Clear();
                starList_1.Clear();
                List<StarItem> temp2 = FindStar(HeroStar[i, j].gameObject);
                if (temp2.Count >= 2)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 全部填充星星
    /// </summary>
    private void AllFillStar()
    {
        //循环填充星星，直到填满
        while (isFilledFinished)
        {
            StepFillStar();
        }
    }
    /// <summary>
    /// 分步填充星星
    /// </summary>
    private bool StepFillStar()
    {
        isFilledFinished = false;

        for (int i = 0; i < xColumn; i++)
        {
            for (int j = yRow-1; j >=1; j--)
            {
                if (HeroStar[i, j].starType != StarEnum.Empty && HeroStar[i, j - 1].starType == StarEnum.Empty) 
                {
                    ObjectPool.Instance.UnSpawn(HeroStar[i, j - 1].gameObject);
                    HeroStar[i, j].Move(i, j - 1);
                    HeroStar[i, j - 1] = HeroStar[i, j];
                    HeroStar[i, j] = CreatStar(i, j, StarEnum.Empty); ;

                    isFilledFinished = true;
                }
            }
        }

        for(int a = 0; a < xColumn; a++)
        {
            if (HeroStar[a, 11].starType == StarEnum.Empty)
            {
                ObjectPool.Instance.UnSpawn(HeroStar[a, 11].gameObject);

                int index = UnityEngine.Random.Range(0, 6);
                StarEnum starType = (StarEnum)Enum.Parse(typeof(StarEnum), starColor[index]);

                HeroStar[a, 11] = CreatStar(a, 11, starType); 

                isFilledFinished = true;
            }
        }

        return isFilledFinished;
    }

    /// <summary>
    /// 获取选中范围内同样对象的集合
    /// </summary>
    private List<StarItem> FindStar(GameObject go)
    {
        Queue<StarItem> queue = new Queue<StarItem>();
        GameObject go_1 = go;

        for (int dd = 0; dd < 100; dd++) 
        {
            int i = go_1.GetComponent<StarItem>().X;
            int j = go_1.GetComponent<StarItem>().Y;

            if (j <= yRow - 5)
            {
                starOutList.AddExcept(HeroStar[i, j]);
                starList_1.AddExcept(HeroStar[i, j]);

                if (i - 1 >= 0 && HeroStar[i - 1, j] != null && HeroStar[i - 1, j].starType == HeroStar[i, j].starType)
                {
                    starOutList.AddExcept(HeroStar[i - 1, j]);
                    queue.EnqueueExcept(HeroStar[i - 1, j]);
                }
                if (j - 1 >= 0 && HeroStar[i, j - 1] != null && HeroStar[i, j - 1].starType == HeroStar[i, j].starType)
                {
                    starOutList.AddExcept(HeroStar[i, j - 1]);
                    queue.EnqueueExcept(HeroStar[i, j - 1]);
                }
                if (i + 1 <= 6 && HeroStar[i + 1, j] != null && HeroStar[i + 1, j].starType == HeroStar[i, j].starType)
                {
                    starOutList.AddExcept(HeroStar[i + 1, j]);
                    queue.EnqueueExcept(HeroStar[i + 1, j]);
                }
                if (j + 1 <= yRow - 5 && HeroStar[i, j + 1] != null && HeroStar[i, j + 1].starType == HeroStar[i, j].starType)
                {
                    starOutList.AddExcept(HeroStar[i, j + 1]);
                    queue.EnqueueExcept(HeroStar[i, j + 1]);
                }
            }

            if (queue.Count >= 1)
            {
                StarItem starItem = queue.Dequeue();
                if (starItem != null)
                {
                    while (true)
                    {
                        if (starList_1.Contains(starItem))
                        {
                            if (queue.Count > 0)
                            {
                                starItem = queue.Dequeue();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            go_1 = starItem.gameObject;
                            break;
                        }
                    }
                }
            }

        }
        return starOutList;
    }
    /// <summary>
    /// 怪物攻击AI
    /// </summary>
    private void MonsterAI()
    {
        if (timer > 100 && timer < 3000 && timer % (monster.M_AttackInterVal * 50) == 0) 
        {
            EventCenter.Broadcast(EventDefine.MonsterAttack, monster.M_Damage);
            //EventCenter.Broadcast(EventDefine.RoleAttacked, monster.M_Damage);
        }
        else if (timer >= 3000 && timer < 4500 && timer % (monster.M_AttackInterVal * 25) == 0)
        {
            EventCenter.Broadcast(EventDefine.MonsterAttack, monster.M_Damage);
            //EventCenter.Broadcast(EventDefine.RoleAttacked, monster.M_Damage);
        }
        else if(timer >= 4500 && timer % (monster.M_AttackInterVal * 10) == 0)
        {
            EventCenter.Broadcast(EventDefine.MonsterAttack, monster.M_Damage);
            //EventCenter.Broadcast(EventDefine.RoleAttacked, monster.M_Damage);
        }
    }
}
