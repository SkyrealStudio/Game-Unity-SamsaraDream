using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    /// <summary>
    /// Items in inventory.<br />
    /// Indexed by GameObject.name.
    /// </summary>
    public Dictionary<string, GameObject> Items;

    /// <summary>
    /// Add new item.
    /// </summary>
    public void Append(GameObject _Item)
    {
        Items.Add(_Item.name, _Item);
    }

    /// <summary>
    /// Add new items.
    /// </summary>
    public void Append(GameObject[] _Items)
    {
        foreach(GameObject Item in _Items)
        {
            Append(Item);
        }
    }

    /// <summary>
    /// Remove an item.
    /// </summary>
    public void Remove(string _Index)
    {
        Destroy(Items[_Index]);
        Items.Remove(_Index);
    }

    /// <summary>
    /// Remove items.
    /// </summary>
    public void Remove(string[] _Indexes)
    {
        foreach(string Index in _Indexes)
        {
            Remove(Index);
        }
    }

    /// <summary>
    /// Search the existence of an item.
    /// </summary>
    public bool Exist(string _Index)
    {
        return Items.ContainsKey(_Index);
    }
}