using Unity.Multiplayer.PlayMode;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonActionForChoice : MonoBehaviour
{
    private SelectableUpgradeSO _selectableUpgradeSO;
    private SetSelects _setSelects;
    private SelectableUpgradeManager _selectableUpgradeManager;
    public void AddSelectable()
    {
        if (_selectableUpgradeSO == null)
        {
            Debug.LogError($"Null reference to {nameof(_selectableUpgradeSO)} in the script {nameof(ButtonActionForChoice)}");
            return;
        }
        GameObject player = PlayerSpawnManager.CurrentPlayer;
        if (_selectableUpgradeSO.ReadEffect)
            player.GetComponent<PlayerStatuses>().AddNewEffect(_selectableUpgradeSO.EffectData);
        if (_selectableUpgradeSO.ReadUpgrade) { }
            player.GetComponent<PlayerStatuses>().AddNewUpgrade(_selectableUpgradeSO.Upgrade);
        _setSelects.CloseSelectPanel();
        _selectableUpgradeManager.RemoveChoice(_selectableUpgradeSO);
    }
    public void SetSelectableUpgrade(SelectableUpgradeSO selectableUpgradeSO, SetSelects setSelect, SelectableUpgradeManager selectableUpgradeManager)
    {
        _setSelects = setSelect;
        _selectableUpgradeSO = selectableUpgradeSO;
        _selectableUpgradeManager = selectableUpgradeManager;
    }
}
