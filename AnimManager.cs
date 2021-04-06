using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    private static AnimManager instance;
    public static AnimManager Instance
    {
        get
        {
            return instance;
        }
    }

    private GameObject roleDeadEffect;
    private GameObject monsterDeadEffect;

    private void Start()
    {
        instance = this;
        roleDeadEffect = transform.Find("RoleDead").gameObject;
        monsterDeadEffect = transform.Find("MonsterDead").gameObject;
        roleDeadEffect.SetActive(false);
        monsterDeadEffect.SetActive(false);
        roleDeadEffect.GetComponent<Animator>().SetBool("RoleDead", false);
        monsterDeadEffect.GetComponent<Animator>().SetBool("IsDead", false);
    }

    // <summary>
    // 播放死亡之后的特效和音乐，0代表role死亡，1代表monster死亡
    // </summary>
    public void PlayDeadEffect(int whoDead)
    {
        switch (whoDead)
        {
            case 0:
                roleDeadEffect.SetActive(true);
                roleDeadEffect.GetComponent<Animator>().SetBool("RoleDead", true);
                break;
            case 1:
                monsterDeadEffect.SetActive(true);
                monsterDeadEffect.GetComponent<Animator>().SetBool("IsDead", true);
                break;
            default:
                break;
        }
    }
}
