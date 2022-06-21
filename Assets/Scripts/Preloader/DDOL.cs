using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Preserves the object that houses general properties and behaviours.
/// Typically used for preloading objects in a preloader scene.
/// </summary>
public class DDOL : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Debug.LogFormat($"DDOL: {this.gameObject.name}");
    }
}
