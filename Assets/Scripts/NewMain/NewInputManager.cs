using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// Input manager.
/// </summary>
public class NewInputManager : MonoBehaviour
{
    #region Link Managers
    /*----- Link Managers -----*/
    public NewCanvasManager CanvasManager;
    #endregion
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

    /// <summary>
    /// No vertical movement event.
    /// </summary>
    public UnityEvent NoVertical;

    /// <summary>
    /// No Horizontal movement event.
    /// </summary>
    public UnityEvent NoHorizontal;
    #endregion
    #region Button Locks
    /*----- Button Locks -----*/
    /// <summary>
    /// Whether jump button has been down last frame.
    /// </summary>
    private bool _jumpLock = false;

    /// <summary>
    /// Whether interact button has been down last frame.
    /// </summary>
    private bool _interactLock = false;
    #endregion
    #region Unity Calls
    /*----- Unity Calls -----*/
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || CanvasManager.ButtonClicking["L"])
        {
            Left.Invoke();
        }
        else
        {
            if (Input.GetKey(KeyCode.D) || CanvasManager.ButtonClicking["R"])
            {
                Right.Invoke();
            }
            else
            {
                NoHorizontal.Invoke();
            }
        }
        if(Input.GetKey(KeyCode.W) || CanvasManager.ButtonClicking["U"])
        {
            Up.Invoke();
        }
        else
        {
            if (Input.GetKey(KeyCode.S) || CanvasManager.ButtonClicking["D"])
            {
                Down.Invoke();
            }
            else
            {
                NoHorizontal.Invoke();
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) || CanvasManager.ButtonClicking["J"] && !_jumpLock)
        {
            Jump.Invoke();
            _jumpLock = true;
        }
        if(!CanvasManager.ButtonClicking["J"])
        {
            _jumpLock = false;
        }
        if(Input.GetKeyDown(KeyCode.E) || CanvasManager.ButtonClicking["I"] && !_interactLock)
        {
            Interact.Invoke();
            _interactLock = true;
        }
        if (!CanvasManager.ButtonClicking["I"])
        {
            _interactLock = false;
        }
    }
    #endregion
}