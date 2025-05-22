using UnityEngine;

public class EnemyPickup : MonoBehaviour
{
    public float pickupRadius = 1.5f;
    public LayerMask pickupLayer;
    private EnemyCombat combat;

    void Start()
    {
        combat = GetComponent<EnemyCombat>();
    }

    void Update()
    {
        if (combat == null || combat.hasWeapon) return;

        Collider2D[] pickups = Physics2D.OverlapCircleAll(transform.position, pickupRadius, pickupLayer);
        foreach (Collider2D pickup in pickups)
        {
            WeaponPickup weapon = GetComponent<WeaponPickup>();

            if (weapon != null && weapon.weaponData != null)
            {
                // Equipar el arma
                combat.equippedWeapon = weapon.weaponData;
                combat.hasWeapon = true;

                Destroy(pickup.gameObject);
                Debug.Log("Enemigo recogió arma: " + weapon.weaponData.name);
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
