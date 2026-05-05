using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MyButtonClickController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent _actionList;
    public void OnPointerClick(PointerEventData eventData)
    {
        _actionList?.Invoke();
        Debug.Log("Action list was invoked");
    }
}
