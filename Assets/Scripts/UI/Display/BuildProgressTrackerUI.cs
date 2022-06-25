using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
/// <summary>
/// Behaviour for mapping the build progress to the worldspace UI attached to the building unit.
/// </summary>
public class BuildProgressTrackerUI : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField]
    private GameObject _progressBar;
    [SerializeField]
    private Image _imgProgressBar;
    //using vector 3 to store the current value of the progress bar(x),
    //the minimum value(y), and the max value(z).
    private BuildModeBlueprintBehaviour _progressData;

    private void Awake()
    {
        _progressData = this.GetComponentInParent<BuildModeBlueprintBehaviour>();
    }
    private void OnEnable()
    {
        _progressBar.SetActive(false);
        _progressData.OnValueChanged += MapToUI;
    }
    private void OnDisable()
    {
        _progressData.OnValueChanged -= MapToUI;
    }
    private void MapToUI(Vector3 input)
    {
        float current = input.x;
        float min = input.y;
        float max = input.z;
        current = Mathf.Clamp(current, min, max);
        float result = MathUtilities.GetPercentage(current, min, max);
        //Turn on progress bar if there is progress.
        if (!_progressBar.activeInHierarchy && result > 0f)
        {
            _progressBar.SetActive(true);
        }
        _imgProgressBar.fillAmount = result;

        //Turn off progress bar when it has reached completion.
        if (current >= max)
        {
            _progressData.OnValueChanged -= MapToUI;
            _progressBar.SetActive(false);
        }
    }
}
