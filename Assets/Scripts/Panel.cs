using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject levelBlock;
    private GameObject _levelBlock;

    private void Start()
    {
        FillPopUp();
    }
    
    private void FillPopUp()
    {
        foreach (var level in gameManager.Levels)
        {
            _levelBlock = Instantiate(levelBlock, transform);
            _levelBlock.gameObject.name = "Level " + level.LevelNo;
            var info = _levelBlock.GetComponent<LevelInfo>();
            info.levelNo = level.LevelNo;
            info.moveCount = level.MoveCount;
            info.highestScore = level.HighestScore;
            info.locked = level.Locked;
        }
    }
}
