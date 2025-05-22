using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private EnemyCombat combat;

    void Start()
    {
        currentHealth = maxHealth;
        combat = GetComponent<EnemyCombat>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemigo dañado. Vida: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

   void Die()
{
    if (combat != null && combat.hasWeapon && combat.equippedWeapon != null)
    {
        GameObject pickup = combat.equippedWeapon.pickupPrefab;
        if (pickup != null)
        {
            Instantiate(pickup, transform.position, Quaternion.identity);
        }
    }

    Destroy(gameObject);
}


    void DropWeapon(Weapon weaponToDrop)
    {
        // Asegúrate de tener un prefab de pickup que tenga el script WeaponPickup
        GameObject pickupPrefab = weaponToDrop.pickupPrefab;

        if (pickupPrefab != null)
        {
            GameObject dropped = Instantiate(pickupPrefab, transform.position, Quaternion.identity);

            // Asignar el arma al pickup recién instanciado
            WeaponPickup wp = dropped.GetComponent<WeaponPickup>();
            if (wp != null)
            {
                wp.weaponData = weaponToDrop;
            }
        }
        else
        {
            Debug.LogWarning("No se ha asignado el prefab de pickup para el arma: " + weaponToDrop.name);
        }
    }
}
