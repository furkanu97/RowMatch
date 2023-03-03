using System;
using UnityEngine;

public class LevelsPopUp : MonoBehaviour
{
    private bool _scaleUp;
    private bool _scaleDown;
    [SerializeField] private float scaler = 0.02f;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (_scaleUp)
        {
            OpenLevelsPopUp();
        }
        if(_scaleDown)
        {
            CloseLevelsPopUp();
        }
    }

    private void OpenLevelsPopUp()
    {
        transform.localScale += new Vector3(1,1,0) * scaler;
        if (transform.localScale.x >= 1 || transform.localScale.y >= 1)
        {
            transform.localScale = Vector3.one;
            _scaleUp = false;
        }
    }

    private void CloseLevelsPopUp()
    {
        transform.localScale -= new Vector3(1,1,0) * scaler;
        if (transform.localScale.x <= 0 || transform.localScale.y <= 0)
        {
            transform.localScale = new Vector3(0,0,1);
            _scaleDown = false;
        }
    }

    public void ScaleUp()
    {
        _scaleUp = true;
    }

    public void ScaleDown()
    {
        _scaleDown = true;
    }
}
