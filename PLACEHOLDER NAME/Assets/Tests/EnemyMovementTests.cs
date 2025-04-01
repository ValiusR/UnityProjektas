using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyMovementTests
{
    private GameObject enemyObj;
    private EnemyMovement enemyMovement;
    private GameObject playerObj;

    // Pasiruošimas prieš kiekvieną testą
    [SetUp]
    public void Setup()
    {
        // Sukuriamas žaidėjo objektas
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        playerObj.AddComponent<PlayerMovementController>();
        playerObj.transform.position = Vector3.zero;

        // Sukuriamas priešo objektas
        enemyObj = new GameObject("Enemy");
        enemyMovement = enemyObj.AddComponent<EnemyMovement>();
        enemyMovement.moveSpeed = 5f;

        enemyMovement.player = playerObj.transform;
    }

    // Sunaikinimas po kiekvieno testo
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(enemyObj);
        Object.DestroyImmediate(playerObj);
    }

    // Ar Start metodas priskiria teisingas reikšmes kintamiesiems
    [Test]
    public void Start_InitializesPlayerReferenceAndSpeed()
    {
        var testEnemy = new GameObject().AddComponent<EnemyMovement>();
        testEnemy.moveSpeed = 3f;

        var testPlayer = new GameObject().AddComponent<PlayerMovementController>();

        testEnemy.Start();

        Assert.IsNotNull(testEnemy.player);
        Assert.AreEqual(3f, testEnemy.startMoveSpeed);

        Object.DestroyImmediate(testEnemy.gameObject);
        Object.DestroyImmediate(testPlayer.gameObject);
    }

    // Ar prasidėjus kolizijai su žaidėju priešo greitis yra prilyginamas nuliui
    [Test]
    public void OnCollisionEnter2D_StopsMovement_WhenPlayerCollision()
    {
        enemyMovement.SimulateCollisionEnter(playerObj);

        Assert.AreEqual(0f, enemyMovement.moveSpeed);
    }

    // Ar pasibaigus kolizijai su žaidėju priešo greitis yra lygus jo pradiniam greičiui
    [Test]
    public void OnCollisionExit2D_ResumesMovement_WhenPlayerLeaves()
    {
        enemyMovement.moveSpeed = 0f; // Vyksta kolizija su žaidėju

        enemyMovement.SimulateCollisionExit(playerObj);

        Assert.AreEqual(enemyMovement.startMoveSpeed, enemyMovement.moveSpeed);
    }

    // Ar įvykus kolizijai ne su žaidėju priešo greitis nepasikeičia
    [Test]
    public void OnCollisionEnter2D_IgnoresNonPlayerCollisions()
    {
        var otherObj = new GameObject("Obstacle");
        float originalSpeed = enemyMovement.moveSpeed;

        enemyMovement.SimulateCollisionEnter(otherObj);

        Assert.AreEqual(originalSpeed, enemyMovement.moveSpeed);
        Object.DestroyImmediate(otherObj);
    }
}
