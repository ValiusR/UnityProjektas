using NUnit.Framework;
using UnityEngine;

public class XPOrbTests
{
    [Test]
    public void OnTriggerEnter2D_SunaikinaOrba()
    {
        // paruošiame orbą
        var orb = new GameObject().AddComponent<XPOrb>();
        orb.xpValue = 10;
        var collider = orb.gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        // sukuriame mock žaidėją
        var player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<BoxCollider2D>();

        // simulioujame susidūrimą
        orb.OnTriggerEnter2D(player.GetComponent<Collider2D>());

        // tikriname ar orbas sunaikintas
        Assert.IsTrue(orb == null);
    }
}