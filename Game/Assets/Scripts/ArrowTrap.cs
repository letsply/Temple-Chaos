using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class ArrowTrap : MonoBehaviour
{
    List<GameObject> traps = new List<GameObject>();
    [SerializeField] GameObject arrow;
    [SerializeField] bool allwaysActive;
    [SerializeField] float Speed;
    bool spawnArrows;
    Coroutine coroutine = null;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            traps.Add(transform.GetChild(i).gameObject);
        }
        if (allwaysActive) spawnArrows = true; StartCoroutine(Spawn(2, 3)); ; 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && coroutine == null && allwaysActive == false)
        {
            coroutine = StartCoroutine(Spawn(2,3));
            spawnArrows = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && allwaysActive == false)
        {
            spawnArrows = false;
        }
    }
    //spawns Frequency amount of Arrows/Sec
    IEnumerator Spawn(float frequency,int amount)
    {
        // Foreach trap spawn a amount of arrows shifting with each arrow to the right
        foreach (GameObject trap in traps)
        {
            //Set the pos from the trap Note: the texture has to be seperate 
            Vector3 arrowPos = trap.transform.position - new Vector3(arrow.transform.localScale.x + 0.1f,0);
            for (int i = 0; i < amount; i++)
            {
                if (trap.transform.eulerAngles.z == 90 || trap.transform.eulerAngles.z == -90)
                    arrowPos += new Vector3(0, arrow.transform.localScale.y + 0.1f, 0);

                else arrowPos += new Vector3(arrow.transform.localScale.x + 0.1f, 0, 0);
                
                GameObject spawnedArrow = Instantiate(arrow, arrowPos, trap.transform.rotation);

                switch (spawnedArrow.transform.eulerAngles.z)
                {
                    case 0:
                        spawnedArrow.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Speed, ForceMode2D.Impulse);
                        break;
                    case 90:
                        spawnedArrow.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Speed, ForceMode2D.Impulse);
                        break;
                    case -90:
                        spawnedArrow.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Speed, ForceMode2D.Impulse);
                        break;
                    case 180:
                        spawnedArrow.GetComponent<Rigidbody2D>().AddForce(Vector2.down * Speed, ForceMode2D.Impulse);
                        break;
                }
            }

        }   
        yield return new WaitForSeconds(1 / frequency);
        if (spawnArrows)
        {
            StartCoroutine(Spawn(frequency,amount));
        }
        yield return null;
    }
}
