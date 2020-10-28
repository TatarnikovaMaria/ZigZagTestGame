using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum gameDifficulty { Easy, Medium, Hard };
public enum gameStatus { Preparing, Game, GameOver}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public gameDifficulty usedDifficulty = gameDifficulty.Easy;
    public Ball ballController;

    [HideInInspector]
    public gameStatus gameStatus = gameStatus.Preparing;

    private SmoothFollow cameraFollowComp;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            this.enabled = false;
    }

    private void Start()
    {
        cameraFollowComp = Camera.main.GetComponent<SmoothFollow>();
        PrepareGame();
    }

    private void Update()
    {
        if (GameManager.instance.gameStatus == gameStatus.Game)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (gameStatus == gameStatus.GameOver)
                PrepareGame();
            else
                StartGame();
        }
    }
    public void PrepareGame()
    {
        gameStatus = gameStatus.Preparing;
        TilePooler.instance.Init();
        TilePooler.instance.DeactivateAllPoolObjects();
        CrystalPooler.instance.Init();
        LvlManager.instance.Init();
        cameraFollowComp.enabled = true;
        ballController.SetToStart();
    }
    public void StartGame()
    {
        gameStatus = gameStatus.Game;
    }

    public void GameOver()
    {
        gameStatus = gameStatus.GameOver;
        cameraFollowComp.enabled = false;
        Debug.Log("Game over!");
    }

    public void CollectCrystal()
    {
        Debug.Log("Collect crystal");
    }
}
