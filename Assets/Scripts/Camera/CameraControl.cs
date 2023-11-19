using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /// <summary>
    /// 获取Frog对应的Transform组件
    /// </summary>
    public Transform Frog;

    /// <summary>
    /// Y轴偏移量
    /// </summary>
    public float offsetY;

    /// <summary>
    /// 相机初始大小
    /// </summary>
    public float zoomBase;

    /// <summary>
    /// 屏幕比例
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
    /// 更新
    /// </summary>
    private void LateUpdate()
    {
        float rPositionY = Frog.transform.position.y + offsetY * m_rRatio;
        transform.position = new Vector3(transform.position.x, rPositionY, transform.position.z);
    }
}
