using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : shooter
{
    public static RPG Instance;
    private void Start() {
        Instance = this;
    }
    public override void Fire()
    {
        base.Fire();
        if (canFire)
        {

        }
    }
}
