using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Extensions;
public class BuildingActionButton : CreateBuildingButton
{
    [SerializeField]
    private ActionButtonProperties _actionProperties;
    [SerializeField]
    private BuildingActions _buildingAction;
    [SerializeField]
    private Image _imgActionProgress;
    private float _actionDelayCounter;
    private bool _hasStarted = false;
    private SelectedInfoUI _selectedData;
    private PlayerData _playerData;
    private bool CanAfford => _actionProperties.CanAffordCost(_playerData.GetPayment);
    private delegate bool CanActivateMethod();
    private CanActivateMethod _canActivate;

    private SelectedDisplayButtonsUI _timerPreserver;
    private ActionTimerData _currentTimerData;
    private int _previousOwner;
    protected override void Awake()
    {
        base.Awake();
        _selectedData = this.GetComponentInParent<SelectedInfoUI>();
        _timerPreserver = this.GetComponentInParent<SelectedDisplayButtonsUI>();
        _playerData = FindObjectOfType<PlayerData>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _imgActionProgress.gameObject.SetActive(false);
        GetActivationCondition(_buildingAction);
    }
    protected virtual void OnDisable()
    {
        _canActivate = null;
        if (_actionProperties.ActivationDelay > 0 && _selectedData.SelectedObjectData && _hasStarted)
        {
            _previousOwner = _selectedData.SelectedObjectData.Blueprint.ID;
            _hasStarted = false;
            _timerPreserver.AddToPreservedTimers(new ActionTimerData(this, _selectedData.SelectedObjectData.Blueprint.ID));
        }
    }
    protected virtual void Update()
    {
        if (_hasStarted)
        {
            _actionDelayCounter += Time.deltaTime;
            if (Time.frameCount % 5 == 0)
            {
                MapActionProgress(new Vector3(_actionDelayCounter, 0, _actionProperties.ActivationDelay));
            }
            if (_actionDelayCounter >= _actionProperties.ActivationDelay)
            {
                _actionDelayCounter = 0f;
                _hasStarted = false;
                ClickAction(_buildingAction);
                if (_currentTimerData != null)
                {
                    _currentTimerData = null;
                }
                if (_imgActionProgress.gameObject.activeInHierarchy)
                {
                    _imgActionProgress.gameObject.SetActive(false);
                }
            }
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!_buildingDetailsToolTip.activeInHierarchy)
        {
            MapDetailsToUI();
            _toolTipTransfrom.SetParent(_rTransform);
            _toolTipTransfrom.anchoredPosition = Vector2.zero + offsetVector;
            _buildingDetailsToolTip.SetActive(true);
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (_canActivate != null && _canActivate())
        {
            if (CanAfford)
            {
                //For instant activation effects.
                if (_actionProperties.ActivationDelay == 0)
                {
                    _playerData.AddPayment(-_actionProperties.CostToActivate);
                    ClickAction(_buildingAction);
                }
                else if (!_hasStarted)
                {
                    _playerData.AddPayment(-_actionProperties.CostToActivate);
                    _actionDelayCounter = 0f;
                    MapActionProgress(new Vector3(_actionDelayCounter, 0, _actionProperties.ActivationDelay));
                    _hasStarted = true;
                }
            }
        }
    }
    protected override void MapDetailsToUI()
    {
        Vector3 cost = _actionProperties.CostToActivate;
        _txtName.text = _actionProperties.Name;
        _txtDescription.text = _actionProperties.Description;
        _txtCostPopulation.text = $"{cost.x}";
        _txtCostMoney.text = $"{cost.y}";
        _txtCostBuildingMaterial.text = $"{cost.z}";
    }
    private void MapActionProgress(Vector3 input)
    {
        float current = input.x;
        float min = input.y;
        float max = input.z;
        current = Mathf.Clamp(current, min, max);
        float result = MathUtilities.GetPercentage(current, min, max);
        _imgActionProgress.fillAmount = result;
        if (!_imgActionProgress.gameObject.activeInHierarchy)
        {
            _imgActionProgress.gameObject.SetActive(true);
        }
    }
    private void ClickAction(BuildingActions action)
    {
        switch (action)
        {
            case BuildingActions.None:
                break;
            case BuildingActions.SendWorker:
                _selectedData.SelectedObjectData.Resource.AddWorker(1);
                break;
            case BuildingActions.RetrieveWorker:
                _selectedData.SelectedObjectData.Resource.ReleaseWorkers();
                break;
            case BuildingActions.RepairBuilding:
                break;
            case BuildingActions.DestroyBuilding:
                if (_selectedData.SelectedObjectData.Blueprint != null)
                {
                    _selectedData.SelectedObjectData.Blueprint.DamageHandler.ModifyHealth(int.MaxValue);
                }
                break;
            case BuildingActions.IncreasePopulation:
                _playerData.Population.AddCurrentPopulation(1);
                break;
            default:
                break;
        }
    }
    public void ClickAction()
    {
        ClickAction(_buildingAction);
        if (_currentTimerData != null)
        {
            _currentTimerData = null;
        }
    }
    private void GetActivationCondition(BuildingActions actions)
    {
        switch (actions)
        {
            case BuildingActions.None:
                break;
            case BuildingActions.SendWorker:
                _canActivate = () => true;
                break;
            case BuildingActions.RetrieveWorker:
                _canActivate = () => true;
                break;
            case BuildingActions.RepairBuilding:
                if (_selectedData.SelectedObjectData != null)
                {
                    _canActivate = _selectedData.SelectedObjectData.Blueprint.DamageHandler.CanRepair;
                }
                break;
            case BuildingActions.DestroyBuilding:
                _canActivate = () => true;
                break;
            case BuildingActions.IncreasePopulation:
                _canActivate = () => _playerData.Population.Current < _playerData.Population.MaxLimit;
                break;
            default:
                break;
        }
    }
    public ActionTimerData CurrentTimerData
    {
        get
        {
            return _currentTimerData;
        }
        set
        {
            _currentTimerData = value;
            _actionDelayCounter = _currentTimerData.CurrentTimer;
            _hasStarted = true;
        }
    }
    public class ActionTimerData
    {
        private string _name;
        public string Name => _name;
        private float _currentTimer;
        public float CurrentTimer { get { return _currentTimer; } set { _currentTimer = value; } }
        private BuildingActionButton _retriever;
        public BuildingActionButton Retriever => _retriever;
        private float _timerLimit;
        public float TimerLimit => _timerLimit;
        private int _uid;
        public int UID => _uid;
        private bool _hasExecuted;
        public bool HasExecuted => _hasExecuted;
        public ActionTimerData(BuildingActionButton retriever, int uid)
        {
            _name = retriever._selectedData.SelectedObjectData.Blueprint.gameObject.name;
            _currentTimer = retriever._actionDelayCounter;
            _timerLimit = retriever._actionProperties.ActivationDelay;
            _retriever = retriever;
            _uid = uid;
            _hasExecuted = false;
        }
        public void InvokeAction()
        {
            _hasExecuted = true;
            _currentTimer = 0f;
            _retriever.ClickAction();
        }
    }
}
