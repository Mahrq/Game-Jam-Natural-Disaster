using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// The game's scoring system.
/// Tracks how long the game has gone in game days with adjustable values.
/// </summary>
public class DayTracker : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField]
    private Text _txtCurrentDay;
    [SerializeField]
    private Text _txtCurrentTime;
    [SerializeField]
    [Range(0.01f, 60f)]
    [Tooltip("Seconds in real time that corresponds to a game minute.")]
    private float _gameMinutes = 5f;
    private float _gameMinuteCounter = 0f;
    private int _currentDay = 1;
    private int _hours = 0;
    private int _minutes = 0;
    private delegate void CountingMethod();

    private CountingMethod _thisCountingMethod;
    public static event System.Action OnNewDay;
    public static event System.Action OnNewHour;

    private void Start()
    {
        MapTimeToUI();
        if (_gameMinutes <= 0.1f)
        {
            _thisCountingMethod = CountGameMinuteFast;
        }
        else
        {
            _thisCountingMethod = CountGameMinuteSlow;
        }
    }
    private void Update()
    {
        _thisCountingMethod();
    }
    /// <summary>
    /// When the game minute is considerd slow, use this method to update the UI.
    /// </summary>
    private void CountGameMinuteSlow()
    {
        _gameMinuteCounter += Time.deltaTime;
        if (_gameMinuteCounter >= _gameMinutes)
        {
            _gameMinuteCounter = 0f;
            _minutes++;
            _minutes %= 60;         
            if (_minutes == 0)
            {
                _hours++;
                _hours %= 24;
                OnNewHour?.Invoke();
                if (_hours == 0)
                {
                    _currentDay++;
                    OnNewDay?.Invoke();
                }
            }
            MapTimeToUI();
        }
    }
    /// <summary>
    /// When the game minute is considered fast, use this method to reduce the number of times it
    /// has to update the UI.
    /// </summary>
    private void CountGameMinuteFast()
    {
        _gameMinuteCounter += Time.deltaTime;
        if (_gameMinuteCounter >= _gameMinutes)
        {
            _gameMinuteCounter = 0f;
            _minutes++;
            _minutes %= 60;
            //Updates the UI every 15 game minutes
            if (_minutes % 15 == 0)
            {
                if (_minutes == 0)
                {
                    _hours++;
                    _hours %= 24;
                    OnNewHour?.Invoke();
                    if (_hours == 0)
                    {
                        _currentDay++;
                        OnNewDay?.Invoke();
                    }
                }
                MapTimeToUI();
            }
        }
    }
    private void MapTimeToUI()
    {
        _txtCurrentDay.text = $"Day  {_currentDay}";
        _txtCurrentTime.text = ConvertToTimeFormat(_hours, _minutes);
    }
    private string ConvertToTimeFormat(int hours, int minutes)
    {
        string[] formattedValue = { $"{hours}", $"{minutes}" };
        for (int i = 0; i < formattedValue.Length; i++)
        {
            if (formattedValue[i].Length < 2)
            {
                formattedValue[i] = $"0{formattedValue[i]}";
            }
        }
        return $"{formattedValue[0]}:{formattedValue[1]}";
    }
}
