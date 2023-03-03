using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GridManager gridManager;
    private GameplayManager _gameplayManager;
    private bool _dragging;
    private Vector3 _mousePos;
    private Vector3 _mouseStartPos;
    private Vector3 _targetPos;
    private float _halfCellSize;
    private bool _returnHome;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        _gameplayManager = FindObjectOfType<GameplayManager>();
        _halfCellSize = gridManager.cellSize / 2;
    }

    private void Update()
    {
        if (_returnHome) transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, 1.0f);
    }

    private void OnMouseDown()
    {
        _mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _mouseStartPos = _mousePos;
        _dragging = true;
    }

    private void OnMouseDrag()
    {
        if (_dragging)
        {
            _mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (_mousePos.x - _mouseStartPos.x > _halfCellSize & _mousePos.y - _mouseStartPos.y < _halfCellSize)
            {
                Swap(Vector3.right); //SwapRight
            }
            else if (_mouseStartPos.x - _mousePos.x > _halfCellSize & _mouseStartPos.y - _mousePos.y < _halfCellSize)
            {
                Swap(Vector3.left); //SwapLeft();
            }
            else if (_mousePos.x - _mouseStartPos.x < _halfCellSize & _mousePos.y - _mouseStartPos.y > _halfCellSize)
            {
                Swap(Vector3.up); //SwapUp();
            }
            else if (_mouseStartPos.x - _mousePos.x < _halfCellSize & _mouseStartPos.y - _mousePos.y > _halfCellSize)
            {
                Swap(Vector3.down); //SwapDown();
            }
        }
    }

    private void Swap(Vector3 direction)
    {
        _dragging = false;
        var raycastHit2D = Physics2D.Raycast(transform.parent.transform.position + direction * _halfCellSize, direction * _halfCellSize);
        if (raycastHit2D)
        {
            var targetObject = raycastHit2D.collider.gameObject;
            if (targetObject.name == "Check(Clone)") return;
            var targetParent = targetObject.transform.parent;
            var parent = transform.parent;
            
            //Change the parents
            targetObject.transform.SetParent(parent);
            transform.SetParent(targetParent);
            
            //Arrange grid for future aspects
            var grid = gridManager.GridCells;
            var parentName = parent.gameObject.name;
            var targetParentName = targetParent.gameObject.name;
            grid[parentName[0] - '0', parentName[4] - '0'] = targetObject;
            grid[targetParentName[0] - '0', targetParentName[4] - '0'] = gameObject;
            
            //Make objects to go correct places
            targetObject.GetComponent<Diamond>()._returnHome = true;
            _returnHome = true;
            
            //Decrease move count and check for row completion
            _gameplayManager.CheckRows(parentName[0] - '0', targetParentName[0] - '0');
            _gameplayManager.DecreaseMoveCount();
        }
    }
}
