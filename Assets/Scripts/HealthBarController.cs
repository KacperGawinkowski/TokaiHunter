using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public void SetHeatlhBar(float amount)
    {

        transform.GetChild(1).GetComponent<Image>().fillAmount = amount;
    }
}
