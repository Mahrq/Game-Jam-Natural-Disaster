using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class PlayerPopulation
{
    [SerializeField]
    private int _idle;
    [SerializeField]
    private int _current;
    [SerializeField]
    private int _maxLimit;
    public int Idle => _idle;
    public int Current => _current;
    public int MaxLimit => _maxLimit;
    private int _populationCap;

    public event System.Action OnPopulationUpated;

    public PlayerPopulation(int idle, int current, int maxLimit, int populationCap = int.MaxValue)
    {
        _idle = idle;
        _current = current;
        _maxLimit = maxLimit;
        _populationCap = populationCap;
    }

    public void AddIdlePopulation(int amount)
    {
        _idle += amount;
        _idle = Mathf.Clamp(_idle, 0, _current);

        OnPopulationUpated?.Invoke();
    }
    public void AddCurrentPopulation(int amount)
    {
        _idle += amount;
        _current += amount;
        _current = Mathf.Clamp(_current, 0, _maxLimit);

        OnPopulationUpated?.Invoke();

    }
    public void AddMaxPopulationLimit(int amount)
    {
        _maxLimit += amount;
        _maxLimit = Mathf.Clamp(_maxLimit, 0, _populationCap);

        OnPopulationUpated?.Invoke();
    }
    public void AddPopulation(int idleAmount, int currentAmount, int maxLimitAmount)
    {
        _idle += idleAmount;
        _current += currentAmount;
        _maxLimit += maxLimitAmount;

        _idle = Mathf.Clamp(_idle, 0, _current);
        _current = Mathf.Clamp(_current, 0, _maxLimit);
        _maxLimit = Mathf.Clamp(_maxLimit, 0, _populationCap);

        OnPopulationUpated?.Invoke();
    }
}
