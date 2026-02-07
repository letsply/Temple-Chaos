using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Continue()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().Save();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void SaveAndQuit()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().Save();
        Application.Quit();
        EditorApplication.isPlaying = false;
    }

    public void Close(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Time.timeScale = 1f;
        }
    }
}
