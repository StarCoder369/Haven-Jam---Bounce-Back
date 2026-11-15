using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float maxSpeed = 3f;
    public float acceleration = 20f;
    public float stopDistance = 0f;

    public GameObject explodeEffect;
    public GameObject projectile;
    public float projectileCooldown;

    public float maxHealth;
    public float currentHealth;

    public float dmgDelay;
    bool canGetDmged = true;

    public Transform shootPoint;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                target = player.transform;
            }
        }

        if (projectile != null)
        {
            StartCoroutine(ShootCycle());
        }
    }

    private void FixedUpdate()
    {
        Vector2 desired = Vector2.zero;

        if (target != null)
        {
            Vector2 toTarget = (Vector2)target.position - rb.position;

            float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (toTarget.magnitude > stopDistance)
            {
                desired = toTarget.normalized * maxSpeed;
            }
        }

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, desired, acceleration * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            Apply(rb, collision.transform.position, collision.GetComponent<Shield>().knockback);
            TakeDamage(collision.GetComponent<Shield>().dmg);
        }

        if (collision.CompareTag("Projectile"))
        {
            if (collision.GetComponent<Projectile>().canDmgEnemy)
            {
                TakeDamage(1);
                Destroy(collision);
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        if (canGetDmged)
        {
            currentHealth -= dmg;
            StartCoroutine(DamageDelay());

            if (currentHealth <= 0)
            {
                Die();
            }
        }

    }

    public IEnumerator DamageDelay()
    {
        canGetDmged = false;
        yield return new WaitForSeconds(0.1f);
        canGetDmged = true;
    }

    public void Die()
    {
        GameManager.Instance.AddScore(5);
        Destroy(Instantiate(explodeEffect, transform.position, Quaternion.identity), 3);
        Destroy(gameObject);
    }

    public void Apply(Rigidbody2D body, Vector2 sourcePosition, float force)
    {
        if (body == null)
        {
            return;
        }

        Vector2 direction = body.position - sourcePosition;

        if (direction.sqrMagnitude < 0.0001f)
        {
            direction = Vector2.up;
        }

        ApplyDirection(body, direction, force);
    }

    public void ApplyDirection(Rigidbody2D body, Vector2 direction, float force)
    {
        if (body == null)
        {
            return;
        }

        body.linearVelocity += direction.normalized * force;
    }

    public IEnumerator ShootCycle()
    {
        yield return new WaitForSeconds(projectileCooldown);
        Shoot();
        StartCoroutine(ShootCycle());
    }

    public void Shoot()
    {
        Instantiate(projectile, shootPoint.transform.position, transform.rotation);
    }
}