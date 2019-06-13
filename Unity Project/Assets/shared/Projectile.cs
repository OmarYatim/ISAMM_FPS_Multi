using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeToLive;
    [SerializeField] float damage;

    void Start()
    {
        Destroy(gameObject, timeToLive);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        print("hit:" + other.transform.parent.name);
        if (other.gameObject.tag == "Hitbox") {
            string nom = other.transform.parent.GetComponent<NetworkEntity>().ID;
            string nom2 = "";
            for (int i = 0; i < nom.Length; i++) {
                if (nom[i] == '"') {
                    
                } else {
                    nom2 = nom2 + nom[i];
                }
            }
            NetworkControl.Instance.sendKilled2(nom2, other.transform.parent.gameObject); //tsaref to get the freaking ID
        }
    }

    /* Collision avec un obstacle , non joueur */
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    /* */
}
