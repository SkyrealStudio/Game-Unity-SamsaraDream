using UnityEngine;

/// <summary>
/// Game manager.
/// </summary>
public class NewGameManager : MonoBehaviour
{
    #region Link Managers
    /*----- Link Managers -----*/
    /// <summary>
    /// Global input manager.
    /// </summary>
    public NewInputManager InputManager;

    /// <summary>
    /// Global camera manager.
    /// </summary>
    public NewCameraManager CameraManager;

    /// <summary>
    /// Global canvas manager.
    /// </summary>
    public NewCanvasManager CanvasManager;

    /// <summary>
    /// Global main character.
    /// </summary>
    public NewMainCharacter MainCharacter;
    #endregion
}