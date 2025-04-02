using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DamageBlinkTests
{
    private DamageBlink blink;
    private Material testMaterial;

    [SetUp]
    public void Setup()
    {
        // paruošiame testinius komponentus
        var go = new GameObject();
        blink = go.AddComponent<DamageBlink>();
        blink.blinkAnimationTime = 0.1f;

        // naudojame sharedMaterial, kad išvengtume memory leak
        var renderer = go.AddComponent<SpriteRenderer>();
        testMaterial = new Material(Shader.Find("Standard"));
        renderer.sharedMaterial = testMaterial;
        blink.sprite = renderer;
    }

    [Test]
    public void BlinkAnimacija_PakeiciaMaterialProperty()
    {
        // nustatome pradinę reikšmę
        blink.sprite.sharedMaterial.SetFloat("_blinkAmount", 1f);

        // paleidžiame animaciją (simuliuojame koroutiną)
        float elapsedTime = 0f;
        while (elapsedTime < blink.blinkAnimationTime)
        {
            elapsedTime += 0.01f;
            blink.sprite.sharedMaterial.SetFloat("_blinkAmount",
                Mathf.Lerp(1f, 0f, elapsedTime / blink.blinkAnimationTime));
        }

        // tikriname galutinę reikšmę
        Assert.AreEqual(0f, blink.sprite.sharedMaterial.GetFloat("_blinkAmount"), 0.01f,
            "material property turėtų būti 0 po animacijos");
    }

    [TearDown]
    public void Teardown()
    {
        // išvalome testinius objektus
        Object.DestroyImmediate(blink.gameObject);
        Object.DestroyImmediate(testMaterial);
    }
}