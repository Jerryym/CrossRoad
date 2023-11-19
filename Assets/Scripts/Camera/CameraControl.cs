using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /// <summary>
    /// ��ȡFrog��Ӧ��Transform���
    /// </summary>
    public Transform Frog;

    /// <summary>
    /// Y��ƫ����
    /// </summary>
    public float offsetY;

    /// <summary>
    /// �����ʼ��С
    /// </summary>
    public float zoomBase;

    /// <summary>
    /// ��Ļ����
    /// </summary>
    private float m_rRatio;

    /// <summary>
    ///
    /// </summary>
    private void Start()
    {
        m_rRatio = (float)Screen.height/ (float)Screen.width;
        Camera.main.orthographicSize = zoomBase;
    }

    /// <summary>
    /// ����
    /// </summary>
    private void LateUpdate()
    {
        float rPositionY = Frog.transform.position.y + offsetY * m_rRatio;
        transform.position = new Vector3(transform.position.x, rPositionY, transform.position.z);
    }
}
