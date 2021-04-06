using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FightManager : MonoBehaviour
{
    public GameObject role;
    public GameObject monster;

    private RoleType roleType;
    private MonsterType monsterType;

    private Transform bullet;
    private GameObject fireFall;

    private void Awake()
    {
        Time.timeScale = 1;
        bullet = role.transform.Find("Bullet").transform;
        fireFall = monster.transform.Find("FireFall").gameObject;

        EventCenter.AddListener<int>(EventDefine.RoleAttack, RoleAttack);
        EventCenter.AddListener<int>(EventDefine.MonsterAttack, MonsterAttack);
        //EventCenter.AddListener<int>(EventDefine.RoleAttacked, RoleAttacked);
        //EventCenter.AddListener<int>(EventDefine.MonsterAttacked, MonsterAttacked);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.RoleAttack, RoleAttack);
        EventCenter.RemoveListener<int>(EventDefine.MonsterAttack, MonsterAttack);
        //EventCenter.RemoveListener<int>(EventDefine.RoleAttacked, RoleAttacked);
        //EventCenter.RemoveListener<int>(EventDefine.MonsterAttacked, MonsterAttacked);
    }

    /// <summary>
    /// 初始化主角和怪物的形象信息，存储他们的type
    /// </summary>
    private void Init()
    {
        //TODO
    }

    /// <summary>
    /// 主角攻击
    /// </summary>
    private void RoleAttack(int damage)
    {
        GameObject go = ObjectPool.Instance.Spawn("Bullet", role.transform);
        go.transform.localScale = new Vector3(50, 50, 1);
        go.transform.localPosition = bullet.localPosition;
        go.transform.DOMove(monster.transform.position, 0.3f).OnComplete(
            () => 
            {
                ObjectPool.Instance.UnSpawn(go);
                MonsterAttacked(damage);
            });
    }

    /// <summary>
    /// 主角被攻击
    /// </summary>
    private void RoleAttacked(int damage)
    {
        //TODO  播放被攻击动画
        role.GetComponentInChildren<ParticleSystem>().Play();

    }

    /// <summary>
    /// 怪物攻击
    /// </summary>
    private void MonsterAttack(int damage)
    {
        ParticleSystem particle = fireFall.GetComponent<ParticleSystem>();
        particle.Play();

        RoleAttacked(damage);
    }

    /// <summary>
    /// 怪物被攻击
    /// </summary>
    private void MonsterAttacked(int damage)
    {
        //TODO 播放被攻击动画
        monster.transform.Find("BulletBomb").GetComponent<ParticleSystem>().Play();
    }
}
