using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatformerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;

    [Header("Combat Settings")]
    public int damageAmount = 1;
    public float attackCooldown = 0.5f; // Wait 0.5 seconds between hits

    private Rigidbody2D rb;
    private Transform target;
    private float lastAttackTime = -999f; // Start at -999 so it can attack immediately

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            target = Player.transform;
        }
    }

    void Update()
    {
        if (target == null) return;
        LookAtTarget();
    }

    void FixedUpdate()
    {
        if (target == null) return;
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        float xDirection = target.position.x - transform.position.x;

        if (Mathf.Abs(xDirection) > 0.1f)
        {
            rb.velocity = new Vector2(Mathf.Sign(xDirection) * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void LookAtTarget()
    {
        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (target.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // --- UPDATED DAMAGE LOGIC ---
    // We use OnCollisionStay2D so damage keeps happening if they stay touching
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if enough time has passed since the last attack
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    // Reset the timer
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}