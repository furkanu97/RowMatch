using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBar : MonoBehaviour
{
    public GridManager gridManager;
    public TMP_Text levelNo;
    public TMP_Text highestScore;
    public TMP_Text moveCount;
    public TMP_Text score;
    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        var gridSize = gridManager.GetComponent<RectTransform>().sizeDelta;

        //Arrange bar width and position
        transform.localPosition += new Vector3(0,gridSize.y / 3 + 60,0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(gridSize.x * 0.8f > 600f ? gridSize.x * 0.8f : 600f, 40);
    }
}
