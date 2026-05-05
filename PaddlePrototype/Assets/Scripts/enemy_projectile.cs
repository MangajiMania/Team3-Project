using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lifeTime = 5f;

    [SerializeField] private float stunDuration = 0.5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.down * moveSpeed;
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PaddleDebuff debuff = other.GetComponentInParent<PaddleDebuff>();

        if (debuff != null)
        {
            debuff.ApplyStun(stunDuration);
            Destroy(gameObject);
            return;
        }
    }
}