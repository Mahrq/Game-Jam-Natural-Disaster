using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles various inputs and the game state of the player controller.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode _houseShortcut = KeyCode.Alpha1;
    private ControllerState _state = ControllerState.Free;
    private PrefabHolder _prefabHolder;
    private BuildModeBlueprintBehaviour _selectedObjectToBuild;
    
    [SerializeField]
    private float _rayCastLength = 10000f;
    private RaycastHit _rayHit;
    Vector3 hitResult = Vector3.zero;
    [SerializeField]
    private LayerMask _rayHitMask;

    private void Awake()
    {
        _prefabHolder = FindObjectOfType<PrefabHolder>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _rayHit, _rayCastLength, _rayHitMask))
        {
            hitResult = _rayHit.point;
            Debug.Log($"Ray Hit Point:{hitResult}");
        }
        Debug.DrawRay(ray.origin, ray.direction * _rayCastLength, Color.red);
        switch (State)
        {
            case ControllerState.Free:
                if (Input.GetKeyUp(_houseShortcut))
                {
                    SetObjectToBuild(PrefabHolder.Item.PlayerHouse, hitResult);
                }
                Debug.Log($"Ray Hit Point:{hitResult}" +
                    $"\nFree Mode");
                break;
            case ControllerState.BuildMode:
                if (_selectedObjectToBuild != null)
                {
                    _selectedObjectToBuild.SetPosition(hitResult);
                    Debug.Log($"Ray Hit Point:{hitResult}" +
                        $"\nBuild Mode");
                    //Confirm build position with left click
                    if (Input.GetMouseButtonUp(0))
                    {
                        //Attempt to build, if successful, releases the selected object.
                        if (_selectedObjectToBuild.TryBuild())
                        {
                            _selectedObjectToBuild = null;
                        }
                    }
                    //Cancel build mode with right click
                    else if (Input.GetMouseButtonUp(1))
                    {
                        _selectedObjectToBuild.CancelBuildMode();
                    }
                }
                else
                {
                    State = ControllerState.Free;
                }
                break;
            case ControllerState.MenuMode:
                break;
            default:
                break;
        }
    }
    private void SetObjectToBuild(PrefabHolder.Item item, Vector3 position)
    {
        GameObject prefab = _prefabHolder.GetFromPrefabCollection(item);

        if (prefab != null)
        {
            //Check if the prefab that you're trying to spawn contains the behaviour.
            BuildModeBlueprintBehaviour checkComponent;
            if (prefab.TryGetComponent<BuildModeBlueprintBehaviour>(out checkComponent))
            {
                //Then grab the spawned object's behaviour for referencing.
                GameObject spawnedBp = Instantiate(prefab, position, Quaternion.identity);
                if (spawnedBp.TryGetComponent<BuildModeBlueprintBehaviour>(out _selectedObjectToBuild))
                {
                    State = ControllerState.BuildMode;
                }
            }
        }
    }
    public enum ControllerState
    {
        Free,
        BuildMode,
        MenuMode
    }

    public ControllerState State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
        }
    }
}
