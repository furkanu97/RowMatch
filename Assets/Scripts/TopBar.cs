using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBar : MonoBehaviour
{
    [SerializeField] private GridManager gridManager; 
    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        var gridSize = gridManager.GetComponent<RectTransform>().sizeDelta;
        
        //Arrange bar width and position
        transform.localPosition += new Vector3(0,gridSize.y / 3 + 60,0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(gridSize.x * 0.8f, 40);
        
        //Change texts
        GameObject.Find("HighestScore").GetComponent<TMP_Text>().text = "Highest Score: 0";
        GameObject.Find("LevelNo").GetComponent<TMP_Text>().text = SceneManager.GetActiveScene().name;
        GameObject.Find("MoveCount").GetComponent<TMP_Text>().text = "Moves " + gridManager.moveCount;
    }
}
