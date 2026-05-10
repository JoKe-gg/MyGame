using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }
    private HashSet<Savable> _savables = new();
    public event Action OnGameSaved;
    string _path;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            _path = Application.persistentDataPath + "/save.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(WaitToLoad());
    }
    private IEnumerator WaitToLoad()
    {
        yield return new WaitForEndOfFrame();
        LoadGame();
    }
    public void Register(Savable savable)
    {
        if (!_savables.Contains(savable)){
            _savables.Add(savable);
        }
        StartCoroutine(WaitToLoad());
    }
    public void Unregister(Savable savable)
    {
        if (_savables.Contains(savable))
        {
            _savables.Remove(savable);
        }
    }
    public void SaveGame()
    {
        DataSave saveData = new DataSave();

        if (File.Exists(_path))
        {
            string savedJson = File.ReadAllText(_path);
            saveData = JsonUtility.FromJson<DataSave>(savedJson);
        }

        foreach (var savable in _savables)
        {
            savable.Save(saveData);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_path, json); 
        int count = OnGameSaved?.GetInvocationList().Length ?? 0;
        Debug.Log($"The game is saved : \n {json} \n AmountOfRegistered {count}");
        OnGameSaved?.Invoke();
    }
    public void LoadGame()
    {
        DataSave saveData = new DataSave();
        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            Debug.Log($"The game is being loaded : \n {json}");

            saveData = JsonUtility.FromJson<DataSave>(json);
        }
        foreach (var savable in _savables)
        {
            savable.Load(saveData);
        }
    }
}
