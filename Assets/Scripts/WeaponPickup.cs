using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private bool canPickup = false;
    private PlayerCombat player;

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            Weapon weapon = GetComponent<Weapon>();
            if (weapon != null && player != null)
            {
                player.EquipWeapon(weapon);
                gameObject.SetActive(false); // Desactiva el pickup
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            player = other.GetComponent<PlayerCombat>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
            player = null;
        }
    }
}
