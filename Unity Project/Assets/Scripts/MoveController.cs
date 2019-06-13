using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    Animator anim;


    public void Start() {
        anim = transform.GetComponent<Animator>();
        
    }
    public void Move(Vector2 direction)
    {
        if (Gm.Instance.InputController.Vertical!=0 || Gm.Instance.InputController.Horizontal != 0) { 
             transform.position += transform.forward * direction.x * Time.deltaTime +
            transform.right * direction.y * Time.deltaTime;
            anim.SetFloat("CanWalk", 0.2f);
            NetworkControl.Instance.UpdateMovement(transform.position);
        } else {
            anim.SetFloat("CanWalk", 0f);
        }

    }
}
