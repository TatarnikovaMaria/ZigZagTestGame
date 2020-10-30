using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();

	public void AddPool(GameObject prefab, int poolAmount)
    {
		if (pool.ContainsKey(prefab.name))
			return;

		List<GameObject> newPool = new List<GameObject>();

		for (int i = 0; i < poolAmount; i++)
		{
			GameObject obj = (GameObject)Instantiate(prefab);
			obj.SetActive(false);
			newPool.Add(obj);
		}
		pool.Add(prefab.name, newPool);
	}

	public GameObject Spawn(Vector3 spawnPoint, Quaternion spawnRotation, GameObject prefab)
	{
		string poolName = prefab.name;
		for (int i = 0; i < pool[poolName].Count; i++)
		{
			if (!pool[poolName][i].activeInHierarchy)
			{
				return ActivateGO(spawnPoint, spawnRotation, pool[poolName][i]);
			}
		}

		GameObject obj = (GameObject)Instantiate(prefab);
		pool[poolName].Add(obj);
		return ActivateGO(spawnPoint, spawnRotation, obj);
	}

	private GameObject ActivateGO(Vector3 spawnPoint, Quaternion spawnRotation, GameObject obj)
    {
		obj.transform.position = spawnPoint;
		obj.transform.rotation = spawnRotation;
		obj.SetActive(true);
		return obj;
	}

	public void DeactivateGO(GameObject obj)
	{
		obj.SetActive(false);
	}

	public void DisableAllObjects()
    {
		foreach (KeyValuePair<string, List<GameObject>> pair in pool)
		{
			for (int i = 0; i < pair.Value.Count; i++)
			{
				pair.Value[i].SetActive(false);
			}
		}
	}
}

