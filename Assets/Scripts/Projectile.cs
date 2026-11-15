using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;

    public bool canDmgEnemy = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            rb.linearVelocity *= -1;
            canDmgEnemy = true;
        }
    }
}
