using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameplayManager : MonoBehaviour
{
    private int _moveCounter;
    private int _score;
    private int _row = -1;
    private int _row2 = -1;
    private bool _disappear;
    private bool _create;
    private List<GameObject> _checks;
    private List<GameObject> _checks2;
    [SerializeField] private GameObject check;
    private GameManager _gameManager;
    public TopBar topBar;
    public GridManager gridManager;
    public float turnSpeed;
    private Level _currentLevel;
    private List<int> _counters = new (){0,0,0,0};
    public TMP_Text text;


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
        text.text = $"Width: {Screen.width} Height: {Screen.height}";
    }

    private void DisplayTopBarInfo()
    {
        topBar = FindObjectOfType<TopBar>();

        //Change texts
        _moveCounter = _currentLevel.MoveCount;
        topBar.highestScore.SetText("Highest\n Score " + _currentLevel.HighestScore);
        topBar.levelNo.SetText("Level\n " + _currentLevel.LevelNo);
        topBar.moveCount.SetText("Moves\n " + _currentLevel.MoveCount);
        topBar.score.SetText("Score\n " + _score);
    }

    private void TransformRow()
    {
        for (var i = 0; i < _currentLevel.GridWidth; i++)
        {
            var trObject = gridManager.GridCells[_row, i].transform;
            var trObject2 = _row2 != -1 ? gridManager.GridCells[_row2, i].transform : null;
            if (trObject.rotation.eulerAngles.x < 90 & trObject.rotation.eulerAngles.x > 270)
            {
                trObject.Rotate(Vector3.left, turnSpeed * Time.deltaTime);
                if (trObject2) trObject2.Rotate(Vector3.left, turnSpeed * Time.deltaTime);
            }
            else
            {
                _disappear = false;
                Destroy(trObject.gameObject);
                if (trObject2) Destroy(trObject2.gameObject);
                _create = true;
            }
        }
        _row = -1;
        _row2 = -1;
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
    }

    private void CheckPossibleMoves()
    {
        var diamonds = FindObjectsOfType<Diamond>();
        foreach (var diamond in diamonds)
        {
            switch (diamond.name.Split('(')[0])
            {
                case "Red":
                    _counters[0] += 1;
                    break;
                case "Green":
                    _counters[1] += 1;
                    break;
                case "Blue":
                    _counters[2] += 1;
                    break;
                case "Yellow":
                    _counters[3] += 1;
                    break;
            }
        }
        var terminate = true;
        foreach (var counter in _counters)
        {
            if (counter >= _currentLevel.GridWidth)
            {
                terminate = false;
                break;
            }
        }
        _counters = new List<int>{0, 0, 0, 0};
        if (terminate) Invoke(nameof(WaitAndGo),0.5f);
    }
    
    public void DecreaseMoveCount()
    {
        _moveCounter -= 1;
        topBar.moveCount.SetText("Moves " + _moveCounter);
        CheckPossibleMoves();
        if (_moveCounter == 0) Invoke(nameof(WaitAndGo),0.5f);
    }

    private void WaitAndGo()
    {
        _gameManager.ReturnMainMenu(_score);
    }

    public void CheckRows(int firstCellIndex, int secondCellIndex)
    {
        _checks = new List<GameObject>();
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
            if (_row == -1)
            {
                _row = cellIndex;
            }
            else
            {
                _row2 = cellIndex;
            }
            for (var i = 0; i < width; i++)
            {
                _checks.Add(Instantiate(check, grid[cellIndex,i].transform.parent));
            }
            _disappear = true;
        }
    }
    
}
