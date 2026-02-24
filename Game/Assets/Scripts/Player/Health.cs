using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(0,10)] private int hearts = 3;
    public int Hearts() => hearts;

    [SerializeField] GameObject[] uiHearts;
    [SerializeField] GameObject deathUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < uiHearts.Length; i++)
        {
            if (hearts < i + 1)
            {
                uiHearts[i].SetActive(false);
            }
            else
            {
                uiHearts[i].SetActive(true);
            }
        }

        if (hearts <= 0)
        {
            deathUI.SetActive(true);
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().SaveFile();
            Time.timeScale = 0f;
        }
    }
    public void TakeDamage(int amount)
    {
        hearts -= amount;
    }
}
