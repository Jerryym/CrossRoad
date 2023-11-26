using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventHandler
{
    /// <summary>
    /// �÷��¼�
    /// </summary>
    public static event Action<int> GetScoreEvent;
    public static void CallGetScoreEvent(int iScore)
    {
        if (GetScoreEvent != null)
        {
            GetScoreEvent.Invoke(iScore);
        }
    }

    /// <summary>
    /// ������Ϸ�����¼�
    /// </summary>
    public static event Action GameOverEvent;
    public static void CallGameOverEvent()
    {
        if (GameOverEvent != null)
        {
            GameOverEvent.Invoke();
        }
    }
}
