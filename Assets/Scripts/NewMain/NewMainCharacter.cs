using UnityEngine;

/// <summary>
/// Script for main character.
/// </summary>
public class NewMainCharacter : MonoBehaviour
{
    #region Enums
    /*----- Enums -----*/
    /// <summary>
    /// Directions of character movement.
    /// </summary>
    private enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right,
        Null
    }
    #endregion
    #region Components
    /*----- Components -----*/
    /*----- Work Area -----*/
    private Rigidbody2D _rigidbody;
    #endregion
    #region Basic Behavior
    /*----- Basic Behavior -----*/
    /*----- Work Area -----*/
    /// <summary>
    /// Perform a jump from ground.
    /// </summary>
    private void __jump(MoveDirection _Direction)
    {
        __hop(_Direction);
        _rigidbody.AddForce(new Vector2(0f, 5f));
    }

    /// <summary>
    /// Perform a hop in the air.
    /// </summary>
    private void __hop(MoveDirection _Direction)
    {
        switch(_Direction)
        {
            case MoveDirection.Left:
                _rigidbody.AddForce(new Vector2(-1.5f, 0f), ForceMode2D.Impulse);
                break;
            case MoveDirection.Right:
                _rigidbody.AddForce(new Vector2(1.5f, 0f), ForceMode2D.Impulse);
                break;
            default:
                // No side acceleration.
                break;
        }
        _rigidbody.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
    }

    /// <summary>
    /// Walk on the ground.
    /// </summary>
    private void __walk(MoveDirection _Direction)
    {

    }

    /// <summary>
    /// Glide in the air.
    /// </summary>
    private void __glide(MoveDirection _Direction)
    {

    }

    /// <summary>
    /// Climb on a ladder( or rope).
    /// </summary>
    private void __climb(MoveDirection _Direction)
    {

    }
    #endregion
    #region Unity Calls
    /*----- Unity Calls -----*/
    /*----- Work Area -----*/
    private void Awake()
    {
        // Initialization.
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        __hop(MoveDirection.Right);
    }
    #endregion
}