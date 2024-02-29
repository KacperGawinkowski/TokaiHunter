using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Perks : MonoBehaviour
{
    private FPSCharacterController cc;
    private int amountOfPerks = 9;
    [SerializeField] private GameObject HealLight;
    [SerializeField] private GameObject ExplosionSystem;
    
    
    private void Start()
    {
        cc = FindObjectOfType<FPSCharacterController>();
    }

    public void ApplyPerk(int id)
    {
        switch (id)
        {
            case 1:
                cc.MoreHpNoDashPerk();
                break;
            
            case 2:
                cc.TeleportPerk();
                break;
            
            case 3:
                cc.ability.AddListener(Heal);
                break;
            
            case 4:
                cc.ability.AddListener(Explosion);
                break;
            
            case 5:
                Time.timeScale = Time.timeScale * 1.5f;
                break;
            
            case 6:
                cc.SlimShady();
                break;
            
            case 7:
                cc.SmallerPlayer();
                break;
            
            case 8:
                cc.ThirdEye();
                break;
            
            case 9:
                cc.UltimatePower();
                break;
        }
    }

    private void Heal()
    {
        StartCoroutine(HealEffect());
        cc.HealToMax();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            item.GetComponent<EnemyHp>().TakeDamage(-6);
        }
        
        cc.ability.RemoveListener(Heal);
    }

    private IEnumerator HealEffect()
    {
        Instantiate(HealLight, new Vector3(transform.position.x, transform.position.y + 16, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        //Destroy(healL);
    }
    
    private void Explosion()
    {
        Instantiate(ExplosionSystem,transform.position,Quaternion.identity);
        cc.TakeDamage(40);
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            item.GetComponent<EnemyHp>().TakeDamage(20);
        }
        
        cc.ability.RemoveListener(Explosion);
    }

    public String GetPerkDesc(int id)
    {
        switch (id)
        {
            case 1:
                return "Iron Skin \n You are too heavy to jump";

            case 2:
                return "Teleportation \n Except, you don't know where";
            
            case 3:
                return "Heal Spell \n You heal every being";
            
            case 4:
                return "Explosion Spell \n You explode with great power";
            
            case 5:
                return "Super-speed \n But actually everything has super-speed";
            
            case 6:
                return "Incredibly Slim \n But also weak";
            
            case 7:
                return "Minimization \n Your legs are shorter, so you'll move slower";
            
            case 8:
                return "Additional Eye \n Now you'll see more";

            case 9:
                return "Ultimate Power \n Inhuman magic knowledge, but its hard to cast";
            
            default:
                return " ";
        }
    }


    public int GetAmoutOfPerks()
    {
        return amountOfPerks+1;
    }
    

    
}
