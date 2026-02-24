using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] string[] deathTexts;
    [SerializeField] TextMeshProUGUI deathText;
    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int r = Random.Range(0, deathTexts.Length);
        deathText.text = deathTexts[r];
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Restart()
    {
        gameManager.Load(SceneManager.GetActiveScene().buildIndex);
    }
    public void Menu()
    {
        gameManager.SaveFile();
        SceneManager.LoadScene(0);
    }
}
