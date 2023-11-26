using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    /// <summary>
    /// ����ģ���ƶ��ٶ�
    /// </summary>
    public float Speed = 2.0f;
    /// <summary>
    /// ģ�ͳ�ʼλ��
    /// </summary>
    private Vector2 m_StartPos;
    /// <summary>
    /// �ƶ��������� = 1 | ���� = -1��
    /// </summary>
    private int m_moveDir;

    /// <summary>
    /// ��ǰ�ӿڿ��
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
    /// �ƶ�����
    /// </summary>
    private void Move()
    {
        //����ģ��λ��
        transform.position += transform.right * Speed * Time.deltaTime * m_moveDir;
    }
}
