using System;
using UnityEngine;
using System.Collections;

public class CoinsManagerMainMenu : MonoBehaviour, ISavable
{
    public static CoinsManagerMainMenu instance;
    public int Coins {get; private set;} = 0;
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
    private void OnEnable()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Register(this);
        }
        else
        {
            Debug.Log($"SAVE MANAGER IS NOT READY");
            StartCoroutine(WaitToRegister());
        }
    }
    private void OnDisable()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Unregister(this);
        }
    }
    private IEnumerator WaitToRegister()
    {
        yield return new WaitForEndOfFrame();
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Register(this);
        }
        else
        {
            Debug.Log($"SAVE MANAGER IS NOT READY IN COROUTINE");
        }
    }
    public void AddCoins(int value)
    {
        if (value > 0) { 
            Coins += value;
            OnCoinsChanged?.Invoke(Coins);
        }
    }
    public bool TrySpendCoins(int value)
    {
        return Coins >= value;
    }
    public void SpendCoins(int value)
    {
        if (TrySpendCoins(value))
        {
            Coins -= value;
            OnCoinsChanged?.Invoke(Coins);
        }
        else
        {
            Debug.Log("Not enough coins to purchase");
        }
    }
    public void Load(DataSave dataSave)
    {
        if (dataSave != null)
        {
            Coins = dataSave.Coins;
            OnCoinsChanged?.Invoke(Coins);
        }
    }
    public void Save(DataSave dataSave)
    {
        if (dataSave != null)
        {
            dataSave.SetCoins(Coins);
        }
    }
}
