using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Effect", menuName = "Scriptable/Building Effects")]
public class BuildingEffect : ScriptableObject
{
    [SerializeField]
    private PlayerEconomy _effect;
    [SerializeField]
    private int _value;
    [SerializeField]
    private bool _recursValue;
    public bool RecursValue => _recursValue;
    [SerializeField]
    [Tooltip("Time in seconds when the effect is applied again")]
    [Range(1f, 60f)]
    private float _recurisonInterval;
    public float RecurisonInterval => _recurisonInterval;
    public PlayerEconomy Effect => _effect;
    public int Value => _value;

    public virtual void ApplyEffect(ref PlayerData playerData)
    {
        switch (Effect)
        {
            case PlayerEconomy.CurrentPopulation:
                playerData.Population.AddCurrentPopulation(Value);
                break;
            case PlayerEconomy.MaxPopulation:
                playerData.Population.AddMaxPopulationLimit(Value);
                break;
            case PlayerEconomy.Money:
                playerData.AddPayment(new Vector3(0, Value, 0));
                break;
            case PlayerEconomy.BuildMaterial:
                playerData.AddPayment(new Vector3(0, 0, Value));
                break;
            case PlayerEconomy.Food:
                playerData.AddFood(Value);
                break;
            default:
                break;
        }
    }
    public virtual void RemoveEffect(ref PlayerData playerData)
    {
        switch (Effect)
        {
            case PlayerEconomy.CurrentPopulation:
                playerData.Population.AddCurrentPopulation(-Value);
                break;
            case PlayerEconomy.MaxPopulation:
                playerData.Population.AddMaxPopulationLimit(-Value);
                break;
            case PlayerEconomy.Money:
                playerData.AddPayment(new Vector3(0, -Value, 0));
                break;
            case PlayerEconomy.BuildMaterial:
                playerData.AddPayment(new Vector3(0, 0, -Value));
                break;
            case PlayerEconomy.Food:
                playerData.AddFood(-Value);
                break;
            default:
                break;
        }
    }
}
