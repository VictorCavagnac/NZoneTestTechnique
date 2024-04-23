using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToLoader : MonoBehaviour
{
    [Header("Menu to load")]
    [SerializeField]
    private LoadEventChannelSO _loadMenu = default;

    [SerializeField] 
	private GameSceneSO _menuToLoad = default;

    public void GoToMenu()
    {
        _loadMenu.RaiseEvent(_menuToLoad, true, true);
    }
}
