using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

internal class Level
{
    public int LevelNo;
    public int GridWidth;
    public int GridHeight;
    public int MoveCount;
    public string[,] Grid;
}

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridPiece;
    [SerializeField] private GameObject background;
    [SerializeField] private float space;
    private GridLayoutGroup _gridLayoutGroup;
    private float _cellSize;
    private Transform _createdCellTr;
    private Transform _backgroundTr;
    private string[] _infos;
    private Level _level;
    public int moveCount;

    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _level = new Level();
        ReadLevel();
        moveCount = _level.MoveCount;
        //Set Grid Size
        _cellSize = space / (_level.GridWidth > _level.GridHeight ? _level.GridWidth : _level.GridHeight);
        GetComponent<RectTransform>().sizeDelta = new Vector2(_cellSize * _level.GridWidth,_cellSize * _level.GridHeight);
        
        //Set Cell Size
        _gridLayoutGroup.cellSize = new Vector2(_cellSize, _cellSize);
        
        //Create Background and set size
        _backgroundTr = Instantiate(background).GetComponent<Transform>();
        _backgroundTr.localScale = new Vector3(_cellSize * _level.GridWidth + 40, _cellSize * _level.GridHeight + 40, 1);
        
        //Instantiate cells with desired settings
        for (var i = 0; i < _level.GridHeight; i++)
        {
            for (var j = 0; j < _level.GridWidth; j++)
            {
                _createdCellTr = Instantiate(gridPiece, transform).GetComponent<Transform>().Find("Sprite");
                _createdCellTr.localScale = new Vector3(_cellSize, _cellSize,1);
                switch (_level.Grid[i,j])
                {
                    case "r":
                        Debug.Log(i + ", " + j + " Red");
                        break;
                    case "b":
                        Debug.Log(i + ", " + j + " Blue");
                        break;
                    case "g":
                        Debug.Log(i + ", " + j + " Green");
                        break;
                    case "y":
                        Debug.Log(i + ", " + j + " Yellow");
                        break;
                    default:
                        Debug.Log("Wrong color");
                        break;
                }
            }
        }
    }
    
    private void ReadLevel()
    {
        var path = "Assets/Levels/RM_";
        _level.LevelNo = int.Parse(SceneManager.GetActiveScene().name.Split(' ')[1]);
        path += _level.LevelNo <= 15 ? 'A' + _level.LevelNo.ToString() : 'B' + (_level.LevelNo - 15).ToString();
        
        //Read the text from the path
        var reader = new StreamReader(path);
        _infos = reader.ReadToEnd().Split();
        
        //Fill the class with necessary infos
        _level.LevelNo = int.Parse(_infos[1]);
        _level.GridWidth = int.Parse(_infos[3]);
        _level.GridHeight = int.Parse(_infos[5]);
        _level.MoveCount = int.Parse(_infos[7]);
        _level.Grid = new string[_level.GridHeight, _level.GridWidth];
        for (var i = 0; i < _level.GridHeight; i++)
        {
            for (var j = 0; j < _level.GridWidth; j++)
            {
                _level.Grid[i,j] = _infos[9].Split(',')[i * _level.GridWidth + j];
            }
        }
        reader.Close();
    }
}
