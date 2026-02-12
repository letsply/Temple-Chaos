using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float knockBack;
    [SerializeField] float speed;
    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;
    [SerializeField]float dir;
    [SerializeField] Vector3 Target;
    Rigidbody2D rb;

    private void Start()
    {
        Target = pos1;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb.AddForce(new Vector2(dir * speed,0), ForceMode2D.Force);

        dir = Mathf.Sign(Target.x - transform.localPosition.x);

        bool InRange = Mathf.Clamp(transform.localPosition.x, Target.x - 0.25f, Target.x + 0.25f) == transform.localPosition.x;
        if (InRange)
        {
            if (Target == pos1)
            {
                Target = pos2;
            }
            else 
            {
                Target = pos1;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(rb.linearVelocityX * knockBack,knockBack), ForceMode2D.Impulse);
            collision.gameObject.GetComponent<Health>().TakeDamage(1);
        }
    }
}
