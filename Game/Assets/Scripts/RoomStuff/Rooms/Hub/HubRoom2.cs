using UnityEngine;

public class HubRoom2 : DefaultRoom
{
    GameObject Player;
    bool jumpBoostInInv;
    [SerializeField] GameObject jumpPotion;
    [SerializeField] Transform spawnPos;
    void Start()
    {
        roomName = "Hub2";
        roomLenght = 43;
    }

    void Update()
    {
        if(Player != null)
        {
            jumpBoostInInv = false;
            for (int i = 0; i < Player.GetComponent<Inventory>().Items().Length; i++)
            {
                if (Player.GetComponent<Inventory>().Items()[i] == 2)
                {
                    jumpBoostInInv = true;
                }
            }
            if (jumpBoostInInv == false && spawnPos.childCount == 0)
            {
                Instantiate(jumpPotion, spawnPos);
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player = collision.gameObject;
        }
    }
}
