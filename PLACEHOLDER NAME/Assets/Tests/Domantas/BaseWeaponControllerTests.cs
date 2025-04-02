using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class BaseWeaponBehaviourTests
{
    private GameObject weaponObj;
    private BaseWeaponBehaviour weapon;
    private Rigidbody2D rb;

    [SetUp]
    public void SetUp()
    {
        // Create the weapon GameObject
        weaponObj = new GameObject();
        weapon = weaponObj.AddComponent<BaseWeaponBehaviour>();

        // Attach a real Rigidbody2D component
        rb = weaponObj.AddComponent<Rigidbody2D>();

        // Manually set private Rigidbody2D reference using reflection
        var rbField = typeof(BaseWeaponBehaviour).GetField("rb", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        rbField.SetValue(weapon, rb);

        weapon.speed = 5f;
        weapon.damage = 10;
        weapon.destroyAfterSeconds = 1f; // Short lifespan for testing
    }

    [TearDown]
    public void TearDown()
    {
        // Use DestroyImmediate to avoid issues in Edit Mode
        if (weaponObj != null)
        {
            Object.DestroyImmediate(weaponObj);
        }
    }

    [Test]
    public void Start_InitializesDirection()
    {
        // Ensure the camera exists before calling Start()
        if (Camera.main == null)
        {
            var cameraObj = new GameObject("MainCamera");
            cameraObj.AddComponent<Camera>();
            cameraObj.transform.position = new Vector3(0, 0, -10);
        }

        // Call Start manually
        weapon.Start();

        // Assert direction is set
        Assert.AreNotEqual(Vector2.zero, weaponObj.transform.up);
    }

    

    [Test]
    public void SolveCollisions_HitsEnemy_DealsDamage()
    {
        // Create enemy object and add necessary components
        GameObject enemyObj = new GameObject();
        enemyObj.tag = "Enemy";

        var enemyCollider = enemyObj.AddComponent<BoxCollider2D>();
        var enemyHealth = enemyObj.AddComponent<EnemyHealthController>();

        // Set enemy health to a known state
        enemyHealth.currHP = 100;

        // Simulate collision manually
        Collider2D[] hitColliders = { enemyCollider };

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                weapon.OnCollisionWithEnemy(collider);
            }
        }

        // Verify the enemy took damage
        Assert.AreEqual(90, enemyHealth.currHP);

        // Clean up
        Object.DestroyImmediate(enemyObj);
    }
}
