using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridPiece;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject topBar;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private float space;
    [SerializeField] private List<GameObject> diamonds;
    private GameManager _gameManager;
    private Transform _createdCellTr;
    private Transform _backgroundTr;
    private string[] _infos;
    public float cellSize;
    public GameObject[,] GridCells;
    public Level CurrentLevel;

    private void OnEnable()
    {
        CreateGrid();
        CreateBackground();
        CreateTopBar();
    }

    private void CreateGrid()
    {
        _gameManager = FindObjectOfType<GameManager>();
        CurrentLevel = _gameManager.Levels[_gameManager.levelIndex];
        Debug.Log("Move Count: " + CurrentLevel.MoveCount);

        //Set Grid Size
        cellSize = space / (CurrentLevel.GridWidth > CurrentLevel.GridHeight ? CurrentLevel.GridWidth : CurrentLevel.GridHeight);
        GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize * CurrentLevel.GridWidth,cellSize * CurrentLevel.GridHeight);
        GridCells = new GameObject[CurrentLevel.GridHeight, CurrentLevel.GridWidth];

        //Set Cell Size
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);


        //Instantiate cells with desired settings
        for (var i = 0; i < CurrentLevel.GridHeight; i++)
        {
            for (var j = 0; j < CurrentLevel.GridWidth; j++)
            {
                _createdCellTr = Instantiate(gridPiece, transform).transform;
                _createdCellTr.gameObject.name = i + " x " + j;
                _createdCellTr.GetChild(0).localScale = new Vector3(cellSize, cellSize,1);
                switch (CurrentLevel.Grid[i,j])
                {
                    case "r":
                        GridCells[i,j] = Instantiate(diamonds[0], _createdCellTr);
                        break;
                    case "g":
                        GridCells[i,j] = Instantiate(diamonds[1], _createdCellTr);
                        break;
                    case "b":
                        GridCells[i,j] = Instantiate(diamonds[2], _createdCellTr);
                        break;
                    case "y":
                        GridCells[i,j] = Instantiate(diamonds[3], _createdCellTr);
                        break;
                    default:
                        Debug.Log("Wrong color");
                        break;
                }
            }
        }
    }
    
    private void CreateBackground()
    {
        //Create Background and set size
        _backgroundTr = Instantiate(background).GetComponent<Transform>();
        _backgroundTr.localScale = new Vector3(cellSize * CurrentLevel.GridWidth + 40, cellSize * CurrentLevel.GridHeight + 40, 1);
    }

    private void CreateTopBar()
    {
        //Create TopBar as a child of Canvas
        Instantiate(topBar, canvasTransform);
    }
}
