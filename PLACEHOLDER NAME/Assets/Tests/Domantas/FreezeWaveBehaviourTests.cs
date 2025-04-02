using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Collections;

public class FreezeWaveBehaviourTests
{
    private GameObject enemyGameObject;

    private Collider2D enemyCollider;
    private EnemyHealthController enemyHealthController;

    private GameObject wave;
    private FreezeWaveBehaviour freezeWaveBehaviour;

    [SetUp]
    public void Setup()
    {
        // Create a GameObject for the enemy and add a Collider2D
        enemyGameObject = new GameObject();
        enemyCollider = enemyGameObject.AddComponent<BoxCollider2D>();
        enemyHealthController = enemyGameObject.AddComponent<EnemyHealthController>();
        enemyHealthController.currHP = 1000;

        wave = new GameObject();
        freezeWaveBehaviour = wave.AddComponent<FreezeWaveBehaviour>();
        // Set freeze length and strength for testing
        freezeWaveBehaviour.freezeLength = 5f;
        freezeWaveBehaviour.freezeStrength = 10f;
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up after each test
        GameObject.DestroyImmediate(enemyGameObject);
        GameObject.DestroyImmediate(wave);
    }

    [Test]
    public void OnCollisionWithEnemy_ThrowsException_WhenNoFreezeAnimationControllerIsPresent()
    {
        // Act & Assert: Check if the exception is thrown
        var exception = Assert.Throws<Exception>(() => freezeWaveBehaviour.OnCollisionWithEnemy(enemyCollider));
        Assert.AreEqual("Why is there no FreezeAnimationController on enemy?", exception.Message);
    }
}