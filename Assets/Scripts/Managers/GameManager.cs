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

    private SmoothFollow cameraFollowComp;
    private int scores =  0;

    private gameStatus gameStatus = gameStatus.Preparing;
    public gameStatus GameStatus 
    {
        get
        {
            return gameStatus;
        }

        private set
        {
            gameStatus = value;

            switch(gameStatus)
            {
                case gameStatus.Preparing:
                    PrepareGame();
                    break;

                case gameStatus.Game:
                    StartGame();
                    break;

                case gameStatus.GameOver:
                    GameOver();
                    break;
            }
        }
    }

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
        GameStatus = gameStatus.Preparing;
    }

    private void Update()
    {
        if (GameManager.instance.gameStatus == gameStatus.Game)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (gameStatus == gameStatus.GameOver)
                GameStatus = gameStatus.Preparing;
            else
                GameStatus = gameStatus.Game;
        }
    }
    private void PrepareGame()
    {
        UIManager.instance.ShowInfoText("Tap to play!");
        scores = 0;
        UIManager.instance.SetScores(scores);
        TilePooler.instance.Init();
        TilePooler.instance.DeactivateAllPoolObjects();
        CrystalPooler.instance.Init();
        LvlManager.instance.Init();
        cameraFollowComp.enabled = true;
        ballController.SetToStart();
    }
    private void StartGame()
    {
        UIManager.instance.HideInfoText();
    }

    private void GameOver()
    {
        UIManager.instance.ShowInfoText("Game over!\n Tap to start new game.");
        cameraFollowComp.enabled = false;
    }

    public void EndGame()
    {
        GameStatus = gameStatus.GameOver;
    }

    public void CollectCrystal()
    {
        scores++;
        UIManager.instance.SetScores(scores);
    }
}
