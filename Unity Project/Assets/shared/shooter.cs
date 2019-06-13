using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter : MonoBehaviour {
    [SerializeField] float rateOfFire;
    [SerializeField] Projectile projectile;

    [HideInInspector]
    public Transform muzzle;
    float nextFireAllowed;
    public bool canFire;

    void Awake() {
        muzzle = transform.Find("Muzzle");
    }
    public virtual void Fire() {

        canFire = false;
        if (Time.time < nextFireAllowed)
            return;
        nextFireAllowed = Time.time + rateOfFire;
        print("Firing!" + Time.time);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 LookPoint;
        if (Physics.Raycast(ray, out hit)) {
            LookPoint = hit.point;
            Debug.Log("9atous");
        } else
            LookPoint = ray.GetPoint(50.0f);
            Projectile temp = Instantiate(projectile, muzzle.position, muzzle.rotation);
            temp.transform.LookAt(LookPoint);
            canFire = true;
            NetworkControl.Instance.SendFireToServer(ray, muzzle.position, muzzle.rotation);
    }

    public virtual void FireForOthers(Ray ray,Vector3 MuzzleP,Vector3 MuzzleR) {
        RaycastHit hit;
        Vector3 LookPoint;
        if (Physics.Raycast(ray, out hit)) {
            LookPoint = hit.point;
        } else
            LookPoint = ray.GetPoint(50.0f);
        Projectile temp = Instantiate(projectile, MuzzleP, Quaternion.Euler(MuzzleR));
        temp.transform.LookAt(LookPoint);
    }


}
