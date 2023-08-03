using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private GameObject _scope;
    public void SetCrosshairActive(bool isActive)
    {
        _crosshair.SetActive(isActive);
    }

    public void SetScopeActive(bool isActive)
    {
        _scope.SetActive(isActive);
    }
}
