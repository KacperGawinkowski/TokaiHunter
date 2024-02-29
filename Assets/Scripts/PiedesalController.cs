using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PiedesalController : MonoBehaviour
{
    private TextMeshPro perkDesc;
    [SerializeField] private int PerkID;
    
    void Start()
    {
        perkDesc = transform.GetChild(0).GetComponent<TextMeshPro>();
        PerkID = Random.Range(1, FindObjectOfType<Perks>().GetAmoutOfPerks());
        perkDesc.text = FindObjectOfType<Perks>().GetPerkDesc(PerkID);
    }
    

    public int getPerkID()
    {
        return PerkID;
    }

    
}
