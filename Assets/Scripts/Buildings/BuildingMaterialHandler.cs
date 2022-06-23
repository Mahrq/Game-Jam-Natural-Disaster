using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the Changing of materials on a building object that houses multiple objects with their own renderers and materials.
/// </summary>
public class BuildingMaterialHandler : MonoBehaviour
{
    [SerializeField]
    private Material[] _materialBuildModeStates;
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
        //Some models within the collection that makes up the building uses a different texture map.
        if (newState == BuildingState.Default)
        {
            Material[] newMaterials = _materialCollection;
            string tag = "";
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                tag = _meshRenderers[i].gameObject.tag;
                switch (tag)
                {
                    case "Atlas1":
                        _meshRenderers[i].material = newMaterials[(int)TextureMaps.Atlas1];
                        break;
                    case "Atlas2":
                        _meshRenderers[i].material = newMaterials[(int)TextureMaps.Atlas2];
                        break;                                               
                    default:
                        break;
                }              
            }
        }
        else
        {
            Material newMaterial = _materialBuildModeStates[(int)newState - 1];
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material = newMaterial;
            }
        }

    }
    public enum BuildingState
    {
        Default,
        AllowedToBuild,
        InProgress,
        Error
    }

    private enum TextureMaps
    {
        Atlas1,
        Atlas2
    }
}
