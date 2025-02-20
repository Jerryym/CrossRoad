using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	/// <summary>
	/// 移动方向（向右 = 1 | 向左 = -1）
	/// </summary>
	public int direction;
	/// <summary>
	/// 模型对象列表
	/// </summary>
	public List<GameObject> spawnObjects;

	private void Start()
	{
		InvokeRepeating(nameof(Spawn), 0.2f, Random.Range(5f, 8f));
	}

	/// <summary>
	/// 随机生成gameObj
	/// </summary>
	private void Spawn()
	{
		var iIndex = Random.Range(0, spawnObjects.Count);
		var spawObj = Instantiate(spawnObjects[iIndex], transform.position, Quaternion.identity, transform);
		spawObj.GetComponent<MoveForward>().setMoveDirection(direction);//设置模型移动方向
	}
}
