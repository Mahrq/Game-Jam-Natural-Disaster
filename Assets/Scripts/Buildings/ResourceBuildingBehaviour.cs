using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuildingBehaviour : MonoBehaviour, IMapableUI<Vector3>
{
    [Header("Assign")]
    [SerializeField]
    private ResourceProperties _properties;
    [SerializeField]
    private PlayerEconomy _resourceLabel = PlayerEconomy.Money;
    private BuildModeBlueprintBehaviour _blueprint;
    private PlayerData _platerData;
    private int _currentResourceAmount;
    private int _currentWorkers;
    private int _latestEffectIndex;
    public event IMapableUI<Vector3>.MapToUIDelegate OnValueChanged;

    private IEnumerator[] _lockedEffectCoroutines;
    private void Awake()
    {
        _blueprint = this.GetComponent<BuildModeBlueprintBehaviour>();
        _platerData = FindObjectOfType<PlayerData>();
        _currentResourceAmount = _properties.ResourceAmount;
        _lockedEffectCoroutines = ConstructEffectLibrary();
        _currentWorkers = 0;
        _latestEffectIndex = -1;
    }
    private void OnEnable()
    {
        _blueprint.OnDeathPrep += ReleaseWorkers;
        DayTracker.OnNewDay += ReleaseWorkers;
    }
    private void OnDisable()
    {
        _blueprint.OnDeathPrep -= ReleaseWorkers;
        DayTracker.OnNewDay -= ReleaseWorkers;
    }
    //Workers can only be added one by one so when one worker is added,
    //it will check if the current amount of workers is enough to unlock an effect.
    public void AddWorker(int amount)
    {
        if (CurrentWorkers < _properties.WorkerCap && _platerData.Population.Idle > 0) 
        {
            _platerData.Population.AddIdlePopulation(-amount);
            CurrentWorkers += amount;
        }
    }
    private void UnlockEffect(int index)
    {
        StartCoroutine(_lockedEffectCoroutines[index]);
    }
    private void LockEffect(int index)
    {
        StopCoroutine(_lockedEffectCoroutines[index]);
    }

    public void ReleaseWorkers()
    {
        if (CurrentWorkers > 0)
        {
            StopAllCoroutines();
            //Reconstructing the coroutine array fixes the bug
            //where you would instantly gain the resources
            //when sending workers back in after releasing them
            _lockedEffectCoroutines = ConstructEffectLibrary();
            _platerData.Population.AddIdlePopulation(_currentWorkers);
            _currentWorkers = 0;
            _latestEffectIndex = -1;
            Vector3 changedData = new Vector3(CurrentWorkers, _properties.WorkerCap, CurrentResourceAmount);
            OnValueChanged?.Invoke(changedData);
        }
    }

    private IEnumerator ContinuousEffect(int index, float interval)
    {
        for(; ; )
        {
            yield return new WaitForSeconds(interval);
            if (CurrentResourceAmount >= _properties.WorkersRequiredPerLockedEffect)
            {
                _properties.LockedEffects[index].ApplyEffect(ref _platerData);
                if (!_properties.IsInfinite)
                {
                    CurrentResourceAmount -= _properties.LockedEffects[index].Value;
                }
            }
        }
    }
    private bool TryGetEffectIndex(int _currentWorkers, out int result)
    {
        result = _currentWorkers / _properties.WorkersRequiredPerLockedEffect;
        result--;
        return result < 0 ? false : true;
    }
    private IEnumerator[] ConstructEffectLibrary()
    {
        IEnumerator[] lib = new IEnumerator[_properties.LockedEffects.Length];
        for (int i = 0; i < lib.Length; i++)
        {
            lib[i] = ContinuousEffect(i, _properties.LockedEffects[i].RecurisonInterval);
        }
        return lib;
    }
    public int CurrentWorkers
    {
        get
        {
            return _currentWorkers;
        }
        private set
        {
            //Removed implementation for single removal of worker.
            _currentWorkers = value;
            Vector3 changedData = new Vector3(CurrentWorkers, _properties.WorkerCap, CurrentResourceAmount);
            OnValueChanged?.Invoke(changedData);
            int index;
            if (TryGetEffectIndex(_currentWorkers, out index))
            {
                if (index > _latestEffectIndex)
                {
                    UnlockEffect(index);
                }
                _latestEffectIndex = index;
            }
        }
    }
    public int CurrentResourceAmount
    {
        get
        {
            return _currentResourceAmount;
        }
        private set
        {
            _currentResourceAmount = value;
            _currentResourceAmount = Mathf.Clamp(_currentResourceAmount, 0, _properties.ResourceAmount);
            Vector3 changedData = new Vector3(CurrentWorkers, _properties.WorkerCap, CurrentResourceAmount);
            OnValueChanged?.Invoke(changedData);
            if (_currentResourceAmount <= 0)
            {
                ReleaseWorkers();
                _blueprint.DamageHandler.ModifyHealth(int.MaxValue);
            }
        }
    }
    public PlayerEconomy ResourceLabel => _resourceLabel;
    public ResourceProperties R_Properties => _properties;
}
