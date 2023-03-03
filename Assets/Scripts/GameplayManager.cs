using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameplayManager : MonoBehaviour
{
    private int _moveCounter;
    private int _score;
    private int _row;
    private bool _disappear;
    private bool _create;
    private List<GameObject> _checks;
    [SerializeField] private GameObject check;
    private GameManager _gameManager;
    public TopBar topBar;
    public GridManager gridManager;
    public float turnSpeed;
    private Level _currentLevel;
    private List<int> _counter;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _currentLevel = gridManager.CurrentLevel;
        DisplayTopBarInfo();
    }

    private void Update()
    {
        if (_disappear) TransformRow();
        if (_create) CreateChecks();
    }

    private void DisplayTopBarInfo()
    {
        topBar = FindObjectOfType<TopBar>();

        //Change texts
        _moveCounter = _currentLevel.MoveCount;
        topBar.highestScore.SetText("Highest Score: " + _currentLevel.HighestScore);
        topBar.levelNo.SetText("Level " + _currentLevel.LevelNo);
        topBar.moveCount.SetText("Moves " + _currentLevel.MoveCount);
        topBar.score.SetText("Score: " + _score);
        _moveCounter = _currentLevel.MoveCount;
    }

    private void TransformRow()
    {
        for (var i = 0; i < _currentLevel.GridWidth; i++)
        {
            var trObject = gridManager.GridCells[_row, i].transform;
            if (trObject.rotation.eulerAngles.x < 90 & trObject.rotation.eulerAngles.x > 270)
            {
                trObject.Rotate(Vector3.left, turnSpeed * Time.deltaTime);
            }
            else
            {
                _disappear = false;
                Destroy(trObject.gameObject);
                _create = true;
            }
        }
    }

    private void CreateChecks()
    {
        foreach (var obj in _checks)
        {
            if (obj.transform.rotation.eulerAngles.x > 1 & obj.transform.rotation.eulerAngles.x < 359)
            {
                obj.transform.Rotate(Vector3.left, turnSpeed * Time.deltaTime);
            }
            else
            {
                _create = false;
            }
        }
    }

    private void AddScore(int toAdd)
    {
        _score += toAdd;
        topBar.score.SetText("Score: " + _score);
        //Invoke(nameof(CheckPossibleMoves),0.5f);
    }

    // private void CheckPossibleMoves()
    // {
    //     _counter = new List<int>(4);
    //     var diamonds = FindObjectsOfType<Diamond>();
    //     foreach (var diamond in diamonds)
    //     {
    //         switch (diamond.name)
    //         {
    //             case "Red(Clone)":
    //                 _counter[0] += 1;
    //                 break;
    //             case "Green(Clone)":
    //                 _counter[1] += 1;
    //                 break;
    //             case "Blue(Clone)":
    //                 _counter[2] += 1;
    //                 break;
    //             case "Yellow(Clone)":
    //                 _counter[3] += 1;
    //                 break;
    //         }
    //     }
    //     bool terminate = true;
    //     foreach (var counter in _counter)
    //     {
    //         if (counter > _currentLevel.GridWidth) terminate = false;
    //         Debug.Log(counter);
    //     }
    //     if (terminate) Invoke(nameof(WaitAndGo),0.5f);
    // }
    
    public void DecreaseMoveCount()
    {
        _moveCounter -= 1;
        topBar.moveCount.SetText("Moves " + _moveCounter);
        if (_moveCounter == 0) Invoke(nameof(WaitAndGo),0.5f);
    }

    private void WaitAndGo()
    {
        _gameManager.ReturnMainMenu(_score);
    }

    public void CheckRows(int firstCellIndex, int secondCellIndex)
    {
        CheckRow(firstCellIndex);
        if (firstCellIndex != secondCellIndex) CheckRow(secondCellIndex);
    }

    private void CheckRow(int cellIndex)
    {
        var rowCompleted = true;
        var grid = gridManager.GridCells;
        var width = _currentLevel.GridWidth;
        
        //Find the color
        var typeOfCell = grid[cellIndex, 0].name;

        //Check if this is a Row Match!
        for (var i = 1; i < width; i++)
        {
            if (grid[cellIndex, i].name != typeOfCell)
            {
                rowCompleted = false;
                break;
            }
        }

        if (rowCompleted)
        {
            //Calculate and add Score
            int scoreToAdd;
            switch (typeOfCell.Split('(')[0])
            {
                case "Red":
                    scoreToAdd = 100;
                    break;
                case "Green":
                    scoreToAdd = 150;
                    break;
                case "Blue":
                    scoreToAdd = 200;
                    break;
                case "Yellow":
                    scoreToAdd = 250;
                    break;
                default:
                    scoreToAdd = 0;
                    break;
            }
            AddScore(scoreToAdd * width);
            
            //Instantiate check sprites
            _checks = new List<GameObject>();
            _row = cellIndex;
            _disappear = true;
            for (var i = 0; i < width; i++)
            {
                _checks.Add(Instantiate(check, grid[cellIndex,i].transform.parent));
            }
        }
    }
    
}
