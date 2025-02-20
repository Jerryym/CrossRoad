using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    /// <summary>
    /// 汽车模型移动速度
    /// </summary>
    public float Speed = 2.0f;
    /// <summary>
    /// 模型初始位置
    /// </summary>
    private Vector2 m_StartPos;
    /// <summary>
    /// 移动方向（向右 = 1 | 向左 = -1）
    /// </summary>
    private int m_moveDir;

    /// <summary>
    /// 当前视口宽度
    /// </summary>
    private float m_rWidth;
    
    void Start()
    {
        m_rWidth = (float)Camera.main.orthographicSize * 2 * Camera.main.aspect;
        m_StartPos = transform.position;
        transform.localScale = new Vector3(m_moveDir, 1, 1);
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x - m_StartPos.x) > m_rWidth + 15)
        {
            Destroy(this.gameObject);
        }
        Move();
    }

    public void setMoveDirection(int Dir)
    {
		m_moveDir = Dir;
    }

    /// <summary>
    /// 移动函数
    /// </summary>
    private void Move()
    {
        //计算模型位置
        transform.position += transform.right * Speed * Time.deltaTime * m_moveDir;
    }
}
