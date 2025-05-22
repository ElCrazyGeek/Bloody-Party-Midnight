using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Armas/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public int damage = 1;
    public bool isSharp = false;
    public float throwForce = 10f;
    public GameObject projectilePrefab; // solo para la pistola
    public GameObject pickupPrefab;     // objeto que aparece al morir o lanzar
}
