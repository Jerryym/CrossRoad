using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
	public List<ScoreRecord> scores;
	private List<int> m_iScoreList;

	private void OnEnable()
	{
		m_iScoreList = GameManager.instance.GetScoreListData();
	}

	private void Start()
	{
		SetLeaderBoardData();
	}

	public void SetLeaderBoardData()
	{
		for (int i = 0; i < scores.Count; i++)
		{
			if (i < m_iScoreList.Count)
			{
				scores[i].SetScoreText(m_iScoreList[i]);
				scores[i].gameObject.SetActive(true);
			}
			else
			{
				scores[i].gameObject.SetActive(false);
			}
		}
	}
}
