using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Determines the behaviour of the building during it's building process.
/// 
/// This object should be on the Structure layer so it can do collision checks with other
/// building objects.
/// </summary>
public class BuildModeBlueprintBehaviour : MonoBehaviour, IMapableUI<Vector3>
{
    [Header("Must Assign")]
    [SerializeField]
    private BuildingProperties _buildingProperties;
    private bool _buildingEffectActivated = false;
    private BuildingMaterialHandler _buildingMaterialHandler;
    private BuildingDamageHandler _damageHandler;
    private PlayerData _playerData;
    [SerializeField]
    [Tooltip("For buildings already active in the scene select 'Done'")]
    private BuildingStage _startingStage = BuildingStage.Preparing;
    [SerializeField]
    private BuildingStage _currentStage;
    private bool _canBuild;
    private bool _isColliding;
    private Transform _thistransform;
    private float _currentBuildTimer = 0f;
    [SerializeField]
    [Range(1, 60)]
    [Tooltip("Time in frames when the next Value Changed interval is called.")]
    private int _valueChangedEventSendInterval = 5;
    private Vector3 _progressBarMapData;

    public event IMapableUI<Vector3>.MapToUIDelegate OnValueChanged;
    public event System.Action OnInitialise;
    public event System.Action<bool> OnSelectionChanged;
    private bool _isSelected = false;
    private void Awake()
    {
        _buildingMaterialHandler = this.GetComponent<BuildingMaterialHandler>();
        _damageHandler = this.GetComponent<BuildingDamageHandler>();
        _playerData = FindObjectOfType<PlayerData>();
        _thistransform = this.GetComponent<Transform>();
    }

    private void Start()
    {
        if (_startingStage != BuildingStage.Initialise)
        {
            CurrentStage = BuildingStage.Preparing;
        }
        else
        {
            _currentBuildTimer = _buildingProperties.BuildTime;
            _progressBarMapData = new Vector3(_currentBuildTimer, 0, _buildingProperties.BuildTime);
            CurrentStage = BuildingStage.Initialise;
        }
    }

    private void OnDestroy()
    {
        if (_buildingEffectActivated)
        {
            RemoveBuildingEffects();
        }
        if (CurrentStage == BuildingStage.Initialise)
        {
            _damageHandler.OnDeath -= OnDeathCallback;
        }
    }
    private void Update()
    {
        switch (CurrentStage)
        {
            case BuildingStage.Moving:
                //TODO: make event system to check if can build so it doesn't perform this check on update.
                if (CanBuild)
                {
                    _buildingMaterialHandler.ApplyMaterial(BuildingMaterialHandler.BuildingState.AllowedToBuild);
                }
                else
                {
                    _buildingMaterialHandler.ApplyMaterial(BuildingMaterialHandler.BuildingState.Error);
                }
                break;
            case BuildingStage.Building:
                _currentBuildTimer += Time.deltaTime;
                if (BuildComplete())
                {
                    ReturnWorkers();
                    CurrentStage = BuildingStage.Initialise;
                }
                break;
            default:
                break;
        }
        if (Time.frameCount % _valueChangedEventSendInterval == 0)
        {
            if (OnValueChanged != null)
            {
                _progressBarMapData = new Vector3(_currentBuildTimer, 0, _buildingProperties.BuildTime);
                OnValueChanged(_progressBarMapData);
            }
        }
    }
    //Objects on the Structure layer can only collide with eachother,
    //therefore only bother to check if the object collided with has it's own instance of this behaviour
    private void OnTriggerStay(Collider other)
    {
        BuildModeBlueprintBehaviour otherBuilding;
        if (other.TryGetComponent<BuildModeBlueprintBehaviour>(out otherBuilding))
        {
            _isColliding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        BuildModeBlueprintBehaviour otherBuilding;
        if (other.TryGetComponent<BuildModeBlueprintBehaviour>(out otherBuilding))
        {
            _isColliding = false;
        }
    }
    private void ApplyBuildingEffects()
    {
        if (_buildingProperties.BuildingEffects.Length > 0)
        {
            BuildingEffect currentEffect;
            for (int i = 0; i < _buildingProperties.BuildingEffects.Length; i++)
            {
                currentEffect = _buildingProperties.BuildingEffects[i];
                if (currentEffect.RecursValue)
                {
                    float interval = currentEffect.RecurisonInterval;
                    StartCoroutine(RecurringEffect(i, interval));
                }
                else
                {
                    currentEffect.ApplyEffect(ref _playerData);
                }
            }
            _buildingEffectActivated = true;
        }
    }
    private IEnumerator RecurringEffect(int index, float interval)
    {
        //Wait for forloop to finish applying all effects before applying continuous effects
        while (!_buildingEffectActivated)
        {
            yield return null;
        }
        while (_buildingEffectActivated)
        {
            _buildingProperties.BuildingEffects[index].ApplyEffect(ref _playerData);
            yield return new WaitForSecondsRealtime(interval);
        }
    }
    private void RemoveBuildingEffects()
    {
        if (_buildingProperties.BuildingEffects.Length > 0)
        {
            StopAllCoroutines();
            BuildingEffect currentEffect;
            for (int i = 0; i < _buildingProperties.BuildingEffects.Length; i++)
            {
                currentEffect = _buildingProperties.BuildingEffects[i];
                //Non recuring effects are removed while recurring ones no longer apply
                //becuase the coroutine has stopped.
                if (!currentEffect.RecursValue)
                {
                    currentEffect.RemoveEffect(ref _playerData);
                }
            }
            _buildingEffectActivated = false;
        }
    }
    private void OnDeathCallback()
    {
        //TODO: special FX for death otherwise it's same as normal destruction;
        Destroy(this.gameObject);
    }
    public void SetPosition(Vector3 position)
    {
        ThisTransform.position = position;
    }
    public bool BuildComplete() => _currentBuildTimer >= _buildingProperties.BuildTime;
    public bool TryBuild()
    {
        if (CanBuild)
        {
            //Subtract Cost of the building to the player's resources
            _playerData.AddPayment(-_buildingProperties.CostToBuild);
            CurrentStage = BuildingStage.Building;
            return true;
        }
        return false;
    }
    public void CancelBuild()
    {
        //Refund the player of their resources if build in progress was cancelled;
        _playerData.AddPayment(_buildingProperties.CostToBuild);
        Destroy(this.gameObject);
    }
    public void CancelBuildMode()
    {
        Destroy(this.gameObject);
    }
    //Return workers as idle population that were sent as cost.
    public void ReturnWorkers()
    {
        int returningWorkers = (int)_buildingProperties.CostToBuild.x;
        _playerData.Population.AddIdlePopulation(returningWorkers);
    }
    public BuildingStage CurrentStage
    {
        get
        {
            return _currentStage;
        }
        private set
        {
            _currentStage = value;
            switch (_currentStage)
            {
                case BuildingStage.Preparing:
                    if (CanBuild)
                    {
                        _buildingMaterialHandler.ApplyMaterial(BuildingMaterialHandler.BuildingState.AllowedToBuild);
                    }
                    else
                    {
                        _buildingMaterialHandler.ApplyMaterial(BuildingMaterialHandler.BuildingState.Error);
                    }
                    CurrentStage = BuildingStage.Moving;
                    break;
                case BuildingStage.Building:
                    _currentBuildTimer = 0f;
                    _buildingMaterialHandler.ApplyMaterial(BuildingMaterialHandler.BuildingState.InProgress);
                    break;
                case BuildingStage.Initialise:
                    _buildingMaterialHandler.ApplyMaterial(BuildingMaterialHandler.BuildingState.Default);
                    ApplyBuildingEffects();
                    _damageHandler.OnDeath += OnDeathCallback;
                    OnInitialise?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
    public bool CanBuild
    {
        get
        {
            _canBuild = _buildingProperties.CanBuild(_playerData.GetPayment) && !_isColliding;
            return _canBuild;
        }
    }
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            OnSelectionChanged?.Invoke(_isSelected);
        }
    }
    public BuildingProperties BuildProperties
    {
        get
        {
            if (_buildingProperties == null)
            {
                _buildingProperties = this.GetComponent<BuildingProperties>();
            }
            return _buildingProperties;
        }
    }
    public Transform ThisTransform
    {
        get
        {
            if (_thistransform == null)
            {
                _thistransform = this.GetComponent<Transform>();
            }
            return _thistransform;
        }
    }
    public BuildingDamageHandler DamageHandler
    {
        get
        {
            if (_damageHandler == null)
            {
                _damageHandler = this.GetComponent<BuildingDamageHandler>();
            }
            return _damageHandler;
        }
    }
    public enum BuildingStage
    {
        Preparing,
        Moving,
        Building,
        Initialise
    }
}
