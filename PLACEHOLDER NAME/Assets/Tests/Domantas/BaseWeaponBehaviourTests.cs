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
        weaponObj = new GameObject();
        weapon = weaponObj.AddComponent<BaseWeaponBehaviour>();

        rb = weaponObj.AddComponent<Rigidbody2D>();

        var rbField = typeof(BaseWeaponBehaviour).GetField("rb", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        rbField.SetValue(weapon, rb);

        weapon.speed = 5f;
        weapon.damage = 10;
        weapon.destroyAfterSeconds = 1f;
    }

    [TearDown]
    public void TearDown()
    {
        if (weaponObj != null)
        {
            Object.DestroyImmediate(weaponObj);
        }
    }


    [Test]
    public void SolveCollisions_HitsEnemy_DealsDamage()
    {
        GameObject enemyObj = new GameObject();
        enemyObj.tag = "Enemy";

        var enemyCollider = enemyObj.AddComponent<BoxCollider2D>();
        var enemyHealth = enemyObj.AddComponent<EnemyHealthController>();

        enemyHealth.currHP = 100;

        Collider2D[] hitColliders = { enemyCollider };

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                weapon.OnCollisionWithEnemy(collider);
            }
        }

        Assert.AreEqual(90, enemyHealth.currHP);

        Object.DestroyImmediate(enemyObj);
    }

    [Test]
    public void DidHitProp_ReturnsTrueForPropLayer()
    {
        GameObject propObj = new GameObject();
        Collider2D propCollider = propObj.AddComponent<BoxCollider2D>();
        propObj.layer = LayerMask.NameToLayer("Wall");

        weapon.propLayer = LayerMask.NameToLayer("Wall");

        bool result = weapon.DidHitProp(propCollider);

        Assert.IsTrue(result, "DidHitProp should return true for prop layer.");

        Object.DestroyImmediate(propObj);
    }
}
