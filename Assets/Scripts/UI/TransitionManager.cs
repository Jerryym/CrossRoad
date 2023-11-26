using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    private CanvasGroup canvasGroup;
    public float scaler = 2.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        //不要删除当前物体
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //启动协程
        StartCoroutine(Fade(0));
    }

    public void Transition(string sceneName)
    {
        Time.timeScale = 1.0f;
        //启动协程
        StartCoroutine(TransitionToScene(sceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        yield return Fade(1);

        yield return SceneManager.LoadSceneAsync(sceneName);

        yield return Fade(0);
    }

    private IEnumerator Fade(int iAmount)
    {
        canvasGroup.blocksRaycasts = true;
        while (canvasGroup.alpha != iAmount)
        {
            switch (iAmount)
            {
                case 0:
                    canvasGroup.alpha -= Time.deltaTime * scaler;
                    break;
                case 1:
                    canvasGroup.alpha += Time.deltaTime * scaler;
                    break;
            }
            yield return null;
        }
        canvasGroup.blocksRaycasts = false;
    }
}
