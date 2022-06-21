using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the Changing of materials on a building object that houses multiple objects with their own renderers and materials.
/// </summary>
public class BuildingMaterialHandler : MonoBehaviour
{
    [SerializeField]
    private Material[] _materialCollection;
    private MeshRenderer[] _meshRenderers;

    private void Awake()
    {
        _meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
    }

    //Test Jank
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        ApplyMaterial(BuildingState.Default);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        ApplyMaterial(BuildingState.AllowedToBuild);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        ApplyMaterial(BuildingState.InProgress);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //    {
    //        ApplyMaterial(BuildingState.Error);
    //    }
    //}
    public void ApplyMaterial(BuildingState newState)
    {
        Material newMaterial = _materialCollection[(int)newState];
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material = newMaterial;
        }
    }
    public enum BuildingState
    {
        Default,
        AllowedToBuild,
        InProgress,
        Error
    }
}
