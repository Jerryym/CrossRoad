using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /// <summary>
    /// ������
    /// ���򣺴Ӵ�С
    /// </summary>
    public List<int> m_scoreList;
    /// <summary>
    /// ��ǰ����
    /// </summary>s
    private int m_iScore;
    /// <summary>
    /// �����ļ���·��
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
        //��ͬ�������ظ���¼
        if (m_scoreList.Contains(m_iScore) == false)
        {
            m_scoreList.Add(m_iScore);
        }

        m_scoreList.Sort((x, y) => -x.CompareTo(y));//����
        File.WriteAllText(m_dataPath, JsonConvert.SerializeObject(m_scoreList));
    }

    /// <summary>
    /// ��ȡjson������
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
