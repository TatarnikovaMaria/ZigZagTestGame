using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoresText;
    public Text infoText;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            this.enabled = false;
    }

    public void SetScores(int scores)
    {
        scoresText.text = scores.ToString();
    }
    public void ShowInfoText(string text)
    {
        infoText.text = text;
        infoText.enabled = true;
    }

    public void HideInfoText()
    {
        infoText.enabled = false;
    }
}
