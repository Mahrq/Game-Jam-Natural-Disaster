using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// For handling cursor events.
/// </summary>
public interface ISelectableObject<T>
{
    event System.Action<T> OnCursorEnter;
    event System.Action<T> OnCursorExit;
    event System.Action<T> OnMouseLeftClick;
}
