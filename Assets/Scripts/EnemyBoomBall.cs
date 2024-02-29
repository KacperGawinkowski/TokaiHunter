using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomBall : MonoBehaviour
{

    [HideInInspector] public int damage;
    [SerializeField]
    private LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            RaycastHit[] hit = Physics.SphereCastAll(transform.position, 5f, Vector3.zero, 10f, layerMask);
            foreach (var item in hit)
            {
                Debug.Log(item.collider.gameObject);
                Debug.Log("SUSSUS AMOGUS");
                if (item.collider.gameObject.CompareTag("Player"))
                {
                    item.collider.gameObject.GetComponent<FPSCharacterController>().TakeDamage(damage);
                }
            }
            Destroy(gameObject);
        }
    }
}
