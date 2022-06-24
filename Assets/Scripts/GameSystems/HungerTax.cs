using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Food is consumed every game day.
/// Consequences for the player if they don't have enough food for their current population.
/// </summary>
public class HungerTax : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Required amount of food per person per day")]
    [Range(1, 100)]
    private int _foodPerDay = 5;
    private PlayerData _player;

    public static event System.Action<int, int> OnFoodTaxCalculated;
    private void Awake()
    {
        _player = FindObjectOfType<PlayerData>();
    }
    private void OnEnable()
    {
        DayTracker.OnNewDay += CalculateRequiredFood;
    }
    private void OnDisable()
    {
        DayTracker.OnNewDay -= CalculateRequiredFood;
    }
    private void CalculateRequiredFood()
    {
        int currentPopulation = _player.Population.Current;
        int foodRequired = currentPopulation * _foodPerDay;
        OnFoodTaxCalculated?.Invoke(foodRequired, _foodPerDay);
    }
}
