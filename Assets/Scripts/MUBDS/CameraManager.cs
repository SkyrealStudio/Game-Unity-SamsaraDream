using UnityEngine;
using System;

/// <summary>
/// 2021/2/6 by wtc:<br />
/// Camera manager.<br />
/// **Basics Already done!**
/// </summary>
public class CameraManager : MonoBehaviour
{
    #region Camera State
    /*----- Camera State -----*/

    /// <summary>
    /// States of camera.
    /// </summary>
    public enum CameraState { Following, Focusing, Locked };

    /// <summary>
    /// The state of camera.
    /// </summary>
    private CameraState _state;

    /// <summary>
    /// The state of camera.
    /// </summary>
    public CameraState State
    {
        get
        {
            return _state;
        }
    }
    #endregion

    #region Camera Zoom
    /*----- Camera Zoom -----*/

    /// <summary>
    /// Camera zoom size.
    /// </summary>
    private float _size;

    /// <summary>
    /// Camera zoom size.
    /// </summary>
    public float Size
    {
        get
        {
            return _size;
        }
    }

    /// <summary>
    /// Set the camera zoom to in a frame.
    /// </summary>
    public void ZoomTo(float _Zoom)
    {
        _size = _Zoom;
        gameObject.GetComponent<Camera>().orthographicSize = _Zoom;
    }

    /// <summary>
    /// Zoom the camera smoothly.
    /// </summary>
    public void Zoom(float _Zoom)
    {
        _size = _Zoom;
    }
    #endregion

    #region Camera Speed
    /*----- Camera Speed -----*/

    /// <summary>
    /// Camera speed scale.
    /// </summary>
    private float _speedScale;

    /// <summary>
    /// Camera speed scale.
    /// </summary>
    public float SpeedScale
    {
        get
        {
            return _speedScale;
        }
    }

    /// <summary>
    /// Adjust the speed scale of the camera.<br />
    /// Default: 1.
    /// </summary>
    public void AdjustSpeed(float _Scale = 1f)
    {
        _speedScale = _Scale;
    }

    /// <summary>
    /// Camera min speed so that it won't be moving though there's little offset.
    /// </summary>
    private float _stallingSpeed;

    /// <summary>
    /// Camera min speed so that it won't be moving though there's little offset.
    /// </summary>
    public float StallingSpeed
    {
        get
        {
            return _stallingSpeed;
        }
    }

    /// <summary>
    /// Adjust the min speed of camera movement and zooming so that it won't stuck.<br />
    /// Default: 1.
    /// </summary>
    public void AdjustStallingSpeed(float _Speed = 1f)
    {
        _stallingSpeed = _Speed;
    }

    /// <summary>
    /// The speed ratio of zooming.
    /// </summary>
    private float _zoomingSpeedScale;

    /// <summary>
    /// The speed ratio of zooming.
    /// </summary>
    public float ZoomingSpeedScale
    {
        get
        {
            return _zoomingSpeedScale;
        }
    }

    /// <summary>
    /// Adjust the zooming speed scale of the camera.<br />
    /// Default: 1.
    /// </summary>
    public void AdjustZoomingSpeed(float _Speed = 1f)
    {
        _zoomingSpeedScale = _Speed;
    }

    /// <summary>
    /// Slide camera to a position.
    /// </summary>
    private void _move(Vector2 _Position)
    {
        _Position -= (Vector2)transform.position;
        transform.Translate((_Position + _Position.normalized * StallingSpeed) * Time.deltaTime * SpeedScale);
    }

    /// <summary>
    /// Zoom camera to a size.
    /// </summary>
    public void _zoom()
    {
        Camera cam = gameObject.GetComponent<Camera>();
        cam.orthographicSize += 
            (Math.Sign(Size - cam.orthographicSize) * StallingSpeed + Size - cam.orthographicSize) *
            Time.deltaTime * ZoomingSpeedScale * 7.5f;
    }
    #endregion

    #region Camera Follow
    /*----- Camera Follow -----*/

    /// <summary>
    /// The game object the camera would follow.
    /// </summary>
    private GameObject _followingGameObject;

    /// <summary>
    /// The game object the camera would follow.
    /// </summary>
    public GameObject FollowingGameObject
    {
        get
        {
            return _followingGameObject;
        }
    }

    /// <summary>
    /// Set the game object the camera would follow.
    /// </summary>
    public void SetFollowingGameObject(GameObject _GameObject)
    {
        _followingGameObject = _GameObject;
    }

    /// <summary>
    /// Set the camera state to following.
    /// </summary>
    public void StartFollowing()
    {
        if ((bool)FollowingGameObject)
            _state = CameraState.Following;
    }
    
    /// <summary>
    /// Just a combine of <see cref="SetFollowingGameObject(GameObject)"/> and <see cref="StartFollowing"/>.
    /// </summary>
    public void StartFollowing(GameObject _GameObject)
    {
        SetFollowingGameObject(_GameObject);
        StartFollowing();
    }

    /// <summary>
    /// Logic when the camera is following.
    /// </summary>
    private void _follow()
    {
        if (!(bool)FollowingGameObject)
        {
            Lock();
            return;
        }
        _move(FollowingGameObject.transform.position);
    }
    #endregion

    #region Camera Focus
    /*----- Camera Focus -----*/

    /// <summary>
    /// The position the camera would focus.
    /// </summary>
    private Vector2 _focusPosition;

    /// <summary>
    /// The position the camera would focus.
    /// </summary>
    public Vector2 FocusPosition
    {
        get
        {
            return _focusPosition;
        }
    }

    /// <summary>
    /// Set the position the camera should focus.
    /// </summary>
    public void SetFocusPosition(Vector2 _Position)
    {
        _focusPosition = _Position;
    }

    /// <summary>
    /// Set the camera state to focusing.
    /// </summary>
    public void StartFocusing()
    {
        _state = CameraState.Focusing;
    }

    /// <summary>
    /// Just a combine of <see cref="SetFocusPosition(Vector2)"/> and <see cref="StartFocusing"/>.
    /// </summary>
    public void StartFocusing(Vector2 _Position)
    {
        SetFocusPosition(_Position);
        StartFocusing();
    }

    /// <summary>
    /// Logic when the camera is focusing.
    /// </summary>
    private void _focus()
    {
        _move(FocusPosition);
    }
    #endregion

    #region Camera Lock
    /*----- Camera Lock -----*/

    /// <summary>
    /// Set the camera state to locked.
    /// </summary>
    public void Lock()
    {
        _state = CameraState.Locked;
    }

    /// <summary>
    /// <see cref="Lock"/> and transform it to a position.
    /// </summary>
    public void LockTo(Vector2 _Position)
    {
        Lock();
        transform.position = (Vector3)_Position - new Vector3(0f, 0f, 1f);
    }
    #endregion

    #region Unity Calls
    /*----- Unity Calls -----*/

    private void Start()
    {
        // Initialize.
        _size = 5f;
        _speedScale = 1f;
        _state = CameraState.Locked;
        _zoomingSpeedScale = 1f;
        transform.position = new Vector3(0f, 0f, -1f);
    }

    private void Update()
    {
        _zoom();
        switch(State)
        {
            case CameraState.Following:
                _follow();
                break;
            case CameraState.Focusing:
                _focus();
                break;
            case CameraState.Locked:
                //DO NOTHING
                break;
        }
    }
    #endregion
}