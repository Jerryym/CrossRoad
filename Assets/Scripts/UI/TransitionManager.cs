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

        //��Ҫɾ����ǰ����
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //����Э��
        StartCoroutine(Fade(0));
    }

    public void Transition(string sceneName)
    {
        Time.timeScale = 1.0f;
        //����Э��
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
