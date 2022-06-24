using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Building", menuName = "Scriptable/Building")]
public class BuildingProperties : ScriptableObject
{
    [SerializeField]
    private string _name;
    [SerializeField]
    [TextArea(10, 10)]
    private string _description;
    [SerializeField]
    [Tooltip("Idle Population, Money, Build Materials")]
    private Vector3 _costToBuild;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private float _buildTime;
    [SerializeField]
    private Sprite _profilePicture;
    [SerializeField]
    private BuildingEffect[] _buildingEffects;
    [SerializeField]
    private BuildingActions _availableActions;
    public string Name => _name;
    public string Description => _description;
    public Vector3 CostToBuild => _costToBuild;
    public int MaxHealth => _maxHealth;
    public float BuildTime => _buildTime;
    public Sprite ProfilePicture => _profilePicture;
    public BuildingEffect[] BuildingEffects => _buildingEffects;
    public bool CanBuild(Vector3 payment) => payment.x >= _costToBuild.x && payment.y >= _costToBuild.y && payment.z >= _costToBuild.z;
    public BuildingActions AvailableActions => _availableActions;

    [System.Flags]
    public enum BuildingActions
    {
        None = 0,
        SendWorker = 1,
        RetrieveWorker = 1 << 1,
        DestroyBuilding = 1 << 2,
        RepairBuilding = 1 << 3,
        IncreasePopulation = 1 << 4
    }
}
