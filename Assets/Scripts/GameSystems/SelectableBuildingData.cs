using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableBuildingData
{
    private BuildModeBlueprintBehaviour _blueprintBehaviour;
    private ResourceBuildingBehaviour _resourceBehaviour;
    public BuildModeBlueprintBehaviour BluePrintBehaviour => _blueprintBehaviour;
    public ResourceBuildingBehaviour ResourceBehaviour => _resourceBehaviour;
    public SelectableBuildingData(BuildModeBlueprintBehaviour blueprint, ResourceBuildingBehaviour resource)
    {
        _blueprintBehaviour = blueprint;
        _resourceBehaviour = resource;
    }
}