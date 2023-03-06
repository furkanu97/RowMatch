using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Level> Levels;
    public int from;
    public int to;
    public int levelIndex;
    public int currentLevel;
    private Level _level;
    private TextAsset _info;
    [SerializeField] private LevelsPopUp popUp;
    [SerializeField] private GameObject errorPopUp;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Levels = new List<Level>();
        ReadLevels();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ReadPersistence();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        popUp.ScaleUp();
    }

    private void SetHighestScore(int newHighestScore)
    {
        //Celebration
        
        Levels[currentLevel].HighestScore = newHighestScore;
        if (levelIndex < 25) levelIndex += 1;
        WritePersistence();
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
                  MoveCount = 0,
                  HighestScore = 0
              };
              if (levelNo < 11)
              {
                  _info = new TextAsset();
                  _info = (TextAsset) Resources.Load($"Levels/RM_A{levelNo}");
                  ReadLevelByInfo(_info);
              }
              else
              {
                  var path = $"{Application.persistentDataPath}/RM_";
                  path += _level.LevelNo <= 15 ? 'A' + levelNo.ToString() : 'B' + (levelNo - 15).ToString();
                  if (File.Exists(path))
                  {
                      ReadLevel(path);
                  }
                  else Levels.Add(_level);
              }
          }
    }

    private void ReadLevelByInfo(TextAsset info)
    {
        var infos = info.text.Split();
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
        //Add level to Levels list
        Levels.Add(_level);
    }

    private void ReadLevel(string path)
    {
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

    private void WritePersistence()
    {
        var all = "";
        for (int i = 0; i < Levels.Count; i++)
        {
            all += $"{Levels[i].LevelNo}:{Levels[i].HighestScore}";
            if (i != Levels.Count - 1) all += "\n";
        }
        //Create Persistence file if it does not exist
        var path = $"{Application.persistentDataPath}/Persistence";
        File.WriteAllText(path,all);
    }
    
    private void ReadPersistence()
    {
        var path = $"{Application.persistentDataPath}/Persistence";
        if (File.Exists(path))
        {
            var reader = new StreamReader(path);
            var rows = reader.ReadToEnd().Split('\n');
            //Fill the level class instances high score information
            for ( var i = 0; i < rows.Length-1 ; i++)
            {
                var words = rows[i].Split(':');
                Levels[i].HighestScore = int.Parse(words[1]);
            }
            reader.Close();
        }

        foreach (var level in Levels)
        {
            if (level.HighestScore == 0)
            {
                levelIndex = level.LevelNo - 1;
                break;
            }
        }
    }

    public void Play(int levelNo)
    {
        currentLevel = levelNo - 1;
        SceneManager.LoadSceneAsync("LevelScene");
    }
    
    public void ReturnMainMenu(int score)
    {
        if (score > Levels[currentLevel].HighestScore)
        {
            //Update Highest Score
            SetHighestScore(score);
        }
        SceneManager.LoadSceneAsync("MainScene");
    }

    //Check Internet connection and download
    public void CheckAndDownload(int levelNo)
    {
        StartCoroutine(DownloadAndWriteFile(levelNo));
    }
    
    
    //Download level file
    private IEnumerator DownloadAndWriteFile(int levelNo)
    {
        //Check Internet Connection
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            errorPopUp.SetActive(true);
            errorPopUp.transform.GetChild(0).GetComponent<TMP_Text>().text = "ERROR!\nNO INTERNET CONNECTION.";
        }
        //Download Level and Change Button
        else
        {
            var fileName = levelNo <= 15 ? "RM_A" + levelNo : "RM_B" + (levelNo - 15);
            var url = "https://row-match.s3.amazonaws.com/levels/" + fileName;
            using UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var savePath = $"{Application.persistentDataPath}/{fileName}";
                File.WriteAllText(savePath, www.downloadHandler.text);
                Levels = new List<Level>();
                ReadLevels();
                FindAndCorrectBox(levelNo);
            }
        }
        
    }

    private void FindAndCorrectBox(int levelNo)
    {
        var boxes = FindObjectsOfType<LevelInfo>();
        foreach (var box in boxes)
        {
            if (box.name == "Level " + levelNo)
            {
                if (levelNo - levelIndex == 1) box.locked = false;
                else box.buttons[2].SetActive(false);
                box.moveCount = Levels[levelNo-1].MoveCount;
                box.FillLevelInfo();
            }
        }
    }
}
