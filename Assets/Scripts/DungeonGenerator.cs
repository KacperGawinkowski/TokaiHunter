using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DungeonGenerator : MonoBehaviour
{

    [SerializeField] private GameObject room;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject winPanel;
    private GameObject stageCounterText;

    private int counter = 1;

    private NavMeshSurface navMesh;

    private bool created;

    //private List<GameObject> activeRooms;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
        stageCounterText = GameObject.Find("StageCounter");
        created = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (created && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("RoomInterior"));
            created = false;

            //TODO: pojawi� supermoce
            transform.GetChild(transform.childCount - 1).GetComponent<RoomController>().WinEvent.SetActive(true);
        }
    }

    public void CreateNextRoom()
    {

        if (counter >= 15)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            winPanel.SetActive(true);
        }

        Instantiate(room, new Vector3(0, 0, (float)66 * counter), Quaternion.identity, transform);
        counter++;
        Debug.Log(counter);
        stageCounterText.GetComponent<TextMeshProUGUI>().text = "Current Level: " + (counter - 1);
        if (counter > 2)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        navMesh.BuildNavMesh();
        //SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        int id = counter - 1;
        Debug.Log("ENEMY SPAWN: " + id);
        for (int i = 0; i < id; i++)
        {
            Vector3 randomPoint = new Vector3(Random.Range(-30f, 30f), 0, (float)66 * id + Random.Range(-5, 26f));
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                Instantiate(enemies[Random.Range(0, enemies.Length)], randomPoint, Quaternion.identity, transform);
            }
            else
            {
                i--;
            }
        }
        created = true;
    }
}
