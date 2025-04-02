using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class MagicFlaskDamageAreaBehaviourTests
{
    private GameObject flaskObj;
    private MagicFlaskDamageAreaBehaviour damageArea;

    [SetUp]
    public void SetUp()
    {
        flaskObj = new GameObject();
        damageArea = flaskObj.AddComponent<MagicFlaskDamageAreaBehaviour>();
        damageArea.collisionRadius = 1f;
        damageArea.damageSpeed = 1f;
        damageArea.damage = 10;
        damageArea.destroyAfterSeconds = 5f;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(flaskObj);
    }

    [UnityTest]
    public IEnumerator Start_InitializesValuesAndCallsSolveCollisions()
    {
        damageArea.Start();

        Assert.AreEqual(damageArea.destroyAfterSeconds, damageArea.currDestroySeconds, "Destroy timer should be set correctly");

        yield return null;
    }

    [Test]
    public void OnCollisionWithEnemy_DealsDamage()
    {
        GameObject enemyObj = new GameObject();
        EnemyHealthController enemyHealth = enemyObj.AddComponent<EnemyHealthController>();

        enemyHealth.currHP = 100;
        damageArea.OnCollisionWithEnemy(enemyObj.AddComponent<BoxCollider2D>());

        Assert.AreEqual(90, enemyHealth.currHP, "Enemy should take damage");

        Object.DestroyImmediate(enemyObj);
    }
}
