using UnityEngine;
[System.Serializable]
public class MapChoiceData
{
    public int MapIndex;
    public Sprite MapSprite;
    public string MapName;
}
[CreateAssetMenu(fileName = "MapChoicesSO", menuName = "Scriptable Objects/MapChoicesSO")]
public class MapChoicesSO : ScriptableObject
{
    [SerializeField] private MapChoiceData[] _mapChoicesData;

    private void OnValidate()
    {
        foreach(var mapChoiceData in _mapChoicesData)
        {
            if(mapChoiceData.MapIndex <= 0)
            {
                mapChoiceData.MapIndex = 1;
            }
        }
    }
    public MapChoiceData[] MapChoicesData => _mapChoicesData;
}
