using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MagicFlaskBehaviour : BaseWeaponBehaviour
{
    [HideInInspector] public GameObject damageField;
    [HideInInspector] public float areaSize;
    [HideInInspector] public float damageSpeed;
    

    protected override void Start()
    {
        // The added vector is here to simulate an object that looks like it's falling
        Vector2 start = this.gameObject.transform.position + new Vector3(5, 9);

        // The destination spot is around the player in a 3 distance radius
        Vector2 destination = this.gameObject.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));

        StartCoroutine(PlayFlyingStraightAnimation(start, destination, speed));
    }

    protected override void FixedUpdate()
    {
        // This weapon behaves has custom movement and custom collision detection
        // So base class FixedUpdate not needed
        return;
    }

    private IEnumerator PlayFlyingStraightAnimation(Vector2 start, Vector2 finish, float speed)
    {
        float distance = Vector2.Distance(start, finish);

        float flyTime = distance / speed;

        this.gameObject.transform.position = start;

        //Smooth movement
        transform?.DOMove(finish, flyTime).SetEase(Ease.Linear);
        //Rotation
        transform?.DORotate(new Vector3(0f, 0f, 360f), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        //Wait until flask gets to finish position
        yield return new WaitForSeconds(flyTime);

        //Remove the tween animations
        transform.DOKill();

        CreateDamageArea();
        Destroy(this.gameObject);
    }

    public void CreateDamageArea()
    {
        GameObject area = Instantiate(damageField);

        MagicFlaskDamageAreaBehaviour behaviour = area.GetComponent<MagicFlaskDamageAreaBehaviour>();

        behaviour.damage = this.damage;
        behaviour.collisionRadius = areaSize;
        behaviour.damageSpeed = this.damageSpeed;

        area.transform.position = transform.position;
    }
}
