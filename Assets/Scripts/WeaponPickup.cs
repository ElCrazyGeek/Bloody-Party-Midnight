using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponData;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (weaponData == null) return;

        if (other.CompareTag("Player"))
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.EquipWeapon(Instantiate(weaponData));
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            EnemyCombat enemy = other.GetComponent<EnemyCombat>();
            if (enemy != null && !enemy.hasWeapon)
            {
                enemy.EquippedWeaponFromPickup(Instantiate(weaponData));
                Destroy(gameObject);
            }
        }
    }
}
