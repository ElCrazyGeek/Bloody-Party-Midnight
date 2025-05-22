using UnityEngine;

public class ThrownWeapon : MonoBehaviour
{
    public string targetTag = "Enemy";
    public int damage = 1;
    public bool isSharp = false;
    public float knockbackForce = 10f;
    public GameObject pickupPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                if (isSharp)
                {
                    eh.TakeDamage(damage * 2); // más daño si es filosa
                }
                else
                {
                    eh.TakeDamage(damage);
                }

                // Knockback físico (opcional)
                Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 direction = (other.transform.position - transform.position).normalized;
                    enemyRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
                }
            }

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            // Si choca con otra cosa, destruye el proyectil
            Destroy(gameObject);
        }
    }
    void Update()
{
    transform.Rotate(Vector3.forward * 720f * Time.deltaTime); // Rota el sprite
}

}
