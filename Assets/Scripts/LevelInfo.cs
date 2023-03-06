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
    [SerializeField] private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        FillLevelInfo();
    }

    public void FillLevelInfo()
    {
        GetComponentsInChildren<TMP_Text>()[0].text = "Level " + levelNo + " - " + moveCount + " Moves";
        if (locked)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(false);
            if (moveCount == 0) buttons[2].SetActive(true);
            GetComponentsInChildren<TMP_Text>()[1].text = "Locked Level";
        }
        else
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(true);
            buttons[2].SetActive(false);
            if(highestScore == 0)
            {
                GetComponentsInChildren<TMP_Text>()[1].text = "No Score";
            }
            else GetComponentsInChildren<TMP_Text>()[1].text = "Highest Score: " + highestScore;
        }
    }

    public void PlayButtonClicked()
    {
        gameManager.Play(levelNo);
    }

    public void DownloadButtonClicked()
    {
        gameManager.CheckAndDownload(levelNo);
    }
}