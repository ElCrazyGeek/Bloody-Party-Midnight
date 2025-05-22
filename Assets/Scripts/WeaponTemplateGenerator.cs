/*
using UnityEngine;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]
public class WeaponTemplateGenerator : MonoBehaviour
{
    [Header("Configuración del arma")]
    public string weaponName = "NewWeapon";
    public Sprite spriteBase;
    public int damage = 1;
    public bool isSharp = false;
    public float throwForce = 8f;

    [Header("Ruta de guardado de prefabs")]
    public string outputFolder = "Assets/Prefabs/Weapons/";

    [ContextMenu("Generar Arma y Prefabs")]
    public void GenerarArma()
    {
        if (spriteBase == null)
        {
            Debug.LogError("No se asignó el sprite base.");
            return;
        }

        string armaFolder = Path.Combine(outputFolder, weaponName);
        if (!AssetDatabase.IsValidFolder(armaFolder))
        {
            AssetDatabase.CreateFolder(outputFolder.TrimEnd('/'), weaponName);
        }

        // Crear Visual
        GameObject visualObject = new GameObject(weaponName + "_Equipped");
        var sr = visualObject.AddComponent<SpriteRenderer>();
        sr.sprite = spriteBase;

        // Crear Pickup
        GameObject pickupObject = new GameObject(weaponName + "_Pickup");
        var pickupSR = pickupObject.AddComponent<SpriteRenderer>();
        pickupSR.sprite = spriteBase;

        var weapon = pickupObject.AddComponent<Weapon>();
        var pickup = pickupObject.AddComponent<WeaponPickup>();
        var pickupCol = pickupObject.AddComponent<BoxCollider2D>();
        pickupCol.isTrigger = true;

        weapon.visualPrefab = visualObject;
        weapon.damage = damage;
        weapon.isSharp = isSharp;
        weapon.throwForce = throwForce;

        // Crear Thrown
        GameObject thrownObject = new GameObject(weaponName + "_Thrown");
        var thrownSR = thrownObject.AddComponent<SpriteRenderer>();
        thrownSR.sprite = spriteBase;

        var thrownRB = thrownObject.AddComponent<Rigidbody2D>();
        thrownRB.gravityScale = 0;

        var thrownCol = thrownObject.AddComponent<BoxCollider2D>();
        thrownCol.isTrigger = true;

        var thrown = thrownObject.AddComponent<ThrownWeapon>();
        thrown.isSharp = isSharp;
        thrown.damage = damage;

        // Conectar el pickupPrefab al thrown, y el projectilePrefab al pickup
        weapon.projectilePrefab = thrownObject;
        thrown.pickupPrefab = pickupObject;

        // Crear y guardar prefabs
        string pickupPath = Path.Combine(armaFolder, weaponName + "_Pickup.prefab");
        string thrownPath = Path.Combine(armaFolder, weaponName + "_Thrown.prefab");
        string visualPath = Path.Combine(armaFolder, weaponName + "_Equipped.prefab");

        PrefabUtility.SaveAsPrefabAssetAndConnect(pickupObject, pickupPath, InteractionMode.UserAction);
        PrefabUtility.SaveAsPrefabAssetAndConnect(thrownObject, thrownPath, InteractionMode.UserAction);
        PrefabUtility.SaveAsPrefabAssetAndConnect(visualObject, visualPath, InteractionMode.UserAction);

        // Limpiar escena
        DestroyImmediate(pickupObject);
        DestroyImmediate(thrownObject);
        DestroyImmediate(visualObject);

        AssetDatabase.Refresh();
        Debug.Log("Prefabs generados correctamente en: " + armaFolder);
    }
    [ContextMenu("Verificar Referencias")]
public void VerificarReferencias()
{
    string rootPath = "Assets/Prefabs/Weapons/";
    string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { rootPath });

    foreach (string guid in guids)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null) continue;

        Weapon weapon = prefab.GetComponent<Weapon>();
        ThrownWeapon thrown = prefab.GetComponent<ThrownWeapon>();
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        Collider2D col = prefab.GetComponent<Collider2D>();
        Rigidbody2D rb = prefab.GetComponent<Rigidbody2D>();

        // Validaciones generales
        if (sr == null || sr.sprite == null)
        {
            Debug.LogWarning($"[⚠️] {prefab.name}: no tiene sprite o SpriteRenderer.", prefab);
        }

        if ((weapon != null || thrown != null) && col == null)
        {
            Debug.LogWarning($"[⚠️] {prefab.name}: no tiene Collider2D.", prefab);
        }

        if (thrown != null && rb == null)
        {
            Debug.LogWarning($"[⚠️] {prefab.name}: es un proyectil pero no tiene Rigidbody2D.", prefab);
        }

        // Armas Pickup
        if (weapon != null)
        {
            if (weapon.projectilePrefab == null)
                Debug.LogError($"[❌] {prefab.name}: 'projectilePrefab' está vacío.", prefab);
            else if (weapon.projectilePrefab == prefab)
                Debug.LogWarning($"[⚠️] {prefab.name}: 'projectilePrefab' apunta a sí mismo.", prefab);

            if (weapon.visualPrefab == null)
                Debug.LogError($"[❌] {prefab.name}: 'visualPrefab' está vacío.", prefab);
        }

        // Armas lanzadas
        if (thrown != null)
        {
            if (thrown.pickupPrefab == null)
                Debug.LogError($"[❌] {prefab.name}: 'pickupPrefab' está vacío.", prefab);
            else if (thrown.pickupPrefab == prefab)
                Debug.LogWarning($"[⚠️] {prefab.name}: 'pickupPrefab' apunta a sí mismo.", prefab);
        }

        if (weapon == null && thrown == null)
        {
            Debug.Log($"[ℹ️] {prefab.name}: No es ni arma recogible ni proyectil.", prefab);
        }
    }

    Debug.Log("✅ Verificación completa.");
}

}
*/