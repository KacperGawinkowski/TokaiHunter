using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBlue : MonoBehaviour
{

    //[SerializeField] private Vector3 target;
    private GameObject player;

    private NavMeshAgent agent;
    private Animator anim;

    public int damege = 10;

    private float minDistance;

    private bool canDamage = true;


    [SerializeField] private bool canWalk = true;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canWalk)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            StartCoroutine(Damage());
        }
    }

    private IEnumerator Damage()
    {
        if (canDamage)
        {
            canDamage = false;
            FindObjectOfType<FPSCharacterController>().TakeDamage(damege);
            yield return new WaitForSeconds(0.1f);
            canDamage = true;
        }
    }
}
