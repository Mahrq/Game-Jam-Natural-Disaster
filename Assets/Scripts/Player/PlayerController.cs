using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
/// <summary>
/// Handles various inputs and the game state of the player controller.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode _houseShortcut = KeyCode.Alpha1;
    [SerializeField]
    private KeyCode _resourceMineShortcut = KeyCode.Alpha2;
    [SerializeField]
    private KeyCode _townCenterShortCut = KeyCode.T;
    private ControllerState _state = ControllerState.Free;
    private ControllerState _previousState;
    private PrefabHolder _prefabHolder;
    private BuildModeBlueprintBehaviour _selectedObjectToBuild;
    private BuildModeBlueprintBehaviour _selectedBuilding;
    //private SelectableObjectHandler _mouseSelected;
    private bool _selectedFromButton = false;

    [SerializeField]
    private float _rayCastLength = 10000f;
    private Ray ray;
    private RaycastHit _rayHit;
    private Vector3 hitResult = Vector3.zero;
    private int _hitmask;

    public event System.Action OnDeselectedBuilding;

    private void Awake()
    {
        _prefabHolder = FindObjectOfType<PrefabHolder>();

        _hitmask = LayerMask.GetMask("Structure");
    }
    private void OnEnable()
    {
        CreateBuildingButton.OnCreateBuildingButtonClicked += OnCreateBuildingButtonClickedCallBack;
    }
    private void OnDisable()
    {
        CreateBuildingButton.OnCreateBuildingButtonClicked -= OnCreateBuildingButtonClickedCallBack;
    }
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * _rayCastLength, Color.red);
        hitResult = CursorToWorldSpace(ray, hitResult);
        switch (State)
        {
            case ControllerState.Free:
                if (Input.GetKeyUp(_houseShortcut))
                {
                    //Pre-emptivly change the layermask and recast the raycast so that when the blueprint spawns
                    //it won't jump from the last valid raycast result on the floor layer.
                    _selectedFromButton = false;
                    _hitmask = LayerMask.GetMask("Floor");
                    hitResult = CursorToWorldSpace(ray, hitResult);
                    SetObjectToBuild(PrefabHolder.Item.PlayerHouse, hitResult);
                }
                else if (Input.GetKeyUp(_resourceMineShortcut))
                {
                    _selectedFromButton = false;
                    _hitmask = LayerMask.GetMask("Floor");
                    hitResult = CursorToWorldSpace(ray, hitResult);
                    SetObjectToBuild(PrefabHolder.Item.ResourceMine, hitResult);
                }
                else if (Input.GetKeyUp(_townCenterShortCut))
                {
                    _selectedFromButton = false;
                    _hitmask = LayerMask.GetMask("Floor");
                    hitResult = CursorToWorldSpace(ray, hitResult);
                    SetObjectToBuild(PrefabHolder.Item.TownCenter, hitResult);
                }
                break;
            case ControllerState.BuildMode:
                //If a building is selected while trying to go into build mode via button click remove the selection first.
                if (_selectedBuilding)
                {
                    _selectedBuilding.IsSelected = false;
                    _selectedBuilding = null;
                    OnDeselectedBuilding?.Invoke();
                }
                if (_selectedObjectToBuild != null)
                {
                    _selectedObjectToBuild.SetPosition(hitResult);
                    //Confirm build position with left click
                    //When choosing from a button click, it will skip the current frame the mouse released.
                    if (!_selectedFromButton)
                    {
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
                    _selectedFromButton = false;
                }
                else
                {
                    State = ControllerState.Free;
                }
                break;
            case ControllerState.MenuMode:
                break;
            case ControllerState.HasSelection:
                if (_selectedBuilding != null)
                {
                    //Clicking the left mouse down while having a selection will attempt to deselect the chosen building.
                    //if the cursor is over a selectable building, it will reselect it on mouseclick up. 
                    if (Input.GetMouseButtonDown(0))
                    {
                        _selectedBuilding.IsSelected = false;
                        _selectedBuilding = null;
                        OnDeselectedBuilding?.Invoke();
                        State = ControllerState.Free;
                    }
                }
                else
                {
                    State = ControllerState.Free;
                }
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
    //Delta position used as a default result if the ray cast fails.
    private Vector3 CursorToWorldSpace(Ray ray, Vector3 deltaPosition)
    {
        Vector3 result = deltaPosition;
        if (Physics.Raycast(ray, out _rayHit, _rayCastLength, _hitmask))
        {
            result = _rayHit.point;
        }
        return result;
    }
    private void OnCreateBuildingButtonClickedCallBack(PrefabHolder.Item item)
    {
        if (State != ControllerState.BuildMode)
        {
            _hitmask = LayerMask.GetMask("Floor");
            hitResult = CursorToWorldSpace(ray, hitResult);
            _selectedFromButton = true;
            SetObjectToBuild(item, hitResult);
        }
    }
    public BuildModeBlueprintBehaviour SelectedBuilding
    {
        set
        {
            _selectedBuilding = value;
            _selectedBuilding.IsSelected = true;
            State = ControllerState.HasSelection;
        }
    }
    //public SelectableObjectHandler MouseSelected
    //{
    //    set
    //    {
    //        _mouseSelected = value;
    //        _selectedBuilding = _mouseSelected.Blueprint;
    //        _selectedBuilding.IsSelected = true;
    //        State = ControllerState.HasSelection;
    //    }
    //}
    public enum ControllerState
    {
        Free,
        BuildMode,
        MenuMode,
        HasSelection
    }
    public ControllerState PreviousState => _previousState;
    public ControllerState State
    {
        get
        {
            return _state;
        }
        set
        {
            _previousState = _state;
            _state = value;
            switch (_state)
            {
                case ControllerState.Free:
                    _hitmask = LayerMask.GetMask("Structure");
                    break;
                case ControllerState.BuildMode:
                    break;
                case ControllerState.MenuMode:
                    //Prevent casting on buildings while in menu mode.
                    _hitmask = LayerMask.GetMask("Floor");
                    break;
                case ControllerState.HasSelection:
                    _hitmask = LayerMask.GetMask("Structure");
                    break;
                default:
                    break;
            }
        }
    }
}
