using UnityEngine;

public class PlayerChoicesFiller : MonoBehaviour
{
    [SerializeField] private GameObject _playerChoicePanelPrefab;
    [SerializeField] private PlayerChoicesSO _playerChoices;
    [SerializeField] private GameObject _PlayerChoicesContent;

    private void Start()
    {
        foreach (var playerChoiceData in _playerChoices.PlayerChoicesData)
        {
            GameObject playerChoice = Instantiate(_playerChoicePanelPrefab, _PlayerChoicesContent.transform);
            playerChoice.GetComponent<PlayerChoiceSetter>().SetChoice(playerChoiceData.WeaponSprite, playerChoiceData.PlayerSprite, playerChoiceData.PlayerIndex, playerChoiceData.PlayerName);
        }
    }
}