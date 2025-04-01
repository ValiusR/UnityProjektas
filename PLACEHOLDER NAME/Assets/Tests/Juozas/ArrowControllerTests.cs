using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ArrowControllerTests
{
    private GameObject arrowObj;
    private ArrowController arrow;
    private Rigidbody2D rb;
    private GameObject playerObj;
    private GameObject skeletonObj;
    private MockPlayerHealth mockPlayerHealth;

    // Testavimui skirta PlayerHealthController klasė
    public class MockPlayerHealth : PlayerHealthController
    {
        public int damageTaken;
        public override void TakeDamage(int damage)
        {
            damageTaken = damage;
        }
    }

    // Pasiruošimas prieš kiekvieną testą
    [SetUp]
    public void Setup()
    {
        // Sukuriamas žaidėjas
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        mockPlayerHealth = playerObj.AddComponent<MockPlayerHealth>();

        // Sukuriamas skeletonas
        skeletonObj = new GameObject("Skeleton");
        var skeletonController = skeletonObj.AddComponent<SkeletonController>();
        skeletonController.damage = 10;

        // Sukuriama strėlė
        arrowObj = new GameObject("Arrow");
        arrow = arrowObj.AddComponent<ArrowController>();
        rb = arrowObj.AddComponent<Rigidbody2D>();

        arrow.skeleton = skeletonObj;
        arrow.speed = 5f;

        arrow.player = playerObj;
        arrow.Start();
    }

    // Sunaikinimas po kiekvieno testo
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(arrowObj);
        Object.DestroyImmediate(playerObj);
        Object.DestroyImmediate(skeletonObj);
    }

    // Ar Start metodas nustato teisingą strėlės pasukimo kampą
    [Test]
    public void Start_SetsCorrectRotation()
    {
        playerObj.transform.position = new Vector3(1f, 0f, 0f);
        arrowObj.transform.position = Vector3.zero;

        arrow.Start();

        Assert.AreEqual(90f, arrowObj.transform.eulerAngles.z, 0.01f);
    }

    // Ar prasidėjus kolizijai žaidėjas praranda atitinkamą kiekį gyvybių
    // ir yra sunaikinama strėlė
    [Test]
    public void OnCollisionEnter2D_DamagesPlayer_WhenHittingPlayer()
    {
        arrow.SimulateCollisionEnter(playerObj);

        Assert.AreEqual(10, mockPlayerHealth.damageTaken);
        Assert.IsTrue(arrowObj == null);
    }

    // Ar prasidėjus kolizijai ne su žaidėju jis nepraranda gyvybių ir strėlė nėra sunaikinama
    [Test]
    public void OnCollisionEnter2D_IgnoresNonPlayerCollisions()
    {
        var wall = new GameObject("Wall");

        arrow.SimulateCollisionEnter(wall);

        Assert.AreEqual(0, mockPlayerHealth.damageTaken);
        Assert.IsNotNull(arrowObj);
        Object.DestroyImmediate(wall);
    }
}
