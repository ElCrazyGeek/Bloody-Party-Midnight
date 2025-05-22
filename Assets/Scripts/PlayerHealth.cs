using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    public float normalSpeed = 5f;
    public float slowSpeed = 2.5f;
    public float torpezaDuration = 2f;
    private float torpezaTimer = 0f;

    private PlayerController movement;

    [Header("HUD")]
    public Slider healthSlider;
    public GameObject deathScreen;

    private bool isDead = false;
    public Image fillImage; // referencia al objeto "Fill" del Slider
    public Color fullColor = Color.green;
    public Color midColor = Color.yellow;
    public Color lowColor = Color.red;


    void Start()
    {
        currentHealth = maxHealth;
        movement = GetComponent<PlayerController>();

        // Inicializar HUD
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (deathScreen != null)
            deathScreen.SetActive(false);
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

        // Reinicio tras morir
        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        torpezaTimer = torpezaDuration;

        Debug.Log("Jugador recibió daño. Vida: " + currentHealth);

        UpdateHUD();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Jugador curado. Vida: " + currentHealth);
        UpdateHUD();
    }

   void UpdateHUD()
{
    if (healthSlider != null)
    {
        healthSlider.value = currentHealth;

        if (fillImage != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;

            if (healthPercent > 0.6f)
                fillImage.color = fullColor;
            else if (healthPercent > 0.3f)
                fillImage.color = midColor;
            else
                fillImage.color = lowColor;
        }
    }
}


    void Die()
    {
        isDead = true;
        Debug.Log("Jugador murió.");
        if (deathScreen != null)
            deathScreen.SetActive(true);
    }
    
}
