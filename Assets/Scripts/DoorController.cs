using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator DoorAnimator;
    void Start()
    {
        DoorAnimator = transform.parent.GetComponent<Animator>();
    }

    public void OpenDoors()
    {
        DoorAnimator.SetBool("OpenDoors", true);
    }

    public void OpenSecondDoors(Animator prevDoors)
    {
        prevDoors.SetBool("OpenDoors", false);
        DoorAnimator.SetBool("OpenDoors", true);
    }
}
