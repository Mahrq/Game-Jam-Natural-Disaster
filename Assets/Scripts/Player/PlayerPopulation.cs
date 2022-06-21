using System.Collections;
using System.Collections.Generic;
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

    public PlayerPopulation(int idle, int current, int maxLimit)
    {
        _idle = idle;
        _current = current;
        _maxLimit = maxLimit;
    }

    public void AddIdlePopulation(int amount)
    {
        _idle += amount;
    }
    public void AddCurrentPopulation(int amount)
    {
        _idle += amount;
        _current += amount;
    }
    public void AddMaxPopulationLimit(int amount)
    {
        _maxLimit += amount;
    }
    public void AddPopulation(int idleAmount, int currentAmount, int maxLimitAmount)
    {
        _idle += idleAmount;
        _current += currentAmount;
        _maxLimit += maxLimitAmount;
    }
}
