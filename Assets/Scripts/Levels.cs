using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.Tween;
using UnityEngine;

public class Levels : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    private bool _scaleUp;
    private bool _scaleDown;
    private Transform _transform;
    

    private void Start()
    {
        _transform = popUp.GetComponent<Transform>();
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
        _transform.localScale += new Vector3(1,1,0) * 0.02f;
        if (_transform.localScale.x >= 1 || _transform.localScale.y >= 1)
        {
            _scaleUp = false;
        }
    }

    private void CloseLevelsPopUp()
    {
        _transform.localScale -= new Vector3(1,1,0) * 0.02f;
        if (_transform.localScale.x <= 0 || _transform.localScale.y <= 0)
        {
            _scaleDown = false;
            popUp.SetActive(false);
        }
    }

    public void ScaleUp()
    {
        _scaleUp = true;
        popUp.SetActive(true);
    }

    public void ScaleDown()
    {
        _scaleDown = true;
    }
}
