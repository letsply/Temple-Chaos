using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float knockBack;
    [SerializeField] float speed;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;
    Vector3 Target;
    Rigidbody2D rb;

    private void Start()
    {
        Target = pos1.position;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        transform.position -= Target * speed;
        
    }
}
