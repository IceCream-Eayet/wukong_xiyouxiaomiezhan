using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class FightPanel : MonoBehaviour
{
    //主角形象
    public GameObject roleHarmedHp;
    //怪物形象
    public GameObject monsterHarmedHp;
    //血条父物体
    public Transform harmedText;
    //角色血条
    public Slider roleHpSlider;
    //怪物血条
    public Slider MonsterHpSlider;
    public Text timerTxt;

    private Text passLvText;

    private float timerDead = 0;
    private bool isDead = false;
    private bool isRoleDead = false;

    private int roleHp;
    private int monsterHp;
    private int roleDamaged;
    private int monsterDamaged;
    private float timer = 0;
    private int second = 0;
    private int minute = 0;

    public Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(OnClickBackButton);
        roleHp = MainManager.Instance.roleModel.R_Hp;
        monsterHp = MainManager.Instance.monsterDic[MainManager.Instance.roleModel.R_Barriers].M_Hp;


        passLvText = transform.Find("PassLvText").GetComponent<Text>();
        passLvText.text = "第" + MainManager.Instance.roleModel.R_Barriers + "关：" + MainManager.Instance.monsterDic[MainManager.Instance.roleModel.R_Barriers].M_Name;

        EventCenter.AddListener<int>(EventDefine.MonsterAttack, RoleAttacked);
        EventCenter.AddListener<int>(EventDefine.RoleAttack, MonsterAttacked);

    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        minute = (int)(timer / 60);
        second = (int)(timer - minute * 60);
        timerTxt.text = string.Format("{0:D2}:{1:D2}", minute, second);

        if (isDead)  timerDead += Time.fixedDeltaTime;
        if (timerDead > 4.5f)
        {
            isDead = false;
            UIManager.Instance.PushPanel(UIPanelType.GameOverPanel);
            EventCenter.Broadcast(EventDefine.GameOver, monsterDamaged, !isRoleDead);

            Time.timeScale = 0;
        }
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.MonsterAttack, RoleAttacked);
        EventCenter.RemoveListener<int>(EventDefine.RoleAttack, MonsterAttacked);
        UIManager.Instance.DestroyScence();
    }

    /// <summary>
    /// 主角被攻击
    /// </summary>
    private void RoleAttacked(int damage)
    {
        roleDamaged += damage;
        GameObject go = ObjectPool.Instance.Spawn("RoleHarmedHp", harmedText);
        go.transform.localPosition = roleHarmedHp.transform.localPosition;
        go.GetComponent<Text>().text = "-" + damage.ToString();
        go.transform.DOLocalMove(new Vector2(-262, 860), 0.3f).OnComplete(() => { ObjectPool.Instance.UnSpawn(go); });

        roleHpSlider.value = (float)(roleHp - roleDamaged) / roleHp;

        if(roleHp - roleDamaged <= 0)
        {
            //主角死亡，游戏结束
            Debug.Log("主角死亡，游戏结束");

            //播放死亡特效
            //PlayDeadEffect(0);
            AnimManager.Instance.PlayDeadEffect(0);
            isDead = true;
            isRoleDead = true;
        }
    }

    /// <summary>
    /// 怪物被攻击
    /// </summary>
    private void MonsterAttacked(int damage)
    {
        monsterDamaged += damage;

        GameObject go = ObjectPool.Instance.Spawn("MonsterHarmedHp", harmedText);
        go.transform.localPosition = monsterHarmedHp.transform.localPosition;
        go.GetComponent<Text>().text = "-" + damage.ToString();
        go.transform.DOLocalMove(new Vector2(280, 860), 0.3f).OnComplete(() => { ObjectPool.Instance.UnSpawn(go); });

        MonsterHpSlider.value =(float)(monsterHp - monsterDamaged) / monsterHp;

        if(monsterHp - monsterDamaged <= 0)
        {
            //怪物死亡，是否重生
            Debug.Log("怪物死亡，是否重生");

            //播放死亡特效
            //PlayDeadEffect(1);
            AnimManager.Instance.PlayDeadEffect(1);
            isDead = true;
            isRoleDead = false;
        }
    }

    /// <summary>
    /// 返回按钮
    /// </summary>
    private void OnClickBackButton()
    {
        SceneManager.LoadScene(0);
    }

}