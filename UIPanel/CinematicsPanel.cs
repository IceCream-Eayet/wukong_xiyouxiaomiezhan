using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CinematicsPanel : BasePanel
{

    private Slider loadSlider;

    void Awake()
    {
        loadSlider = transform.Find("LoadSlider").GetComponent<Slider>();

       StartCoroutine(LoadGameScene());
    }

    #region 继承至父类框架的方法
    /// <summary>
    /// 进入界面后执行的方法
    /// </summary>
    public override void OnEnter()
    {

    }
    /// <summary>
    /// 暂停页面后执行的方法
    /// </summary>
    public override void OnPause()
    {

    }
    /// <summary>
    /// 恢复页面后执行的方法
    /// </summary>
    public override void OnResume()
    {

        
    }
    /// <summary>
    /// 关闭界面后执行的方法
    /// </summary>
    public override void OnExit()
    {

    }
    #endregion

    IEnumerator LoadGameScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("GameScene");
        async.allowSceneActivation = false;

        while (loadSlider.value < 0.5f)
        {
            loadSlider.value += 0.01f;

            yield return new WaitForSeconds(0.04f);
        }


        while (async.progress < 0.9f)
        {
            loadSlider.value = async.progress;

            if (loadSlider.value >= 0.9f)
            {
                loadSlider.value = 0.9f;
            }

            yield return new WaitForSeconds(1f);
        }

        loadSlider.value = 1f;
        async.allowSceneActivation = true;
    }
}
