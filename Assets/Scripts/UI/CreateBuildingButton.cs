using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CreateBuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Assign")]
    [SerializeField]
    private BuildingProperties _buildingProperties;
    [SerializeField]
    private GameObject _buildingDetailsToolTip;
    [SerializeField]
    private PrefabHolder.Item _toSpawn = PrefabHolder.Item.PlayerHouse;
    [SerializeField]
    private Text _txtName;
    [SerializeField]
    private Text _txtDescription;
    [SerializeField]
    private Text _txtCostPopulation;
    [SerializeField]
    private Text _txtCostMoney;
    [SerializeField]
    private Text _txtCostBuildingMaterial;
    [SerializeField]
    private float _toolTipOffsetX = 0f;
    [SerializeField]
    private float _toolTipOffsetY = 150f;
    private Vector2 offsetVector;

    private RectTransform _rTransform;
    private RectTransform _toolTipTransfrom;

    public static event System.Action<PrefabHolder.Item> OnCreateBuildingButtonClicked;
    private void Awake()
    {
        _rTransform = this.GetComponent<RectTransform>();
        _toolTipTransfrom = _buildingDetailsToolTip.GetComponent<RectTransform>();
        offsetVector = new Vector2(_toolTipOffsetX, _toolTipOffsetY);
    }

    private void OnEnable()
    {
        if (_buildingDetailsToolTip.activeInHierarchy)
        {
            _buildingDetailsToolTip.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_buildingDetailsToolTip.activeInHierarchy)
        {
            MapDetailsToUI();
            _toolTipTransfrom.parent = _rTransform;
            _toolTipTransfrom.anchoredPosition = Vector2.zero + offsetVector;
            _buildingDetailsToolTip.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_buildingDetailsToolTip.activeInHierarchy)
        {
            _buildingDetailsToolTip.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnCreateBuildingButtonClicked?.Invoke(_toSpawn);
    }
    private void MapDetailsToUI()
    {
        Vector3 cost = _buildingProperties.CostToBuild;
        _txtName.text = _buildingProperties.Name;
        _txtDescription.text = _buildingProperties.Description;
        _txtCostPopulation.text = $"{cost.x}";
        _txtCostMoney.text = $"{cost.y}";
        _txtCostBuildingMaterial.text = $"{cost.z}";
    }


}
