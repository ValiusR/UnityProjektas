using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.Animations;

public class SkeletonControllerTests
{
    private GameObject skeletonObj;
    private SkeletonController skeleton;
    private GameObject playerObj;
    private GameObject arrowObj;
    private Transform arrowPos;
    private MockEnemyMovement mockEnemyMovement;
    private Animator mockAnimator;

    // Testavimui skirta EnemyMovement klasė
    public class MockEnemyMovement : EnemyMovement
    {
        public override void Start()
        {
            startMoveSpeed = moveSpeed;
        }
    }

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
        skeleton = skeletonObj.AddComponent<SkeletonController>();
        skeleton.player = playerObj.transform;

        mockEnemyMovement = skeletonObj.AddComponent<MockEnemyMovement>();
        skeleton.em = mockEnemyMovement;
        skeleton.attackSpeed = 2f;
        skeleton.timer = skeleton.attackSpeed;

        mockAnimator = skeletonObj.AddComponent<Animator>();
        var controller = new AnimatorController();
        controller.AddLayer("Test Layer");
        controller.AddParameter("Move", AnimatorControllerParameterType.Bool);
        controller.AddParameter("Shoot", AnimatorControllerParameterType.Bool);
        mockAnimator.runtimeAnimatorController = controller;
        skeleton.am = mockAnimator;

        skeleton.sr = skeletonObj.AddComponent<SpriteRenderer>();

        arrowObj = new GameObject("Arrow");
        skeleton.arrow = arrowObj;

        arrowPos = new GameObject("ArrowPos").transform;
        skeleton.arrowPos = arrowPos;
    }

    // Sunaikinimas po kiekvieno testo
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(skeletonObj);
        Object.DestroyImmediate(playerObj);
        Object.DestroyImmediate(arrowObj);
        Object.DestroyImmediate(arrowPos.gameObject);
    }

    // Ar teisingai priskiriami kintamieji
    [Test]
    public void Start_InitializesComponents()
    {
        var newSkeleton = new GameObject().AddComponent<SkeletonController>();
        var mockMovement = newSkeleton.gameObject.AddComponent<MockEnemyMovement>();

        newSkeleton.em = mockMovement;
        newSkeleton.Start();

        Assert.IsNotNull(newSkeleton.em);
        Assert.AreEqual(mockMovement.moveSpeed, newSkeleton.em.startMoveSpeed);

        Object.DestroyImmediate(newSkeleton.gameObject);
    }

    // Ar pradeda judėti, kai būna pakankamai toli nuo žaidėjo
    [Test]
    public void Update_StartsMoving_WhenFarFromPlayer()
    {
        skeletonObj.transform.position = new Vector3(5f, 0f, 0f);

        skeleton.Update();

        Assert.IsTrue(mockAnimator.GetBool("Move"));
        Assert.IsFalse(mockAnimator.GetBool("Shoot"));
    }

    // Ar sustoja, kai būna šalia žaidėjo
    [Test]
    public void Update_StopsMovingAndShoots_WhenCloseToPlayer()
    {
        skeletonObj.transform.position = new Vector3(1f, 0f, 0f); // Close to player

        skeleton.Update();

        Assert.IsFalse(mockAnimator.GetBool("Move"));
        Assert.IsTrue(mockAnimator.GetBool("Shoot"));
    }

    // Ar laikmatis (cooldown timer) mažėja, kai skeletonas šaudo
    [Test]
    public void Update_DecrementsTimer_WhenShooting()
    {
        skeletonObj.transform.position = new Vector3(1f, 0f, 0f);
        skeleton.timer = 1f;

        skeleton.Update();

        Assert.Less(skeleton.timer, 1f);
    }

    // Ar atisnaujina timer (tampa lygus attackSpeed), kai jis pasiekia 0
    [Test]
    public void Update_ResetsTimerAndShoots_WhenTimerExpires()
    {
        skeleton.timer = 0;
        skeletonObj.transform.position = new Vector3(1f, 0f, 0f); // Close to player

        skeleton.Update();

        Assert.AreEqual(skeleton.attackSpeed, skeleton.timer);
    }

    // Ar Shoot metodas sukuria strėlę
    [Test]
    public void Shoot_InstantiatesArrow()
    {
        skeleton.Shoot();

        var arrows = Object.FindObjectsOfType<GameObject>();
        bool arrowExists = false;
        foreach (var obj in arrows)
        {
            if (obj.name == "Arrow")
            {
                arrowExists = true;
                break;
            }
        }
        Assert.IsTrue(arrowExists);
    }

    // Ar skeletono sprite yra apverčiamas horizontaliai, kai žaidėjas yra po dešine
    [Test]
    public void Update_FlipsSprite_WhenPlayerIsOnRight()
    {
        playerObj.transform.position = new Vector3(5f, 0f, 0f);
        skeletonObj.transform.position = Vector3.zero;

        skeleton.Update();

        Assert.IsTrue(skeleton.sr.flipX);
    }

    // Ar skeletono sprite yra apverčiamas horizontaliai, kai žaidėjas yra po kaire
    [Test]
    public void Update_FlipsSprite_WhenPlayerIsOnLeft()
    {
        playerObj.transform.position = new Vector3(-5f, 0f, 0f);
        skeletonObj.transform.position = Vector3.zero;

        skeleton.Update();

        Assert.IsFalse(skeleton.sr.flipX);
    }
}
