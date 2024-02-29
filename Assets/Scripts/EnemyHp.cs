using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{

    [SerializeField] private int HP = 10;
    private bool dead = false;
    [SerializeField] private GameObject sound;

    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0 && !dead)
        {
            dead = true;
            Instantiate(sound, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
}
