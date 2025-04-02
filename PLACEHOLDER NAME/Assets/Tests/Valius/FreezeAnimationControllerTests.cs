using NUnit.Framework;
using UnityEngine;

public class FreezeAnimationControllerTests
{
    [Test]
    public void ActivateFreeze_PakeiciaSpalvaIrGreiti()
    {
        // paruošiame freeze efektą
        var controller = new GameObject().AddComponent<FreezeAnimationController>();
        controller.freezeColor = Color.blue;
        controller.spriteRenderer = controller.gameObject.AddComponent<SpriteRenderer>();
        controller.enemyMovement = controller.gameObject.AddComponent<EnemyMovement>();
        controller.enemyMovement.moveSpeed = 5f;

        // paleidžiame freeze efektą
        controller.ActivateFreeze(0.1f, 2f);

        // tikriname ar parametrai pakeisti
        Assert.AreEqual(Color.blue, controller.spriteRenderer.color);
        Assert.AreEqual(2.5f, controller.enemyMovement.moveSpeed);
    }
}