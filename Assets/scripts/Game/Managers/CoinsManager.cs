using System;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager instance;
    public int Coins { get; private set;}
    public event Action<int> OnCoinsChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddCoins(int coins)
    {
        Coins += coins;
        OnCoinsChanged?.Invoke(Coins);
    }
    public bool TrySpendCoins(int RequiredAmount)
    {
        if (RequiredAmount <= Coins)
        {
            return true;
        }
        else 
        { 
            return false; 
        }
    }
    public void SaveCoins()
    {
        int currentSavedCoins = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerCoins);
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerCoins, Coins+currentSavedCoins);
    }
}
