using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // References
    [Header("References")]
    [SerializeField] Animator am;
    [SerializeField] PlayerMovementController pc;

    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<Animator>();
        pc = GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.PlayerInput.x != 0)
        {
            am.SetBool("MoveUp", false);
            am.SetBool("MoveDown", false);
            if (pc.PlayerInput.x > 0)
            {
                am.SetBool("MoveLeft", false);
                am.SetBool("MoveRight", true);
            }
            else
            {
                am.SetBool("MoveRight", false);
                am.SetBool("MoveLeft", true);
            }
        }
        else if (pc.PlayerInput.y != 0)
        {
            am.SetBool("MoveRight", false);
            am.SetBool("MoveLeft", false);
            if (pc.PlayerInput.y > 0)
            {
                am.SetBool("MoveDown", false);
                am.SetBool("MoveUp", true);
            }
            else
            {
                am.SetBool("MoveUp", false);
                am.SetBool("MoveDown", true);
            }
        }
        else
        {
            am.SetBool("MoveRight", false);
            am.SetBool("MoveLeft", false);
            am.SetBool("MoveUp", false);
            am.SetBool("MoveDown", false);
        }
    }
}
