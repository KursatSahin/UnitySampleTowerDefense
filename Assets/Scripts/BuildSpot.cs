using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.PlayerLoop;
using Utils;

public class BuildSpot : MonoBehaviour, IEventManagerHandling
{
    private bool _isBuildMenuOpened;
    private bool _isMaintenanceMenuOpened;
    private bool _isUnitBuilded;
    
    [SerializeField] private GameObject _buildMenu;
    [SerializeField] private GameObject _maintenanceMenu;
    [SerializeField] private GameObject _unit;

    private void OnEnable()
    {
        Init();
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void Init()
    {
        _isBuildMenuOpened = false;
        _isMaintenanceMenuOpened = false;
        _isUnitBuilded = false;
    }
    
    public void OnMouseDown()
    {
        Debug.Log($"{this.name}.{nameof(OnMouseDown)}() is called");
        if (!_isUnitBuilded && !_isBuildMenuOpened)
        {
            _isBuildMenuOpened = true;
            EventManager.GetInstance().Notify(Events.OpenTowerBuildCatalog, this);
        }
        else if (_isUnitBuilded && !_isMaintenanceMenuOpened)
        {
            _isMaintenanceMenuOpened = true;
            EventManager.GetInstance().Notify(Events.OpenTowerMaintenanceCatalog, this);
        }
    }

    private void OnCloseTowerCatalog(object obj)
    {
        _isBuildMenuOpened = false;
        _isMaintenanceMenuOpened = false;
    }

    public void SubscribeEvents()
    {
        EventManager.GetInstance().Subscribe(Events.ClickOutsideOfInteractiveArea, OnCloseTowerCatalog);
    }

    public void UnsubscribeEvents()
    {
        EventManager.GetInstance().Unsubscribe(Events.ClickOutsideOfInteractiveArea, OnCloseTowerCatalog);
    }
}
