using UnityEngine;
using UnityEngine.Events;

#region Enums
/*----- Enums -----*/
/// <summary>
/// Directions of character movement.
/// </summary>
public enum MoveDirection
{
    Up,
    Down,
    Left,
    Right,
    Null
}

/// <summary>
/// States of character pose( & ability to act).
/// </summary>
public enum CharacterState
{
    InAir,
    InAirJumppable,
    OnGround,
    OnLadder
}
#endregion
/// <summary>
/// Script for main character.
/// </summary>
public class NewMainCharacter : MonoBehaviour
{
    #region Events
    /// <summary>
    /// Interact event, exposed to environment.
    /// </summary>
    public UnityEvent Interact;
    #endregion
    #region Components
    /*----- Components -----*/
    /// <summary>
    /// The 2D rigidbody attached to this gameObject.
    /// </summary>
    private Rigidbody2D _rigidbody;

    /// <summary>
    /// The 2D physics material attached to this gameObject.
    /// </summary>
    private PhysicsMaterial2D _naturalForces;
    #endregion
    #region Link Managers
    /*----- Link Managers -----*/
    /// <summary>
    /// Active input manager.
    /// </summary>
    public NewInputManager InputManager;
    #endregion
    #region Player States
    /*----- Player States -----*/
    /// <summary>
    /// Character state.
    /// </summary>
    private CharacterState _state;

    /// <summary>
    /// Character state.
    /// </summary>
    public CharacterState State { get => _state; }

    /// <summary>
    /// Character direction.<br />
    /// ONLY: Left OR Null OR Right.
    /// </summary>
    private MoveDirection _direction;

    /// <summary>
    /// Character direction.<br />
    /// ONLY: Left OR Null OR Right.
    /// </summary>
    public MoveDirection Direction { get => _direction; }
    #endregion
    #region Basic Behavior
    /*----- Basic Behavior -----*/
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
    #region Button Behavior
    /*----- Button Behavior -----*/
    /// <summary>
    /// Handles jump event from input manager.<br />
    /// Should do: Jump OR Hop.
    /// </summary>
    private void _jump()
    {
        switch (State)
        {
            case CharacterState.InAirJumppable:
            case CharacterState.OnLadder:
                __hop(Direction);
                break;
            case CharacterState.OnGround:
                __jump(Direction);
                break;
            default:
                // Do nothing.
                break;
        }
    }

    /// <summary>
    /// Handles interact event from input manager.<br />
    /// Should do: Interact.
    /// </summary>
    private void _interact()
    {
        Interact.Invoke();
    }

    /// <summary>
    /// Handles left key event from input manager.<br />
    /// Should do: Glide left OR Walk left.
    /// </summary>
    private void _left()
    {
        switch (State)
        {
            case CharacterState.InAir:
            case CharacterState.InAirJumppable:
                __glide(MoveDirection.Left);
                break;
            case CharacterState.OnGround:
                __walk(MoveDirection.Left);
                break;
            default:
                // Do nothing.
                break;
        }
    }

    /// <summary>
    /// Handles right key event from input manager.<br />
    /// Should do: Glide right OR Walk right.
    /// </summary>
    private void _right()
    {
        switch (State)
        {
            case CharacterState.InAir:
            case CharacterState.InAirJumppable:
                __glide(MoveDirection.Right);
                break;
            case CharacterState.OnGround:
                __walk(MoveDirection.Right);
                break;
            default:
                // Do nothing.
                break;
        }
    }

    /// <summary>
    /// Handles up key event from input manager.<br />
    /// Should do: Grab the ladder OR Climb up the ladder.
    /// </summary>
    private void _up()
    {
        switch (State)
        {
            case CharacterState.OnLadder:
                __climb(MoveDirection.Up);
                break;
            default:
                // Do nothing.
                break;
        }
    }

    /// <summary>
    /// Handles down key event from input mananger.<br />
    /// Should do: Grab the ladder OR Climb down the ladder.
    /// </summary>
    private void _down()
    {
        switch (State)
        {
            case CharacterState.OnLadder:
                __climb(MoveDirection.Down);
                break;
            default:
                // Do nothing.
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

        // Bind input handler.
        InputManager.Left.AddListener(_left);
        InputManager.Right.AddListener(_right);
        InputManager.Up.AddListener(_up);
        InputManager.Down.AddListener(_down);
        InputManager.Jump.AddListener(_jump);
        InputManager.Interact.AddListener(_interact);
    }
    #endregion
}