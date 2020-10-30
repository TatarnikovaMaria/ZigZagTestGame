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
        GameObject tileSet = null;
        switch (GameManager.instance.usedDifficulty)
        {
            case gameDifficulty.Easy:
                if (tileSetPerfabs.Count > 0)
                {
                    tileSet = objPooler.Spawn(spawnPoint, spawnRotation, tileSetPerfabs[0]);
                }
                break;

            case gameDifficulty.Medium:
                if (tileSetPerfabs.Count > 1)
                {
                    tileSet = objPooler.Spawn(spawnPoint, spawnRotation, tileSetPerfabs[1]);
                }
                break;

            case gameDifficulty.Hard:
                if (tileSetPerfabs.Count > 2)
                {
                    tileSet = objPooler.Spawn(spawnPoint, spawnRotation, tileSetPerfabs[2]);
                }
                break;
        }

        if (tileSet != null)
        {
            List<Animator> animators = new List<Animator>();
            animators.AddRange(tileSet.GetComponentsInChildren<Animator>());

            for (int i = 0; i < animators.Count; i++)
            {
                animators[i].SetBool("isVisible", true);
            }
        }

        return tileSet;
    }

    string hideTilesRoutineName = "HideTiles";
    public void HideTileSet(Transform tileSet)
    {
        StartCoroutine(hideTilesRoutineName, tileSet);
    }

    IEnumerator HideTiles(Transform tileSet)
    {
        List<Animator> animators = new List<Animator>();
        animators.AddRange(tileSet.GetComponentsInChildren<Animator>());

        for (int i = 0; i < animators.Count; i++)
        {
            animators[i].SetBool("isVisible", false);
        }
        yield return new WaitForSeconds(2);
        DeactivateTileSet(tileSet.gameObject);
    }

    public void DeactivateTileSet(GameObject tileSet)
    {
        objPooler.DeactivateGO(tileSet);
    }

    public void DeactivateAllPoolObjects()
    {
        StopCoroutine(hideTilesRoutineName);
        objPooler.DisableAllObjects();
    }
}
