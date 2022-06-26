using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerData : MonoBehaviour, IMapableUI<PlayerData>
{
    [SerializeField]
    [Range(1, 1000)]
    private int _startingPopulation = 20;
    [SerializeField]
    private PlayerPopulation _population;
    [SerializeField]
    private int _money = 200;
    public int Money => _money;
    [SerializeField]
    private int _buildMaterials = 200;
    public int BuildMaterials => _buildMaterials;
    [SerializeField]
    private int _food = 200;
    public int Food => _food;
    [SerializeField]
    [Range(100, 100000)]
    private int _resourceCap = 99999;

    public event IMapableUI<PlayerData>.MapToUIDelegate OnValueChanged;

    private void Awake()
    {
        _population = new PlayerPopulation(_startingPopulation, _startingPopulation, _startingPopulation, _resourceCap);

        SceneManager.sceneLoaded += OnSceneLoadedCallback;
        HungerTax.OnFoodTaxCalculated += HandleFoodRequired;
        Population.OnPopulationUpated += OnPopulationUpdatedCallbaclk;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedCallback;
        HungerTax.OnFoodTaxCalculated -= HandleFoodRequired;
        Population.OnPopulationUpated -= OnPopulationUpdatedCallbaclk;
    }

    public void AddFood(int amount)
    {
        _food += amount;
        _food = Mathf.Clamp(_food, 0, _resourceCap);

        OnValueChanged?.Invoke(this);
    }

    public void AddPayment(Vector3 amount)
    {
        _population.AddIdlePopulation((int)amount.x);
        _money += (int)amount.y;
        _buildMaterials += (int)amount.z;

        _money = Mathf.Clamp(_money, 0, _resourceCap);
        _buildMaterials = Mathf.Clamp(_buildMaterials, 0, _resourceCap);

        OnValueChanged?.Invoke(this);
    }

    public PlayerPopulation Population
    {
        get
        {
            if (_population == null)
            {
                _population = new PlayerPopulation(_startingPopulation, _startingPopulation, _startingPopulation, _resourceCap);
            }
            return _population;
        }
    }
    public Vector3 GetPayment
    {
        get
        {
            return new Vector3(Population.Idle, Money, BuildMaterials);
        }
    }

    private void OnSceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Test_Scene")
        {
            //Send event when the game scene is loaded so that the UI
            //is updated at the start of the game.
            OnValueChanged?.Invoke(this);
        }
    }
    private void OnPopulationUpdatedCallbaclk()
    {
        OnValueChanged?.Invoke(this);
    }
    private void HandleFoodRequired(int foodRequired, int foodRequiredPerDay)
    {
        //Consequence for not having enough food kills some of the population
        if (foodRequired > Food)
        {
            int shortageAmount = foodRequired - Food;
            int amountStarved = shortageAmount / foodRequiredPerDay;
            //ensures atleast 1 is killed if the result is 0, or if any remainders, it counts 1 up.
            amountStarved++;
            Population.AddCurrentPopulation(-amountStarved);
            _food %= foodRequiredPerDay;
            AddFood(0);
        }
        else
        {
            AddFood(-foodRequired);
        }
    }
}
