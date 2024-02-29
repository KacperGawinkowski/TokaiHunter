using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MonoBehaviour
{

    [HideInInspector] public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<FPSCharacterController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
