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
    /// 开始游戏
    /// </summary>
    private void StartGame()
    {
        //启动游戏场景
        TransitionManager.instance.Transition("GamePlay");
    }
}
