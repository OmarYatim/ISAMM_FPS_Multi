using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] shooter RPG;
    
    void Awake()
    {
        if(RPG == null)
        {
            RPG = transform.GetComponentInChildren<shooter>();
        }
    }

    void Update()
    {
        if (Gm.Instance.InputController.Fire1)
        {
            RPG.Fire();

        }
    }
}
