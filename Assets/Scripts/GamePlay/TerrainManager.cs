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
    private int m_lastIndex = -1;
    /// <summary>
    /// ��һ�����ɵĵ��ζ�Ӧ������
    /// </summary>
    private Vector3 m_LastPosition;

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
        terrainObj = terrainObjs[iIndex];

        //��һ�γ��ֵĳ�������ΪRoad
        if (m_lastIndex == -1 && terrainObj.name == "Road")
        {
            while (terrainObj.name == "Road")
            {
                iIndex = Random.Range(0, terrainObjs.Count);
                terrainObj = terrainObjs[iIndex];
            }
        }

        //�ж��µĵ����Ƿ�͵�ǰ�����ص�
        if (m_lastIndex != -1)
        {
            var LastBoxSize = terrainObjs[m_lastIndex].GetComponent<BoxCollider2D>().size;
            var NewBoxSize = terrainObj.GetComponent<BoxCollider2D>().size;
            float rNewOffsetY = (LastBoxSize.y + NewBoxSize.y) / 2 + 0.5f;
            transform.position = new Vector3(0, m_LastPosition.y + rNewOffsetY, 0);

            Debug.Log("1.2-Name: " + terrainObjs[m_lastIndex].name + ", position = " + m_LastPosition + "rNewOffsetY: " + rNewOffsetY);
            Debug.Log("2.2-Name: " + terrainObj.name + ", position = " + transform.position);
        }
        else
        {
            var BoxSize = terrainObjs[iIndex].GetComponent<BoxCollider2D>().size;
            float rNewOffsetY = BoxSize.y / 2 + offsetY / 2  + 1.2f;
            transform.position = new Vector3(0, Camera.main.transform.position.y + rNewOffsetY, 0);

            Debug.Log("1.1-Name: " + terrainObjs[iIndex].name + ", position = " + m_LastPosition + "rNewOffsetY: " + rNewOffsetY);
            Debug.Log("2.1-Name: " + terrainObj.name + ", position = " + transform.position);
        }

        //ʵ�����µ���
        m_lastIndex = iIndex;
        m_LastPosition = transform.position;
        Instantiate(terrainObj, transform.position, Quaternion.identity);
    }

}
