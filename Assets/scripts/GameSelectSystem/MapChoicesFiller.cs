using System;
using UnityEngine;

public class MapChoicesFiller : MonoBehaviour
{
    [SerializeField] private GameObject _mapChoicePanelPrefab;
    [SerializeField] private MapChoicesSO _mapChoicesSO;
    [SerializeField] private GameObject _mapChoicesContent;

    private void Start()
    {
        foreach (var mapChoice in _mapChoicesSO.MapChoicesData)
        {
            GameObject playerChoice = Instantiate(_mapChoicePanelPrefab, _mapChoicesContent.transform);
            playerChoice.GetComponent<MapChoiceSetter>().SetChoice(mapChoice.MapSprite, mapChoice.MapIndex, mapChoice.MapName);
        }
    }
}
