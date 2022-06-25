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

    public event System.Action OnResourceDepleted;
    public event IMapableUI<Vector3>.MapToUIDelegate OnValueChanged;

    private IEnumerator[] _lockedEffectCoroutines;
    private void Awake()
    {
        _blueprint = this.GetComponent<BuildModeBlueprintBehaviour>();
        _platerData = FindObjectOfType<PlayerData>();
        _currentResourceAmount = _properties.ResourceAmount;
        _lockedEffectCoroutines = new IEnumerator[_properties.LockedEffects.Length];
        for (int i = 0; i < _lockedEffectCoroutines.Length; i++)
        {
            _lockedEffectCoroutines[i] = ContinuousEffect(i, _properties.LockedEffects[i].RecurisonInterval);
        }
        _currentWorkers = 0;
        _latestEffectIndex = -1;
    }
    //Workers can only be added one by one so when one worker is added,
    //it will check if the current amount of workers is enough to unlock an effect.
    public void AddWorker(int amount)
    {
        if (CurrentWorkers < _properties.WorkerCap)
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

    private void ReleaseWorkers()
    {
        StopAllCoroutines();
        _platerData.Population.AddIdlePopulation(_currentWorkers);
        _currentWorkers = 0;
    }

    private IEnumerator ContinuousEffect(int index, float interval)
    {
        for(; ; )
        {
            if (CurrentResourceAmount >= _properties.WorkersRequiredPerLockedEffect)
            {
                _properties.LockedEffects[index].ApplyEffect(ref _platerData);
                if (!_properties.IsInfinite)
                {
                    CurrentResourceAmount -= _properties.LockedEffects[index].Value;
                }
            }
            yield return new WaitForSeconds(interval);
        }
    }
    private bool TryGetEffectIndex(int _currentWorkers, out int result)
    {
        result = _currentWorkers / _properties.WorkersRequiredPerLockedEffect;
        result--;
        return result < 0 ? false : true;
    }
    public int CurrentWorkers
    {
        get
        {
            return _currentWorkers;
        }
        private set
        {
            int previous = _currentWorkers;
            _currentWorkers = value;
            Vector3 changedData = new Vector3(CurrentWorkers, _properties.WorkerCap, CurrentResourceAmount);
            OnValueChanged?.Invoke(changedData);
            if (_currentWorkers > previous)
            {
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
            else if (_currentWorkers < previous)
            {
                LockEffect(_latestEffectIndex);
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
                OnResourceDepleted?.Invoke();
                StopAllCoroutines();
            }
        }
    }
    public PlayerEconomy ResourceLabel => _resourceLabel;
    public ResourceProperties R_Properties => _properties;
}
