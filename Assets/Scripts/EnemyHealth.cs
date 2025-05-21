using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;
    private bool isDown = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDown) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDown = true;
            Debug.Log(name + " cayó al suelo");
        }
    }

    public void Execute()
    {
        if (!isDown) return;

        Debug.Log(name + " ejecutado");
        Destroy(gameObject);
    }

    public bool IsVulnerable()
    {
        return isDown;
    }

    public bool IsBackStab(Transform attacker)
    {
        Vector2 toAttacker = (attacker.position - transform.position).normalized;
        Vector2 forward = transform.up;

        float angle = Vector2.Angle(forward, toAttacker);

        // Si el atacante está detrás del enemigo (±60° por la espalda)
        return angle > 120f;
    }
}
