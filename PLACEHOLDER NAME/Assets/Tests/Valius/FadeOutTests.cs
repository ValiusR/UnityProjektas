using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FadeOutTests
{
    private FadeOut fadeOut;
    private Material testMaterial;
    private SpriteRenderer testRenderer;

    [SetUp]
    public void ParuostiTesta()
    {
        // sukuriame pagrindinius komponentus
        var gameObject = new GameObject();
        fadeOut = gameObject.AddComponent<FadeOut>();
        fadeOut.fadeAnimationTime = 0.1f;

        // paruošiame sprite rendererį su material
        testRenderer = gameObject.AddComponent<SpriteRenderer>();
        testMaterial = new Material(Shader.Find("Standard"));
        testRenderer.sharedMaterial = testMaterial;
        fadeOut.sprite = testRenderer;

        // privaloma: inicializuojame material kintamąjį
        fadeOut.material = testMaterial;
    }

    [Test]
    public void FadeAnimacija_PakeiciaMaterialProperty()
    {
        // nustatome pradinę reikšmę
        fadeOut.material.SetFloat("_currFade", 0f);

        // paleidžiame animacijos logiką tiesiogiai (nes testuojame Edit Mode)
        float elapsedTime = 0f;
        while (elapsedTime < fadeOut.fadeAnimationTime)
        {
            elapsedTime += 0.01f;
            float fadeAmount = Mathf.Lerp(0f, 1f, elapsedTime / fadeOut.fadeAnimationTime);
            fadeOut.material.SetFloat("_currFade", fadeAmount);
        }

        // tikriname ar reikšmė pasikeitė iki 1
        Assert.AreEqual(1f, fadeOut.material.GetFloat("_currFade"), 0.01f,
            "material property turėtų būti 1 po pilnos animacijos");
    }

    [TearDown]
    public void IsvalytiPoTesto()
    {
        // ištriname sukurtus objektus
        if (fadeOut != null && fadeOut.gameObject != null)
        {
            Object.DestroyImmediate(fadeOut.gameObject);
        }
        if (testMaterial != null)
        {
            Object.DestroyImmediate(testMaterial);
        }
    }
}