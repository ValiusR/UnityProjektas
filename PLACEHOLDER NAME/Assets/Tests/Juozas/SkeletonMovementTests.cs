using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkeletonMovementTests
{
    private GameObject skeletonObj;
    private SkeletonMovement skeleton;
    private GameObject playerObj;
    private Rigidbody2D rb;

    // Pasiruošimas prieš kiekvieną testą
    [SetUp]
    public void Setup()
    {
        // Sukuriamas žaidėjas
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        playerObj.AddComponent<PlayerMovementController>();
        playerObj.transform.position = Vector3.zero;

        // Sukuriamas skeletonas
        skeletonObj = new GameObject("Skeleton");
        skeleton = skeletonObj.AddComponent<SkeletonMovement>();
        rb = skeletonObj.AddComponent<Rigidbody2D>();

        skeleton.Start(); 
    }

    // Sunaikinimas po kiekvieno testo
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(skeletonObj);
        Object.DestroyImmediate(playerObj);
    }

    // Ar paveldi EnemyMovement klasę
    [Test]
    public void Inherits_From_EnemyMovement()
    {
        Assert.IsNotNull(skeleton as EnemyMovement);
    }

    // Ar Start metodas priskiria teisingas reikšmes kintamiesiems
    [Test]
    public void Start_InitializesPlayerReferenceAndSpeed()
    {
        var testSkeleton = new GameObject().AddComponent<SkeletonMovement>();
        testSkeleton.moveSpeed = 3f;

        var testPlayer = new GameObject().AddComponent<PlayerMovementController>();

        testSkeleton.Start();

        Assert.IsNotNull(skeleton.player);
        Assert.AreEqual(3f, testSkeleton.startMoveSpeed);

        Object.DestroyImmediate(testSkeleton.gameObject);
        Object.DestroyImmediate(testPlayer.gameObject);
    }

    // Ar prasidėjus kolizijai su žaidėju skeletono greitis yra prilyginamas nuliui
    [Test]
    public void OnCollisionEnter2D_Stops_On_Player_Collision()
    {
        skeleton.SimulateCollisionEnter(playerObj);

        Assert.AreEqual(0f, skeleton.moveSpeed);
    }

    // Ar pasibaigus kolizijai su žaidėju skeletono greitis yra lygus jo pradiniam greičiui
    [Test]
    public void OnCollisionExit2D_Resumes_After_Player_Collision()
    {
        skeleton.moveSpeed = 0f; // Vyksta kolizija su žaidėju

        skeleton.SimulateCollisionExit(playerObj);

        Assert.AreEqual(skeleton.startMoveSpeed, skeleton.moveSpeed);
    }

    // Ar įvykus kolizijai ne su žaidėju skeletono greitis nepasikeičia
    [Test]
    public void OnCollisionEnter2D_Ignores_NonPlayer_Collisions()
    {
        float originalSpeed = skeleton.moveSpeed;
        var wall = new GameObject("Wall");

        skeleton.SimulateCollisionEnter(wall);

        Assert.AreEqual(originalSpeed, skeleton.moveSpeed);
        Object.DestroyImmediate(wall);
    }
}
