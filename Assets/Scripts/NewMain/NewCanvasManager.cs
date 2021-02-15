using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Canvas(UI) manager.
/// </summary>
public class NewCanvasManager : MonoBehaviour
{
    /// <summary>
    /// Events called by buttons.
    /// </summary>
    private Dictionary<string, bool> _buttonClicking = new Dictionary<string, bool>();

    /// <summary>
    /// Events called by buttons.
    /// </summary>
    public Dictionary<string, bool> ButtonClicking { get => _buttonClicking; }

    /// <summary>
    /// Load a layout prefab.
    /// </summary>
    private void _loadLayout(string _LayoutName)
    {
        GameObject Layout = Instantiate(Resources.Load<GameObject>(_LayoutName));
        Layout.transform.parent = transform;
        Layout.transform.localPosition = Vector3.zero;
        Layout.transform.localScale = Vector3.one;
        Layout.name = _LayoutName;
        _generateButtons(Layout);
    }
    
    /// <summary>
    /// Load button states upon the root game object.
    /// </summary>
    private void _generateButtons(GameObject _Root)
    {
        _buttonClicking.Clear();
        NewButton[] Buttons = _Root.GetComponentsInChildren<NewButton>();
        foreach(NewButton Button in Buttons)
        {
            _buttonClicking.Add(Button.Name, Button.Pressing);
        }
    }

    /// <summary>
    /// Refresh button states upon the root game object.
    /// </summary>
    private void _refreshButtons(GameObject _Root)
    {
        NewButton[] Buttons = _Root.GetComponentsInChildren<NewButton>();
        foreach (NewButton Button in Buttons)
        {
            _buttonClicking[Button.Name] = Button.Pressing;
        }
    }
    #region Unity Calls
    /*----- Unity Calls -----*/
    private void Start()
    {
        _loadLayout("NewUILayout_1");
    }

    private void Update()
    {
        _refreshButtons(GameObject.Find("NewUILayout_1"));
    }
    #endregion
}