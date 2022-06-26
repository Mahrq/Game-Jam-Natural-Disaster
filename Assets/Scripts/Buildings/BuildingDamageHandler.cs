using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the health and damage modifications on building objects.
/// </summary>
public class BuildingDamageHandler : MonoBehaviour, IDamageable, IMapableUI<Vector3>
{
    private BuildModeBlueprintBehaviour _blueprint;
    private BuildingProperties _properties;
    private int _currentHealth;
    public event IMapableUI<Vector3>.MapToUIDelegate OnValueChanged;
    public event Action OnDeath;

    private void Awake()
    {
        _blueprint = this.GetComponent<BuildModeBlueprintBehaviour>();
        _properties = _blueprint.BuildProperties;
    }
    private void OnEnable()
    {
        _blueprint.OnInitialise += OnInitialiseCallback;
    }
    private void OnDisable()
    {
        _blueprint.OnInitialise -= OnInitialiseCallback;
    }
    //Test Damage
    //private void Update()
    //{
    //    if (Time.frameCount % 60 == 0)
    //    {
    //        ModifyHealth(-5);
    //        Debug.Log("Health Subtracted");
    //    }
    //}
    private void OnInitialiseCallback()
    {
        _currentHealth = _properties.MaxHealth;
        ModifyHealth(0);
    }
    public void ModifyHealth(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _properties.MaxHealth);
        if (OnValueChanged != null)
        {
            Vector3 healthBarMapData = new Vector3(_currentHealth, 0, _properties.MaxHealth);
            OnValueChanged(healthBarMapData);
        }
        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public int CurrentHealth => _currentHealth;
    public bool CanRepair() => _currentHealth != _properties.MaxHealth;
}
