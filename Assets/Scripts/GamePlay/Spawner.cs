using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    /// <summary>
    /// �ƶ��������� = 1 | ���� = -1��
    /// </summary>
    public int direction;
    /// <summary>
    /// ģ�Ͷ����б�
    /// </summary>
    public List<GameObject> spawnObjects;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0.2f, Random.Range(5f, 8f));
    }

    /// <summary>
    /// �������gameObj
    /// </summary>
    private void Spawn()
    {
        var iIndex = Random.Range(0, spawnObjects.Count);
        var spawObj = Instantiate(spawnObjects[iIndex], transform.position, Quaternion.identity, transform);
        spawObj.GetComponent<MoveForward>().setMoveDirection(direction);//����ģ���ƶ�����
    }
}
