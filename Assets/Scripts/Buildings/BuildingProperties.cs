using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Building", menuName = "Scriptable/Building")]
public class BuildingProperties : ScriptableObject
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private string _description;
    [SerializeField]
    [Tooltip("Idle Population, Money, Build Materials")]
    private Vector3 _costToBuild;
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private float _buildTime;
    [SerializeField]
    private BuildingEffect[] _buildingEffects;

    public string Name => _name;
    public string Description => _description;
    public Vector3 CostToBuild => _costToBuild;
    public int MaxHealth => _maxHealth;
    public float BuildTime => _buildTime;
    public BuildingEffect[] BuildingEffects => _buildingEffects;
    public bool CanBuild(Vector3 payment) => payment.x >= _costToBuild.x && payment.y >= _costToBuild.y && payment.z >= _costToBuild.z;

}
