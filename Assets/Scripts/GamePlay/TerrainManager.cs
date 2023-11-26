using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    /// <summary>
    /// ������������������ƫ�Ʋ�ֵ
    /// </summary>
    public float offsetY;
    /// <summary>
    /// ���ζ����б�
    /// </summary>
    public List<GameObject> terrainObjs;
    
    /// <summary>
    /// ��ǰ���ɵĵ��ζ���
    /// </summary>
    private GameObject terrainObj;
    /// <summary>
    /// ��һ�����ɵĵ��ζ�Ӧ��List����
    /// </summary>
    private int m_lastIndex;

    private void OnEnable()
    {
        EventHandler.GetScoreEvent += OnGetScoreEvent;
    }

    private void OnDisable()
    {
        EventHandler.GetScoreEvent -= OnGetScoreEvent;
    }

    private void OnGetScoreEvent(int obj)
    {
        CheckPosition();
    }

    /// <summary>
    /// ���������꣬�������������������µĵ���
    /// </summary>
    public void CheckPosition()
    {
        if (transform.position.y - Camera.main.transform.position.y < offsetY / 2)
        {
            //������ε�����
            transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY, 0);
            //�����µĵ���
            SpawnTerrain();
        }
    }

    /// <summary>
    /// ������ɵ���
    /// </summary>
    private void SpawnTerrain()
    {
        var iIndex = Random.Range(0, terrainObjs.Count);
        //ʹ������ɵĵ��β������������ͬ�ĵ���
        while (iIndex == m_lastIndex)
        {
            iIndex = Random.Range(0, terrainObjs.Count);
        }
        m_lastIndex = iIndex;
        terrainObj = terrainObjs[iIndex];
        Instantiate(terrainObj, transform.position, Quaternion.identity);
    }

}
