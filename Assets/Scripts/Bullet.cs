using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string targetTag = "Player"; // o "Enemy"
    public int damage = 1;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime); // se autodestruye después de un tiempo
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            if (targetTag == "Player")
                other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            else if (targetTag == "Enemy")
                other.GetComponent<EnemyHealth>()?.TakeDamage(damage);

            Destroy(gameObject);
        }
        else if (!other.isTrigger) // evitar que explote con zonas invisibles
        {
            Destroy(gameObject);
        }
    }
}
