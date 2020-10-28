using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class CrystalPooler : MonoBehaviour
{
    public GameObject crystalPerfab;    
    public int startPoolAmount = 5;

    private ObjectPooler objPooler;

    public static CrystalPooler instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            this.enabled = false;
    }

    public void Init()
    {
        objPooler = gameObject.GetComponent<ObjectPooler>();

        if (crystalPerfab != null)
        {
            objPooler.AddPool(crystalPerfab, startPoolAmount);
        }
        else
        {
            Debug.LogError("Set prefab in the CrystalPooler component!");
        }

    }

    public void SpawnCrystal(Vector3 spawnPoint)
    {
        objPooler.Spawn(spawnPoint, Quaternion.identity, crystalPerfab);
    }

    public void DeactivateCrystal(GameObject crystal)
    {
        objPooler.DeactivateGO(crystal);
    }
}
