using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject GameOverPanel;
    public GameObject LeaderBoardPanel;

    private void OnEnable()
    {
        Time.timeScale = 1.0f;
        EventHandler.GetScoreEvent += OnGetScoreEvent;
        EventHandler.GameOverEvent += OnGameOverEvent;
    }

    private void OnDisable()
    {
        EventHandler.GetScoreEvent -= OnGetScoreEvent; 
        EventHandler.GameOverEvent -= OnGameOverEvent;
    }

    private void Start()
    {
        //��ʼ���ı�
        scoreText.text = "00";
    }

    #region Button Event
    /// <summary>
    /// ���¿�ʼ��Ϸ
    /// </summary>
    public void ReStartGame()
    {
        GameOverPanel.SetActive(false);
        TransitionManager.instance.Transition(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// �������˵�
    /// </summary>
    public void BackToMenu()
    {
        GameOverPanel.SetActive(false);
        TransitionManager.instance.Transition("MainWindow");
    }

    public void OpenLeaderBoard()
    {
        LeaderBoardPanel.SetActive(true);
    }

    #endregion

    private void OnGetScoreEvent(int obj)
    {
        //�޸ķ����ı�
        scoreText.text = obj.ToString();
    }

    private void OnGameOverEvent()
    {
        //����GameOverPanel
        GameOverPanel.SetActive(true);
        //��ͣ��Ϸ
        if (GameOverPanel.activeInHierarchy == true)
        {
            Time.timeScale = 0.0f;
        }
    }
}
