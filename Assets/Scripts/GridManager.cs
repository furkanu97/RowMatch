using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridPiece;
    [SerializeField] private GameObject background;
    [SerializeField] private float space;
    [SerializeField] private List<GameObject> diamonds;
    private GameManager _gameManager;
    private float _cellSize;
    private Transform _createdCellTr;
    private Transform _backgroundTr;
    private string[] _infos;
    public Level CurrentLevel;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        CurrentLevel = _gameManager.Levels[_gameManager.levelIndex -1];
        
        //Set Grid Size
        _cellSize = space / (CurrentLevel.GridWidth > CurrentLevel.GridHeight ? CurrentLevel.GridWidth : CurrentLevel.GridHeight);
        GetComponent<RectTransform>().sizeDelta = new Vector2(_cellSize * CurrentLevel.GridWidth,_cellSize * CurrentLevel.GridHeight);
        
        //Set Cell Size
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(_cellSize, _cellSize);
        
        //Create Background and set size
        _backgroundTr = Instantiate(background).GetComponent<Transform>();
        _backgroundTr.localScale = new Vector3(_cellSize * CurrentLevel.GridWidth + 40, _cellSize * CurrentLevel.GridHeight + 40, 1);
        
        //Instantiate cells with desired settings
        for (var i = 0; i < CurrentLevel.GridHeight; i++)
        {
            for (var j = 0; j < CurrentLevel.GridWidth; j++)
            {
                _createdCellTr = Instantiate(gridPiece, transform).transform;
                _createdCellTr.GetChild(0).localScale = new Vector3(_cellSize, _cellSize,1);
                switch (CurrentLevel.Grid[i,j])
                {
                    case "r":
                        Instantiate(diamonds[0], _createdCellTr);
                        break;
                    case "g":
                        Instantiate(diamonds[1], _createdCellTr);
                        break;
                    case "b":
                        Instantiate(diamonds[2], _createdCellTr);
                        break;
                    case "y":
                        Instantiate(diamonds[3], _createdCellTr);
                        break;
                    default:
                        Debug.Log("Wrong color");
                        break;
                }
            }
        }
    }
}
