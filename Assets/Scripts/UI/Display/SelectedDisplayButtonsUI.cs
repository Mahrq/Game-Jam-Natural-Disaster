using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Shows the given actions the selected building can take onto the UI.
/// </summary>
public class SelectedDisplayButtonsUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Order the button game objects according to the 'BuildingActions' enum" +
        "\nA game object button must exist for each enum other than None")]
    private GameObject[] _actionButtons;
    private List<BuildingActionButton.ActionTimerData> _preservedTimers = new List<BuildingActionButton.ActionTimerData>();
    public List<BuildingActionButton.ActionTimerData> PreservedTimers => _preservedTimers;
    private void Update()
    {
        if (_preservedTimers.Count > 0)
        {
            for (int i = 0; i < _preservedTimers.Count; i++)
            {
                _preservedTimers[i].CurrentTimer += Time.deltaTime;
                if (_preservedTimers[i].CurrentTimer >= _preservedTimers[i].TimerLimit)
                {
                    _preservedTimers[i].InvokeAction();
                }
            }
            _preservedTimers.RemoveAll((timers) => timers.HasExecuted);
        }
    }
    public void ToggleButtons(BuildingActions actions, SelectableObjectHandler sender)
    {
        //Building actions is a bitmask enum so get the binary representation of it
        //then reverse the result so it can index the button arrays to toggle them
        string formattedBinary = GetBinaryForToggle(actions);
        char binaryBit;
        BuildingActionButton babs;
        BuildingActionButton.ActionTimerData replacement = null;
        int senderId = sender.Blueprint.ID;
        //Compare sender id to preserved id.
        if (_preservedTimers.Count > 0)
        {
            for (int j = 0; j < _preservedTimers.Count; j++)
            {
                //Get replacement if match
                //This means that the same building was chosen
                if (_preservedTimers[j].UID == senderId)
                {
                    replacement = _preservedTimers[j];
                    _preservedTimers.RemoveAt(j);
                }
            }
        }
        //Loop through buttons
        for (int i = 0; i < _actionButtons.Length; i++)
        {
            //Format binary for toggle
            binaryBit = formattedBinary[i];
            //Toggle button on
            if (binaryBit == '1')
            {
                _actionButtons[i].SetActive(true);
                //Get button data
                if (_actionButtons[i].TryGetComponent<BuildingActionButton>(out babs))
                {
                    //Compare button id to sender
                    //the button the sender and the preserved data should all match
                    //meaning that the same building was chosen, then de-selected, then chosen again.
                    if (babs.CurrentTimerData != null)
                    {
                        if (babs.CurrentTimerData.UID == senderId && replacement != null)
                        {
                            babs.CurrentTimerData = replacement;
                        }
                    }
                }
            }
            else
            {
                _actionButtons[i].SetActive(false);
            }
        }
    }
    public void ClearButtons()
    {
        if (_actionButtons.Length > 0)
        {
            for (int i = 0; i < _actionButtons.Length; i++)
            {
                if (_actionButtons[i].activeInHierarchy)
                {
                    _actionButtons[i].SetActive(false);
                }
            }
        }
    }
    private string GetBinaryForToggle(BuildingActions actions)
    {
        //Get binary representation of the enum
        string actionsToBinary = Convert.ToString((int)actions, 2);
        //reverse the formatted binary so that it can match the index of the buttons array.
        char[] formattedBinary = actionsToBinary.ToCharArray();
        Array.Reverse(formattedBinary);
        string result = new string(formattedBinary);
        //correct the result by adding the extra 0s that were ignored from the binary formatting
        //for flags deeper in the enum that were 0;
        int buttonVariety = _actionButtons.Length;
        for (int i = result.Length; i < buttonVariety; i++)
        {
            result += "0";
        }

        return result;
    }
    public void AddToPreservedTimers(BuildingActionButton.ActionTimerData data)
    {
        BuildingActionButton.ActionTimerData replacement = data;
        //Check if this timer already exists in the list.
        if (_preservedTimers.Count > 0)
        {
            _preservedTimers.RemoveAll(item => item.UID == data.UID);
        }
        _preservedTimers.Add(replacement);
    }
}
