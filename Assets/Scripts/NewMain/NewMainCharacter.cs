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
    /// <summary>
    /// The 2D rigidbody attached to this gameObject.
    /// </summary>
    private Rigidbody2D _rigidbody;

    /// <summary>
    /// The 2D physics material attached to this gameObject.
    /// </summary>
    private PhysicsMaterial2D _naturalForces;
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
        __hop(_Direction);
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
        _rigidbody.AddForce(new Vector2(0f, 4f), ForceMode2D.Impulse);
    }

    /// <summary>
    /// Walk on the ground.
    /// </summary>
    private void __walk(MoveDirection _Direction)
    {
        __glide(_Direction);
        __glide(_Direction);
    }

    /// <summary>
    /// Glide in the air.
    /// </summary>
    private void __glide(MoveDirection _Direction)
    {
        switch (_Direction)
        {
            case MoveDirection.Left:
                _rigidbody.AddForce(new Vector2(-.5f, 0f));
                break;
            case MoveDirection.Right:
                _rigidbody.AddForce(new Vector2(.5f, 0f));
                break;
            default:
                // No side acceleration.
                break;
        }
    }

    /// <summary>
    /// Climb on a ladder( or rope).
    /// </summary>
    private void __climb(MoveDirection _Direction)
    {
        switch (_Direction)
        {
            case MoveDirection.Up:
                _rigidbody.velocity = new Vector2(0f, 1f);
                break;
            case MoveDirection.Down:
                _rigidbody.velocity = new Vector2(0f, -1f);
                break;
            default:
                _rigidbody.velocity = Vector2.zero;
                break;
        }
    }
    #endregion
    #region Unity Calls
    /*----- Unity Calls -----*/
    /*----- Work Area -----*/
    private void Awake()
    {
        // Bind.
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _naturalForces = gameObject.GetComponent<CapsuleCollider2D>().sharedMaterial;
        
        // Initialization.
        _rigidbody.freezeRotation = true;
        _rigidbody.angularDrag = 0f;
        _rigidbody.drag = .1f;
        _rigidbody.mass = 1f;
        _naturalForces.bounciness = 0f;
        _naturalForces.friction = 1f;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A)) __walk(MoveDirection.Left);
        if (Input.GetKey(KeyCode.D)) __walk(MoveDirection.Right);
        if (Input.GetKeyDown(KeyCode.Space)) __jump(MoveDirection.Null);
    }
    #endregion
}