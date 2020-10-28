using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class TilePooler : MonoBehaviour
{
    public List<GameObject> tileSetPerfabs;     //please, set prefabs from easy difficulty to hard
    public int startPoolAmount = 5;

    private ObjectPooler objPooler;

    public static TilePooler instance;

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

        switch(GameManager.instance.usedDifficulty)
        {
            case gameDifficulty.Easy:
                if (tileSetPerfabs.Count > 0)
                {
                    objPooler.AddPool(tileSetPerfabs[0], startPoolAmount);
                }
                else
                {
                    Debug.LogError("Set tile set prefabs in the TilePooler component!");
                }
                break;

            case gameDifficulty.Medium:
                if (tileSetPerfabs.Count > 1)
                {
                    objPooler.AddPool(tileSetPerfabs[1], startPoolAmount);
                }
                else
                {
                    Debug.LogError("Set tile set prefabs in the TilePooler component!");
                }
                break;

            case gameDifficulty.Hard:
                if (tileSetPerfabs.Count > 2)
                {
                    objPooler.AddPool(tileSetPerfabs[2], startPoolAmount);
                }
                else
                {
                    Debug.LogError("Set tile set prefabs in the TilePooler component!");
                }
                break;
        }

    }
    public GameObject SpawnTileSet(Vector3 spawnPoint, Quaternion spawnRotation)
    {
        switch (GameManager.instance.usedDifficulty)
        {
            case gameDifficulty.Easy:
                if (tileSetPerfabs.Count > 0)
                {
                    return objPooler.Spawn(spawnPoint, spawnRotation, tileSetPerfabs[0]);
                }
                break;

            case gameDifficulty.Medium:
                if (tileSetPerfabs.Count > 1)
                {
                    return objPooler.Spawn(spawnPoint, spawnRotation, tileSetPerfabs[1]);
                }
                break;

            case gameDifficulty.Hard:
                if (tileSetPerfabs.Count > 2)
                {
                    return objPooler.Spawn(spawnPoint, spawnRotation, tileSetPerfabs[2]);
                }
                break;
        }
        return null;
    }

    public void DeactivateTileSet(GameObject tileSet)
    {
        objPooler.DeactivateGO(tileSet);
    }

    public void DeactivateAllPoolObjects()
    {
        objPooler.DisableAllObjects();
    }
}
