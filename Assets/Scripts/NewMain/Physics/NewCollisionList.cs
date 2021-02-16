using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Record collision datas.
/// </summary>
public class NewCollisionList : MonoBehaviour
{
    #region Collision List
    /*----- Collision List -----*/
    /// <summary>
    /// List of collisions on the attached gameObject.
    /// </summary>
    private Dictionary<GameObject, Collision2D> _collisions = new Dictionary<GameObject, Collision2D>();

    /// <summary>
    /// List of collisions on the attached gameObject.
    /// </summary>
    public Dictionary<GameObject, Collision2D> Collisions { get => _collisions; }

    /// <summary>
    /// Detect if there is a tagged game object in list.
    /// </summary>
    public Dictionary<GameObject, Collision2D> TaggedList(string _Tag)
    {
        Dictionary<GameObject, Collision2D> ret = new Dictionary<GameObject, Collision2D>();
        foreach (KeyValuePair<GameObject, Collision2D> line in Collisions)
        {
            if(line.Key.tag == _Tag)
            {
                ret.Add(line.Key, line.Value);
            }
        }
        return ret;
    }
    #endregion
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