using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerChoiceData
{
    public int PlayerIndex;
    public Sprite PlayerSprite;
    public Sprite WeaponSprite;
    public string PlayerName;
}

[CreateAssetMenu(fileName = "NewPlayerChoicesSO", menuName = "Scriptable Objects/PlayerChoicesSO")]
public class PlayerChoicesSO : ScriptableObject
{
    [SerializeField] private PlayerChoiceData[] _playerChoicesData;
    public PlayerChoiceData[] PlayerChoicesData => _playerChoicesData;
}
