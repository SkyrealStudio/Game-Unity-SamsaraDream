using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

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
    #region Link Managers
    /*----- Link Managers -----*/
    /// <summary>
    /// Active input manager.
    /// </summary>
    public NewInputManager InputManager;
    #endregion
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

    /// <summary>
    /// The list of collisions of this game object.
    /// </summary>
    private NewCollisionList _collisionList;
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

    /// <summary>
    /// State whether the character can climb.
    /// </summary>
    private bool _touchingLadder = false;
    #endregion
    #region Basic Behavior
    /*----- Basic Behavior -----*/
    /// <summary>
    /// Perform a jump from ground.
    /// </summary>
    private void __jump(MoveDirection _Direction)
    {
        _rigidbody.AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
        switch (_Direction)
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
    }

    /// <summary>
    /// Perform a hop in the air.
    /// </summary>
    private void __hop(MoveDirection _Direction)
    {
        _rigidbody.AddForce(new Vector2(0f, 3f), ForceMode2D.Impulse);
        switch (_Direction)
        {
            case MoveDirection.Left:
                _rigidbody.AddForce(new Vector2(-.75f, 0f), ForceMode2D.Impulse);
                break;
            case MoveDirection.Right:
                _rigidbody.AddForce(new Vector2(.75f, 0f), ForceMode2D.Impulse);
                break;
            default:
                // No side acceleration.
                break;
        }
    }

    /// <summary>
    /// Walk on the ground.
    /// </summary>
    private void __walk(MoveDirection _Direction)
    {
        switch (_Direction)
        {
            case MoveDirection.Left:
                if (_rigidbody.velocity.x > -5f)
                {
                    _rigidbody.AddForce(new Vector2(-20f, 0f));
                }
                break;
            case MoveDirection.Right:
                if (_rigidbody.velocity.x < 5f)
                {
                    _rigidbody.AddForce(new Vector2(20f, 0f));
                }
                break;
            default:
                // No side acceleration.
                break;
        }
    }

    /// <summary>
    /// Glide in the air.
    /// </summary>
    private void __glide(MoveDirection _Direction)
    {
        switch (_Direction)
        {
            case MoveDirection.Left:
                if(_rigidbody.velocity.x > -3f)
                {
                    _rigidbody.AddForce(new Vector2(-4f, 0f));
                }
                break;
            case MoveDirection.Right:
                if(_rigidbody.velocity.x < 3f)
                {
                    _rigidbody.AddForce(new Vector2(4f, 0f));
                }
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
                _state = CharacterState.InAir;
                __hop(Direction);
                break;
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
        _direction = MoveDirection.Left;
        switch (State)
        {
            case CharacterState.InAir:
                __glide(MoveDirection.Left);
                break;
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
        _direction = MoveDirection.Right;
        switch (State)
        {
            case CharacterState.InAir:
                __glide(MoveDirection.Right);
                break;
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
        if (_touchingLadder)
        {
            _state = CharacterState.OnLadder;
            __climb(MoveDirection.Up);
        }
    }

    /// <summary>
    /// Handles down key event from input mananger.<br />
    /// Should do: Grab the ladder OR Climb down the ladder.
    /// </summary>
    private void _down()
    {
        if (_touchingLadder)
        {
            _state = CharacterState.OnLadder;
            __climb(MoveDirection.Down);
        }
    }

    /// <summary>
    /// Handles no horizontal key event from input manager.<br />
    /// Should do: Reset direction AND Stop animation.
    /// </summary>
    private void _horizontalStop()
    {
        _direction = MoveDirection.Null;
    }

    /// <summary>
    /// Handles no vertical key event from input manager.<br />
    /// Should do: Stop animation.
    /// </summary>
    private void _verticalStop()
    {
    }
    #endregion
    #region Unity Calls
    /*----- Unity Calls -----*/
    private void Awake()
    {
        // Bind.
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _naturalForces = GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider2D>().sharedMaterial;
        _collisionList = gameObject.GetComponent<NewCollisionList>();
        
        // Initialization.
        _rigidbody.freezeRotation = true;
        _rigidbody.angularDrag = 0f;
        _rigidbody.drag = .1f;
        _rigidbody.mass = 1f;
        _naturalForces.bounciness = 0f;

        // Bind input handler.
        InputManager.Left.AddListener(_left);
        InputManager.Right.AddListener(_right);
        InputManager.Up.AddListener(_up);
        InputManager.Down.AddListener(_down);
        InputManager.Jump.AddListener(_jump);
        InputManager.Interact.AddListener(_interact);
        InputManager.NoHorizontal.AddListener(_horizontalStop);
        InputManager.NoVertical.AddListener(_verticalStop);
    }

    private void FixedUpdate()
    {
        if (State == CharacterState.OnGround)
        {
            if (_naturalForces.friction < .9f)
            {
                _naturalForces.friction += Time.fixedDeltaTime;
            }
            else
            {
                _naturalForces.friction = .9f;
            }
        }
        else
        {
            _naturalForces.friction = 0f;
        }
        Dictionary<GameObject, Collision2D> gnd = _collisionList.TaggedList("Ground");
        _touchingLadder = _collisionList.TaggedList("Ladder").Count > 0;
        bool SteppingGround = false;
        foreach (Collision2D collision in gnd.Values)
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (transform.position.y - point.point.y >= 1f)
                {
                    SteppingGround = true;
                }
            }
        }
        if (SteppingGround)
        {
            _state = CharacterState.OnGround;
        }
        else
        {
            if (State != CharacterState.InAir && State != CharacterState.InAirJumppable)
            {
                _state = CharacterState.InAirJumppable;
            }
        }
    }
    #endregion
}