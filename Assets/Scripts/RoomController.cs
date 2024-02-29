using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject[] interiors;
    [SerializeField] public GameObject WinEvent;

    void Start()
    {
        Instantiate(interiors[Random.Range(0, interiors.Length)], transform);
        transform.parent.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
