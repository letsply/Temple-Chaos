using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject invUI;
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;
        invUI.SetActive(false);
        gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        gameManager.SaveFile();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void SaveAndQuit()
    {
        gameManager.SaveFile();
        Application.Quit();
    }

    public void Close(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Time.timeScale = 1f;
        }
    }
}
