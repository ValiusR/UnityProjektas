using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlaskBehaviour : BaseWeaponBehaviour
{

    public override void Start()
    {
        Vector2 start = this.gameObject.transform.position;
        Vector2 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        StartCoroutine(PlayFlyingAnimation(start, destination, speed));
    }

    public override void FixedUpdate()
    {
        // This weapon behaves has custom movement and custom collision detection
        // So base class FixedUpdate not needed
        return;
    }

    private IEnumerator PlayFlyingAnimation(Vector2 start, Vector2 finish, float speed)
    {
        float distance = Vector2.Distance(start, finish);

        float flyTime = distance / speed;
        float currTime = 0;

        while (currTime < flyTime) 
        { 
            currTime += Time.deltaTime;

            this.transform.position = Vector2.Lerp(start, finish, currTime/flyTime);

            yield return null;

        }

        Destroy(this.gameObject);
    }
}
