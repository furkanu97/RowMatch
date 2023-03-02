using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public int levelNo;
    public int moveCount;
    public int highestScore;
    public bool locked;
    public List<GameObject> buttons;

    private void Start()
    {
        (locked ? buttons[0] : buttons[1]).SetActive(true);
        GetComponentsInChildren<TMP_Text>()[0].text = "Level " + levelNo + " - " + moveCount + " Moves";
        if (locked)
        {
            GetComponentsInChildren<TMP_Text>()[1].text = "Locked Level";
        }
        else
        {
            if(highestScore == 0)
            {
                GetComponentsInChildren<TMP_Text>()[1].text = "No Score";
            }
            else GetComponentsInChildren<TMP_Text>()[1].text = "Highest Score: " + highestScore;
        }
    }

    public void PlayButtonClicked()
    {
        var gameManager = FindObjectOfType<GameManager>();
        gameManager.levelIndex = levelNo;
        gameManager.Play();
    }
}