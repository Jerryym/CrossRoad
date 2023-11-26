using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button m_startBtn;

    private void Awake()
    {
        m_startBtn = GetComponent<Button>();
        m_startBtn.onClick.AddListener(StartGame);
    }

    /// <summary>
    /// ��ʼ��Ϸ
    /// </summary>
    private void StartGame()
    {
        //������Ϸ����
        TransitionManager.instance.Transition("GamePlay");
    }
}
