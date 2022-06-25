using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuildingActionButton : CreateBuildingButton
{
    [SerializeField]
    private ActionButtonProperties _actionProperties;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!_buildingDetailsToolTip.activeInHierarchy)
        {
            MapDetailsToUI();
            _toolTipTransfrom.SetParent(_rTransform);
            _toolTipTransfrom.anchoredPosition = Vector2.zero + offsetVector;
            _buildingDetailsToolTip.SetActive(true);
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {

    }
    protected override void MapDetailsToUI()
    {
        Vector3 cost = _actionProperties.CostToActivate;
        _txtName.text = _actionProperties.Name;
        _txtDescription.text = _actionProperties.Description;
        _txtCostPopulation.text = $"{cost.x}";
        _txtCostMoney.text = $"{cost.y}";
        _txtCostBuildingMaterial.text = $"{cost.z}";
    }
}
