using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// Input manager.
/// </summary>
public class NewInputManager : MonoBehaviour
{
    #region Public Events
    /*----- Public Events -----*/
    /// <summary>
    /// Jump event.
    /// </summary>
    public UnityEvent Jump;

    /// <summary>
    /// Left event.
    /// </summary>
    public UnityEvent Left;

    /// <summary>
    /// Right event.
    /// </summary>
    public UnityEvent Right;

    /// <summary>
    /// Up event.
    /// </summary>
    public UnityEvent Up;

    /// <summary>
    /// Down event.
    /// </summary>
    public UnityEvent Down;

    /// <summary>
    /// Interact event.
    /// </summary>
    public UnityEvent Interact;
    #endregion
    #region Link Managers
    /*----- Link Managers -----*/
    public NewCanvasManager CanvasManager;
    #endregion
    #region Unity Calls
    /*----- Unity Calls -----*/
    /*----- Work Area -----*/
    private void Update()
    {
    }
    #endregion
}