using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionsOfPanels : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private int? _sceneIndex = null;
    public void RestartArena()
    {
        if (_sceneIndex != null) return;
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
        PauseManager.instance.Reset();
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(_sceneIndex.Value);
    }

    public void GoToMainMenu()
    {
        if (_sceneIndex != null) return;
        _sceneIndex = 0;
        PauseManager.instance.Reset();
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(_sceneIndex.Value);
    }
    public void Resume()
    {
        PauseManager.instance.SetPause(false);
        if (panel != null)
        panel.SetActive(false);
    }
}
