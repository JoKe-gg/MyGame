using UnityEngine;

public interface ISavable
{
    public void Load(DataSave saveData);
    public void Save(DataSave saveData);
}
