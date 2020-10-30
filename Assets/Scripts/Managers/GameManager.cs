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
    private int scores =  0;

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
        UIManager.instance.ShowInfoText("Tap to play!");
        gameStatus = gameStatus.Preparing;
        scores = 0;
        UIManager.instance.SetScores(scores);
        TilePooler.instance.Init();
        TilePooler.instance.DeactivateAllPoolObjects();
        CrystalPooler.instance.Init();
        LvlManager.instance.Init();
        cameraFollowComp.enabled = true;
        ballController.SetToStart();
    }
    public void StartGame()
    {
        UIManager.instance.HideInfoText();
        gameStatus = gameStatus.Game;
    }

    public void GameOver()
    {
        UIManager.instance.ShowInfoText("Game over!\n Tap to start new game.");
        gameStatus = gameStatus.GameOver;
        cameraFollowComp.enabled = false;
    }

    public void CollectCrystal()
    {
        scores++;
        UIManager.instance.SetScores(scores);
    }
}
