using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    //[SerializeField] private Vector3 target;
    private GameObject player;

    private NavMeshAgent agent;
    private Animator anim;

    public int damege = 10;

    private float minDistance;


    [SerializeField] private bool canWalk = true;


    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectileSpawner;

    [SerializeField] float shootOffsetMin;
    [SerializeField] float shootOffsetMax;

    [SerializeField] bool boomBall = false;


    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y + Random.Range(-0.2f, 0.2f), transform.GetChild(0).transform.position.z);

        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        minDistance = agent.stoppingDistance;

        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        if (canWalk)
        {
            agent.SetDestination(player.transform.position);
        }

        RaycastHit hit;
        Vector3 direction = player.transform.position - transform.position;


        if (Physics.Raycast(transform.position, direction, out hit))
        {
            //Debug.Log("ray just hit the gameobject: " + hit.collider.gameObject.name);

            if (hit.collider.gameObject.tag != "Player")
            {
                agent.stoppingDistance = 0;
            }
            else
            {
                agent.stoppingDistance = minDistance;
            }
        }


        //if (agent.remainingDistance <= agent.stoppingDistance)
        //{

        //}
    }


    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(Random.Range(shootOffsetMin, shootOffsetMax));
        //shoot ball
        anim.SetTrigger("attack");

        StartCoroutine(Shoot());
    }

    public void CastFireball()
    {
        GameObject bullet = Instantiate(projectile, transform.position, Quaternion.LookRotation(player.transform.position - transform.position));
        if (!boomBall)
        {
            bullet.GetComponent<EnemyBall>().damage = damege;
        }
        else
        {
            bullet.GetComponent<EnemyBoomBall>().damage = damege;
        }
        
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 8000);
    }
}
