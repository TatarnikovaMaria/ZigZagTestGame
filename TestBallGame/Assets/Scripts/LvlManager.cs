using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrystalGenerateMode { Random, InOrder };

public class LvlManager : MonoBehaviour
{
    public static LvlManager instance;

    public int maxOneDirectionSetsCount = 3;
    public int oneTimeSetsCount = 10;
    public CrystalGenerateMode crystalMode = CrystalGenerateMode.Random;

    private Vector3 forwardAfterForwardPositionDelta;
    private Vector3 rightAfterForwardPositionDelta;
    private Vector3 forwardAfterRightPositionDelta;
    private Vector3 rightAfterRightPositionDelta;

    private Quaternion forwardRotation = Quaternion.Euler(0, -90, 0);
    private Quaternion rightRotation = Quaternion.identity;

    private int setLenght = 1;
    private int setWidth = 3;
    private int crystalTileGroupLengh = 5;   //means one crystal at 5 tile sets

    private Vector3 lastSetPosition;
    private bool lastDirectionWasForward = true;
    private int oneDirectionSetsLine = 1;
    private Transform lastTileSetWithCrystalGroupEnd;  //end of tile group where lasr crystal was
    private int lastCrystalDelta = 0;                   //inde[ in tile group where last crystal was (for in order generation mode)

    private List<Transform> lvlTileSet = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            this.enabled = false;
    }

    public void Init()
    {
        TilePooler.instance.Init();
        lvlTileSet.Clear();

        switch (GameManager.instance.usedDifficulty)
        {
            case gameDifficulty.Medium:
                {
                    setWidth = 2;
                    break;
                }

            case gameDifficulty.Hard:
                {
                    setWidth = 1;
                    break;
                }
        }

        forwardAfterForwardPositionDelta = new Vector3(0, 0, setLenght);
        rightAfterForwardPositionDelta = new Vector3(setWidth/2f + 0.5f, 0, setLenght - 0.5f - setWidth/2f);
        forwardAfterRightPositionDelta = new Vector3(setLenght - 0.5f - setWidth/2f, 0, setWidth/2f + 0.5f);
        rightAfterRightPositionDelta = new Vector3(setLenght, 0, 0);

        lastDirectionWasForward = true;
        lastSetPosition = new Vector3(0, 0, 2);
        lvlTileSet.Add(TilePooler.instance.SpawnTileSet(lastSetPosition, forwardRotation).transform);

        GenerateLvl();
    }

    private void GenerateLvl()
    {
        bool nextDirectionIsForward;

        while (lvlTileSet.Count < oneTimeSetsCount)
        {
            if(oneDirectionSetsLine < setWidth)
                nextDirectionIsForward = lastDirectionWasForward;
            else if (oneDirectionSetsLine == maxOneDirectionSetsCount)
                nextDirectionIsForward = !lastDirectionWasForward;
            else
                nextDirectionIsForward = Random.Range(0, 2) == 0 ? true : false;

            GenerateOneArea(nextDirectionIsForward);
        }

        //GenerateCrystals();
    }

    private void GenerateCrystals()
    {
        int startInd = 0;

        if (lastTileSetWithCrystalGroupEnd == null)
            startInd = 0;
        else
        {
            startInd = lvlTileSet.FindIndex(t => t == lastTileSetWithCrystalGroupEnd);
            if (startInd < 0)
                startInd = 0;
        }

        int crystalSetInd = 0;

        while(startInd + crystalTileGroupLengh < lvlTileSet.Count)
        {
            if(crystalMode == CrystalGenerateMode.Random)
            {
                crystalSetInd = Random.Range(startInd, startInd + crystalTileGroupLengh);
            }
            else
            {
                lastCrystalDelta++;

                if (lastCrystalDelta >= crystalTileGroupLengh)
                    lastCrystalDelta = 0;

                crystalSetInd = startInd + lastCrystalDelta;
            }

            CrystalPooler.instance.SpawnCrystal(lvlTileSet[crystalSetInd].position);
            
            startInd += crystalTileGroupLengh;
            lastTileSetWithCrystalGroupEnd = lvlTileSet[startInd - 1];
        }
    }

    private void GenerateOneArea(bool isForward = true)
    {
        if (isForward)
        {
            lastSetPosition += lastDirectionWasForward ? forwardAfterForwardPositionDelta : forwardAfterRightPositionDelta;
            lvlTileSet.Add( TilePooler.instance.SpawnTileSet(lastSetPosition, forwardRotation).transform);
        }
        else
        {
            lastSetPosition += lastDirectionWasForward ? rightAfterForwardPositionDelta : rightAfterRightPositionDelta;
            lvlTileSet.Add( TilePooler.instance.SpawnTileSet(lastSetPosition, rightRotation).transform);
        }

        if (lastDirectionWasForward == isForward)
        {
            oneDirectionSetsLine++;
        }
        else
        {
            oneDirectionSetsLine = 0;
        }
        lastDirectionWasForward = isForward;
    }

    public void CheckPassedTileSets(Vector3 ballPosition)
    {
        int i = 0;
        while (i < 5 && i < lvlTileSet.Count)   //check first 5 tile sets, because when path with > 1, ball can miss some tile sets
        {
            if(lvlTileSet[i].position.x + setLenght / 2f + 1 < ballPosition.x
                || lvlTileSet[i].position.z + setWidth / 2f + 1 < ballPosition.z)      //tile set is passed
            {
                TilePooler.instance.DeactivateTileSet(lvlTileSet[i].gameObject);
                lvlTileSet.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
        GenerateLvl();
    }
}
