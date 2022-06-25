using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Action Button", menuName = "Scriptable/Action Button")]
public class ActionButtonProperties : ScriptableObject
{
    [SerializeField]
    private string _name;
    [SerializeField]
    [TextArea(10,10)]
    private string _description;
    [SerializeField]
    [Tooltip("Idle Population(x), Money(y), Building Materials(z)")]
    private Vector3 _costToActivate;
    [SerializeField]
    [Tooltip("Time in seconds until the effect is activated.")]
    [Range(0f, 600f)]
    private float _activationDelay;

    public string Name => _name;
    public string Description => _description;
    public Vector3 CostToActivate => _costToActivate;
    public float ActivationDelay => _activationDelay;
}
