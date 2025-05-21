using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    public float normalSpeed = 5f;
    public float slowSpeed = 2.5f;
    public float torpezaDuration = 2f;

    private float torpezaTimer = 0f;
    private PlayerController movement;



    void Start()
    {
        currentHealth = maxHealth;
      movement = GetComponent<PlayerController>();


    }

    void Update()
    {
        if (torpezaTimer > 0)
        {
            torpezaTimer -= Time.deltaTime;
            if (movement != null) movement.SetSpeed(slowSpeed);
        }
        else
        {
            if (movement != null) movement.SetSpeed(normalSpeed);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        torpezaTimer = torpezaDuration;

        Debug.Log("Jugador recibió daño. Vida: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Jugador curado. Vida: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Jugador murió.");
        // Aquí podrías reiniciar el nivel o mostrar menú de muerte
    }
}
