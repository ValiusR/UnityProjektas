using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float timeUntilDestroyObject;

    // Update is called once per frame
    void Update()
    {
        timeUntilDestroyObject -= Time.deltaTime;
        
        if(timeUntilDestroyObject < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
