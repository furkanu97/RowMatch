using UnityEngine;

public class Panel : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject levelBlock;
    private GameObject _levelBlock;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        CheckUnlockSituation();
        FillPopUp();
        transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -625, 0);
    }

    private void CheckUnlockSituation()
    {
        foreach (var level in gameManager.Levels)
        {
            if (level.MoveCount != 0)
            {
                if (level.HighestScore != 0)
                {
                    level.Locked = false;
                }
                else
                {
                    level.Locked = false;
                    break;
                }
            }
        }
    }

    public void FillPopUp()
    {
        foreach (var level in gameManager.Levels)
        {
            _levelBlock = Instantiate(levelBlock, transform);
            _levelBlock.gameObject.name = "Level " + level.LevelNo;
            var info = _levelBlock.GetComponent<LevelInfo>();
            info.levelNo = level.LevelNo;
            info.moveCount = level.MoveCount;
            info.highestScore = level.HighestScore;
            info.locked = level.Locked;
            info.FillLevelInfo();
        }
    }
}
