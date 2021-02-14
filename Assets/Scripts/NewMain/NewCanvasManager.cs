using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Canvas(UI) manager.
/// </summary>
public class NewCanvasManager : MonoBehaviour
{
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
    }
    #region Unity Calls
    /*----- Unity Calls -----*/
    private void Start()
    {
        _loadLayout("NewUILayout_1");
    }
    #endregion
}