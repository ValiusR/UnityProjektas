using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{

    [SerializeField] public float fadeAnimationTime;

    [SerializeField] public float currFadeAmount;

    [SerializeField] public SpriteRenderer sprite;

    [SerializeField] public Material material;

    public virtual IEnumerator FadeAnimation()
    {
        sprite.material = material;

        float elapsedTime = 0f;

        while (elapsedTime < fadeAnimationTime)
        {
            elapsedTime += Time.deltaTime;

            currFadeAmount = Mathf.Lerp(0f, 1f, elapsedTime / fadeAnimationTime);

            sprite.material.SetFloat("_currFade", currFadeAmount);

            yield return null;
        }

    }

}
