using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    [Range(1, 1000)]
    private int _startingPopulation = 20;
    [SerializeField]
    private PlayerPopulation _population;
    [SerializeField]
    private int _money = 200;
    public int Money => _money;
    [SerializeField]
    private int _buildMaterials = 200;
    public int BuildMaterials => _buildMaterials;
    [SerializeField]
    private int _food = 200;
    public int Food => _food;

    private void Awake()
    {
        _population = new PlayerPopulation(_startingPopulation, _startingPopulation, _startingPopulation);
    }

    public void AddFood(int amount)
    {
        _food += amount;
    }

    public void AddPayment(Vector3 amount)
    {
        _population.AddIdlePopulation((int)amount.x);
        _money += (int)amount.y;
        _buildMaterials += (int)amount.z;
    }

    public PlayerPopulation Population
    {
        get
        {
            if (_population == null)
            {
                _population = new PlayerPopulation(_startingPopulation, _startingPopulation, _startingPopulation);
            }
            return _population;
        }
    }
    public Vector3 GetPayment
    {
        get
        {
            return new Vector3(Population.Idle, Money, BuildMaterials);
        }
    }
}
