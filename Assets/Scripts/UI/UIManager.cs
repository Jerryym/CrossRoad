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
		//初始化文本
		scoreText.text = "00";
	}

	#region Button Event
	/// <summary>
	/// 重新开始游戏
	/// </summary>
	public void ReStartGame()
	{
		GameOverPanel.SetActive(false);
		TransitionManager.instance.Transition(SceneManager.GetActiveScene().name);
	}

	/// <summary>
	/// 返回主菜单
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
		//修改分数文本
		scoreText.text = obj.ToString();
	}

	private void OnGameOverEvent()
	{
		//激活GameOverPanel
		GameOverPanel.SetActive(true);
		//暂停游戏
		if (GameOverPanel.activeInHierarchy == true)
		{
			Time.timeScale = 0.0f;
		}
	}
}
