using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CreateBuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Assign")]
    [SerializeField]
    protected BuildingProperties _buildingProperties;
    [SerializeField]
    protected GameObject _buildingDetailsToolTip;
    [SerializeField]
    private PrefabHolder.Item _toSpawn = PrefabHolder.Item.PlayerHouse;
    [SerializeField]
    protected Text _txtName;
    [SerializeField]
    protected Text _txtDescription;
    [SerializeField]
    protected Text _txtCostPopulation;
    [SerializeField]
    protected Text _txtCostMoney;
    [SerializeField]
    protected Text _txtCostBuildingMaterial;
    [SerializeField]
    protected float _toolTipOffsetX = 0f;
    [SerializeField]
    protected float _toolTipOffsetY = 150f;
    protected Vector2 offsetVector;

    protected RectTransform _rTransform;
    protected RectTransform _toolTipTransfrom;

    public static event System.Action<PrefabHolder.Item> OnCreateBuildingButtonClicked;
    protected virtual void Awake()
    {
        _rTransform = this.GetComponent<RectTransform>();
        _toolTipTransfrom = _buildingDetailsToolTip.GetComponent<RectTransform>();
        offsetVector = new Vector2(_toolTipOffsetX, _toolTipOffsetY);
    }

    protected virtual void OnEnable()
    {
        if (_buildingDetailsToolTip.activeInHierarchy)
        {
            _buildingDetailsToolTip.SetActive(false);
        }
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!_buildingDetailsToolTip.activeInHierarchy)
        {
            MapDetailsToUI();
            _toolTipTransfrom.SetParent(_rTransform);
            _toolTipTransfrom.anchoredPosition = Vector2.zero + offsetVector;
            _buildingDetailsToolTip.SetActive(true);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (_buildingDetailsToolTip.activeInHierarchy)
        {
            _buildingDetailsToolTip.SetActive(false);
        }
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnCreateBuildingButtonClicked?.Invoke(_toSpawn);
    }
    protected virtual void MapDetailsToUI()
    {
        Vector3 cost = _buildingProperties.CostToBuild;
        _txtName.text = _buildingProperties.Name;
        _txtDescription.text = _buildingProperties.Description;
        _txtCostPopulation.text = $"{cost.x}";
        _txtCostMoney.text = $"{cost.y}";
        _txtCostBuildingMaterial.text = $"{cost.z}";
    }
}
