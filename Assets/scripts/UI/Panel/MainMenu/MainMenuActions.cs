using UnityEngine;
using UnityEngine.Device;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private GameObject _playerSelectPanel;
    [SerializeField] private GameObject _mapSelectPanel;
    public void StartArena(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }
    public void OpenPanel(GameObject panel)
    {
        if (panel != null) 
        panel.SetActive(true);
    }
    public void ClosePlayerSelectPanel()
    {
        _playerSelectPanel.SetActive(false);
    }
    public void OpenPlayerSelectPanel()
    {
        _playerSelectPanel.SetActive(true);
    }
    public void OpenMapSelectPanel()
    {
        _mapSelectPanel.SetActive(true);
    }
    public void CloseMapSelectPanel()
    {
        _mapSelectPanel.SetActive(false);
    }
    public void ClosePanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(false);
    }
    public void ExitFromGame()
    {
        if(SaveManager.instance != null)
        {
            SaveManager.instance.SaveGame();
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif 
    }
}
