using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] string[] deathTexts;
    [SerializeField] TextMeshProUGUI deathText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int r = Random.Range(0, deathTexts.Length);
        deathText.text = deathTexts[r];
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
