using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBar : MonoBehaviour
{
    public GridManager gridManager; 
    private void Start()
    {
        var gridSize = gridManager.GetComponent<RectTransform>().sizeDelta;
        
        //Arrange bar width and position
        transform.localPosition += new Vector3(0,gridSize.y / 3 + 60,0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(gridSize.x * 0.8f, 40);
        
        //Change texts
        GameObject.Find("HighestScore").GetComponent<TMP_Text>().text = "Highest Score: " + gridManager.CurrentLevel.HighestScore;
        GameObject.Find("LevelNo").GetComponent<TMP_Text>().text = "Level " + gridManager.CurrentLevel.LevelNo;
        GameObject.Find("MoveCount").GetComponent<TMP_Text>().text = "Moves " + gridManager.CurrentLevel.MoveCount;
    }
}
