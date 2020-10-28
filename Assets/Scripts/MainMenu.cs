using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartSoloGame()
    {
        GameManager.IsMultiplayerSet = false;
        SceneManager.LoadScene("Game");
    }

    public void StartCoopGame()
    {
        GameManager.IsMultiplayerSet = true;
        SceneManager.LoadScene("Game");
    }
}
