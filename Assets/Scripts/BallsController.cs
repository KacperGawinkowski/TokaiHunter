using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsController : MonoBehaviour
{
    [SerializeField] private float speed;
    [HideInInspector] public int dmg;
    private AudioSource eH;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed * 100);
        eH = GameObject.Find("EnemyHit").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Enemy Hit!");
            eH.Play();
            other.GetComponent<EnemyHp>().TakeDamage(dmg);
            Destroy(gameObject);
        }
        else if (other.tag != "Player" && other.tag != "Enemy")
        {
            Destroy(gameObject);
        }

    }
}
