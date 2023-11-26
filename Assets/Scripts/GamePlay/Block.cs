using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    /// <summary>
    /// 当前视口高度
    /// </summary>
    private float m_rHeight;

    private void Start()
    {
        m_rHeight = Camera.main.orthographicSize * 2;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        if (Camera.main.transform.position.y - transform.position.y > m_rHeight + 15)
        {
            Destroy(this.gameObject);
        }
    }
}
