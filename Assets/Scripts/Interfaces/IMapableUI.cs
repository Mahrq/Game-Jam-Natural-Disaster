using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapableUI<T>
{
    delegate void MapToUIDelegate(T mapData);
    event MapToUIDelegate OnValueChanged;
}
