using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{

    [SerializeField] float fadeAnimationTime;

    [SerializeField] float currFadeAmount;

    [SerializeField] SpriteRenderer sprite;

    [SerializeField] Material material;

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
