using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Record collision datas.
/// </summary>
public class NewCollisionList : MonoBehaviour
{
    /// <summary>
    /// List of collisions on the attached gameObject.
    /// </summary>
    private Dictionary<GameObject, Collision2D> _collisions;

    /// <summary>
    /// List of collisions on the attached gameObject.
    /// </summary>
    public Dictionary<GameObject, Collision2D> Collisions { get => _collisions; }

    #region Unity Calls
    /*----- Unity Calls -----*/
    private void OnCollisionEnter2D(Collision2D _Collision)
    {
        _collisions.Add(_Collision.gameObject, _Collision);
    }

    private void OnCollisionStay2D(Collision2D _Collision)
    {
        _collisions[_Collision.gameObject] = _Collision; 
    }

    private void OnCollisionExit2D(Collision2D _Collision)
    {
        _collisions.Remove(_Collision.gameObject);
    }
    #endregion
}