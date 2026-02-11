using UnityEngine;
using UnityEngine.UIElements;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Rigidbody2D>().AddForce(rb.linearVelocity * rb.mass, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
        else if (collision.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }

}
