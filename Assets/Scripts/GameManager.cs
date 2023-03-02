using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Level> Levels;
    public int from;
    public int to;
    public int levelIndex;
    private Level _level;

    private void Awake()
    {
        Levels = new List<Level>();
        ReadLevels();
        DontDestroyOnLoad(transform.gameObject);
    }
    
    private void ReadLevels()
    {
        //Read the text from the path
        for (var levelNo = from; levelNo <= to; levelNo++)
        {
            _level = new Level
            {
                LevelNo = levelNo,
                Locked = levelNo != 1,
                HighestScore = 0
            };
            var path = "Assets/Levels/RM_";
            path += _level.LevelNo <= 15 ? 'A' + levelNo.ToString() : 'B' + (levelNo - 15).ToString();
            var reader = new StreamReader(path);
            var infos = reader.ReadToEnd().Split();
            //Fill the level class instance with necessary infos
            _level.LevelNo = int.Parse(infos[1]);
            _level.GridWidth = int.Parse(infos[3]);
            _level.GridHeight = int.Parse(infos[5]);
            _level.MoveCount = int.Parse(infos[7]);
            _level.Grid = new string[_level.GridHeight, _level.GridWidth];
            for (var i = 0; i < _level.GridHeight; i++)
            {
                for (var j = 0; j < _level.GridWidth; j++)
                {
                    _level.Grid[i,j] = infos[9].Split(',')[i * _level.GridWidth + j];
                }
            }
            reader.Close();
            //Add level to Levels list
            Levels.Add(_level);
        }
    }
    
    public void Play()
    {
        SceneManager.LoadSceneAsync("LevelScene");
    }
}
