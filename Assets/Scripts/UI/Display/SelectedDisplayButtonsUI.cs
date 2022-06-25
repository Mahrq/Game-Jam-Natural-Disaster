using System;
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

    public void ToggleButtons(BuildingActions actions)
    {
        //Building actions is a bitmask enum so get the binary representation of it
        //then reverse the result so it can index the button arrays to toggle them
        string formattedBinary = GetBinaryForToggle(actions);
        char binaryBit;
        for (int i = 0; i < _actionButtons.Length; i++)
        {
            binaryBit = formattedBinary[i];
            if (binaryBit == '1')
            {
                _actionButtons[i].SetActive(true);
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
}
