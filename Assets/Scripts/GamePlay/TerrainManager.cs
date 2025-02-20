using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
	/// <summary>
	/// 地形坐标与相机坐标的偏移差值
	/// </summary>
	public float offsetY;
	/// <summary>
	/// 地形对象列表
	/// </summary>
	public List<GameObject> terrainObjs;
	
	/// <summary>
	/// 当前生成的地形对象
	/// </summary>
	private GameObject terrainObj;
	/// <summary>
	/// 上一次生成的地形对应的List索引
	/// </summary>
	private int m_lastIndex = -1;
	/// <summary>
	/// 上一次生成的地形对应的坐标
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
	/// 检测地形坐标，若满足条件，则生成新的地形
	/// </summary>
	public void CheckPosition()
	{
		if (transform.position.y - Camera.main.transform.position.y < offsetY / 2)
		{
			//计算地形的坐标
			transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY, 0);
			//生成新的地形
			SpawnTerrain();
		}
	}

	/// <summary>
	/// 随机生成地形
	/// </summary>
	private void SpawnTerrain()
	{
		var iIndex = Random.Range(0, terrainObjs.Count);
		//使随机生成的地形不会出现两个相同的地形
		while (iIndex == m_lastIndex)
		{
			iIndex = Random.Range(0, terrainObjs.Count);
		}
		terrainObj = terrainObjs[iIndex];

		//第一次出现的场景不可为Road
		if (m_lastIndex == -1 && terrainObj.name == "Road")
		{
			while (terrainObj.name == "Road")
			{
				iIndex = Random.Range(0, terrainObjs.Count);
				terrainObj = terrainObjs[iIndex];
			}
		}

		//判断新的地形是否和当前地形重叠
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

		//实例化新地形
		m_lastIndex = iIndex;
		m_LastPosition = transform.position;
		Instantiate(terrainObj, transform.position, Quaternion.identity);
	}

}
