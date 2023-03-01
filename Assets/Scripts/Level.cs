using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public int levelNo;
    public int gridWidth;
    public int gridHeight;
    public int moveCount;
    public string grid;
    public int highestScore;
    public List<GameObject> buttons;
    [SerializeField] private bool locked;

    private void Awake()
    {
        Instantiate(locked ? buttons [0] : buttons[1], transform);
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
}