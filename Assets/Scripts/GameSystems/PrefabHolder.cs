using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Stores a selection of prefabs that other scripts can access.
/// </summary>
public class PrefabHolder : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _prefabCollection;

    public GameObject GetFromPrefabCollection(Item item)
    {
        return _prefabCollection[(int)item];
    }

    public enum Item
    {
        PlayerHouse,
        TownCenter
    }
}
