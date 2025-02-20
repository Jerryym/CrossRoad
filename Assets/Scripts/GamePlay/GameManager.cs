using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	/// <summary>
	/// 分数表
	/// 排序：从大到小
	/// </summary>
	public List<int> m_scoreList;
	/// <summary>
	/// 当前分数
	/// </summary>s
	private int m_iScore;
	/// <summary>
	/// 保存文件的路径
	/// </summary>
	private string m_dataPath;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}

		m_dataPath = Application.persistentDataPath + "/LeaderBoard.json";
		m_scoreList = GetScoreListData();
		DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		EventHandler.GameOverEvent += OnGameOverEvent;
		EventHandler.GetScoreEvent += OnGetScoreEvent;
	}

	private void OnDisable()
	{
		EventHandler.GameOverEvent -= OnGameOverEvent;
		EventHandler.GetScoreEvent -= OnGetScoreEvent;
	}

	private void OnGetScoreEvent(int iScore)
	{
		m_iScore = iScore;
	}

	private void OnGameOverEvent()
	{
		//相同分数不重复记录
		if (m_scoreList.Contains(m_iScore) == false)
		{
			m_scoreList.Add(m_iScore);
		}

		m_scoreList.Sort((x, y) => -x.CompareTo(y));//降序
		File.WriteAllText(m_dataPath, JsonConvert.SerializeObject(m_scoreList));
	}

	/// <summary>
	/// 获取json中数据
	/// </summary>
	/// <returns></returns>
	public List<int> GetScoreListData()
	{
		if (File.Exists(m_dataPath) == true)
		{
			string jsonData = File.ReadAllText(m_dataPath);
			return JsonConvert.DeserializeObject<List<int>>(jsonData);
		}
		return new List<int> { 0 };
	}
}
