using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class MagicFlaskBehaviourTests
{
    private GameObject flaskObj;
    private MagicFlaskBehaviour flask;
    private GameObject damageFieldPrefab;

    [SetUp]
    public void SetUp()
    {
        // Create MagicFlaskBehaviour object
        flaskObj = new GameObject();
        flask = flaskObj.AddComponent<MagicFlaskBehaviour>();
        flaskObj.AddComponent<Rigidbody2D>();

        // Create a mock damage field prefab
        damageFieldPrefab = new GameObject();
        damageFieldPrefab.AddComponent<MagicFlaskDamageAreaBehaviour>();
        flask.damageField = damageFieldPrefab;

        // Set weapon stats
        flask.damage = 10;
        flask.speed = 5;
        flask.areaSize = 3f;
        flask.damageSpeed = 0.5f;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(flaskObj);
        Object.DestroyImmediate(damageFieldPrefab);
    }

    [Test]
    public void CreateDamageArea_SpawnsDamageFieldWithCorrectStats()
    {
        flask.CreateDamageArea();

        MagicFlaskDamageAreaBehaviour spawnedArea = Object.FindObjectOfType<MagicFlaskDamageAreaBehaviour>();
        Assert.IsNotNull(spawnedArea, "Damage field should be instantiated");
        Assert.AreEqual(flask.damage, spawnedArea.damage, "Damage should match");
        Assert.AreEqual(flask.areaSize, spawnedArea.size, "Area size should match");
        Assert.AreEqual(flask.damageSpeed, spawnedArea.damageSpeed, "Damage speed should match");
    }
}