using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlink : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] Color blinkColor;

    [SerializeField] float blinkAnimationTime;

    [SerializeField] float currBlinkAmount;

    [SerializeField] SpriteRenderer sprite;

    private void Awake()
    {
        Debug.Log("DamageBlink Awake. SpriteRenderer: " + (sprite != null));
    }

    public virtual void PlayBlink()
    {
        Debug.Log("Base play blink");
        StartCoroutine(BlinkAnimation());
    }

    private IEnumerator BlinkAnimation()
    {
        float elapsedTime = 0f;

        while (elapsedTime < blinkAnimationTime)
        {
            elapsedTime += Time.deltaTime;

            currBlinkAmount = Mathf.Lerp(1f, 0f, elapsedTime / blinkAnimationTime);

            sprite.material.SetFloat("_blinkAmount", currBlinkAmount);

            yield return null;
        }

    }
}
