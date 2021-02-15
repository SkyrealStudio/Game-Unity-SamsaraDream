using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Overriding button.
/// </summary>
public class NewButton : Button, IPointerDownHandler, IPointerUpHandler
{
    #region Key & Value
    /*----- Key & Value -----*/
    /// <summary>
    /// Name of this button.
    /// </summary>
    private string _name;

    /// <summary>
    /// Name of this button.
    /// </summary>
    public string Name { get => _name; }

    /// <summary>
    /// State whether the button is pressed.
    /// </summary>
    private bool _pressing = false;

    /// <summary>
    /// State whether the button is pressed.
    /// </summary>
    public bool Pressing { get => _pressing; }
    #endregion
    #region En/Dis|abled
    /*----- En/Dis|abled -----*/

    #endregion
    #region Unity Calls
    /*----- Unity Calls -----*/
    public override void OnPointerDown(PointerEventData _PED)
    {
        base.OnPointerDown(_PED);
        _pressing = true;
    }

    public override void OnPointerUp(PointerEventData _PED)
    {
        base.OnPointerUp(_PED);
        _pressing = false;
    }

    protected override void Awake()
    {
        base.Awake();
        _name = gameObject.name.Substring(6);
    }
    #endregion
}