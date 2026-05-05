using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionsOfPanels : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public void RestartArena()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PauseManager.instance.Reset();
        CoinsManager.instance.SaveCoins();
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void GoToMainMenu()
    {
        PauseManager.instance.Reset();
        CoinsManager.instance.SaveCoins();
        SceneManager.LoadScene(0);
    }
    public void Resume()
    {
        PauseManager.instance.SetPause(false);
        if (panel != null)
        panel.SetActive(false);
    }
}
