using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(0,10)] private int Hearts = 3;
    [SerializeField] GameObject[] uiHearts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < uiHearts.Length; i++)
        {
            if (Hearts < i + 1)
            {
                uiHearts[i].SetActive(false);
            }
            else
            {
                uiHearts[i].SetActive(true);
            }
        }
    }
    public void TakeDamage(int amount)
    {
        Hearts -= amount;
    }
}
