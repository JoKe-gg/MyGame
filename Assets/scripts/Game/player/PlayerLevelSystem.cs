using System;
using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour, IChangingBar
{
    [SerializeField] private int _exp = 400;
    [SerializeField] private BarController _expBarController;
    private PlayerCalculateUpgrades _playerCalculateUpgrades;
    private PlayerBehaviour _playerBehaviour;
    private int _currentLevel;
    private int _xpToNextLevel;
    private int _basicXpToNextLevel;
    public event Action<int> OnLevelUP;
    private void Start()
    {
        if (_expBarController == null)
        {
            _expBarController = GameObject.FindGameObjectWithTag("PlayerEXPBar").GetComponent<BarController>();
        }
        _currentLevel = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);
        _playerCalculateUpgrades = GetComponent<PlayerCalculateUpgrades>();
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _basicXpToNextLevel = _playerBehaviour.PlayerBasicStatsSO.PlayerBasicStatsData.XP;
        PurchaseEXP(0);
    }
    public void PurchaseEXP(int value)
    {
        _exp += value;
        UpdateXpTpNextLevel();

        if (_exp >= _xpToNextLevel)
        {
            LevelUP();
        }
    }
    public void ChangeBar() => _expBarController.ChangeBar(_exp, _xpToNextLevel);

    private void UpdateXpTpNextLevel()
    {
        _xpToNextLevel = Mathf.RoundToInt(_basicXpToNextLevel * Mathf.Pow(1.1f, _currentLevel));
        ChangeBar();
    }
    
    private void LevelUP()
    {
        while (_exp >= _xpToNextLevel) {
            _currentLevel++;

            _exp -= _xpToNextLevel;
            UpdateXpTpNextLevel();
            Debug.Log($"Exp is {_exp}/{_xpToNextLevel}.\n Current level: {_currentLevel}");
            PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, _currentLevel);
        }
        ChangeBar();
        OnLevelUP?.Invoke(_currentLevel);
        _playerCalculateUpgrades.ReCalculate();
    }
    public void ResetLevel()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerLevel, 0);
    }
}
