using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void ButtonExit()
    {
        Application.Quit();
    }

    public void ButtonStart()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
