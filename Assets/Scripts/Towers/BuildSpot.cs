using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.PlayerLoop;

public class BuildSpot : MonoBehaviour, IEventManagerHandling
{
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject maintenanceMenu;
    [SerializeField] private GameObject unit;

    private bool _isBuildMenuOpened;
    private bool _isMaintenanceMenuOpened;
    private bool _isUnitBuilded;
    
    #region Unity Events

    private void OnEnable()
    {
        Init();
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
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

    #endregion

    #region BuildSpot Methods

    public void Init()
    {
        _isBuildMenuOpened = false;
        _isMaintenanceMenuOpened = false;
        _isUnitBuilded = false;
    }

    #endregion

    #region BuildSpot Event Callbacks

    private void OnCloseTowerCatalog(object obj)
    {
        _isBuildMenuOpened = false;
        _isMaintenanceMenuOpened = false;
    }

    #endregion

    #region IEventManagerHandling Methods

    public void SubscribeEvents()
    {
        EventManager.GetInstance().Subscribe(Events.ClickOutsideOfInteractiveArea, OnCloseTowerCatalog);
    }

    public void UnsubscribeEvents()
    {
        EventManager.GetInstance().Unsubscribe(Events.ClickOutsideOfInteractiveArea, OnCloseTowerCatalog);
    }

    #endregion
}
