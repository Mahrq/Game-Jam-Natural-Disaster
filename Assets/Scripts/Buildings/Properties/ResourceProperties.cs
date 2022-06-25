using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Resource Building", menuName = "Scriptable/Resource Building")]
public class ResourceProperties : BuildingProperties
{
    [SerializeField]
    private int _resourceAmount;
    [SerializeField]
    [Tooltip("Resource Amount doesn't matter if true")]
    private bool _isInfinite;
    [SerializeField]
    [Tooltip("The maximum amount of workers that can be assigned to this resource building")]
    private int _workerCap;
    [SerializeField]
    private BuildingEffect[] _lockedEffects;
    [SerializeField]
    [Tooltip("The number of workers required to unlock the ordered effects of the locked effects")]
    private int _workersRequiredPerLockedEffect;

    public int ResourceAmount => _resourceAmount;
    public bool IsInfinite => _isInfinite;
    public int WorkerCap => _workerCap;
    public BuildingEffect[] LockedEffects => _lockedEffects;
    public int WorkersRequiredPerLockedEffect => _workersRequiredPerLockedEffect;
}
