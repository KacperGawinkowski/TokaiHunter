using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class FPSCharacterController : MonoBehaviour
{
    private Transform player;
    private CharacterController characterController;
    private Animator anim;

    [Header("Basic Camera Options")]
    [SerializeField] GameObject camera;
    [SerializeField] private float mouseSens;

    [Header("Camera Rotation Options")]
    [SerializeField] private float yCameraRotation;
    [SerializeField] private float zCameraRotation;

    [Header("Player Movement Options")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float dashLength;
    [SerializeField] private float dashCooldown;

    [Header("Player Atack Options")] 
    [SerializeField] private GameObject shootPrefab;
    [SerializeField] private float atackTimer;
    [SerializeField] private int dmg;
    [SerializeField] public UnityEvent ability = new UnityEvent();
    
    
    
    [Header("Player Rest")] 
    [SerializeField] private int MaxHP = 100;
    [SerializeField] private int CurHP = 100;

    [Header("Audio")]
    [SerializeField] private AudioSource atackAudio;
    [SerializeField] private AudioSource DashSound;
    [SerializeField] private AudioSource GateSound;
    [SerializeField] private AudioSource sadSound;

    //Ballsiak opcje do powyższych rzeczy bols
    private float xRot;
    private float jumpSlow;
    private bool jumpBool = false;
    private bool dashBool;
    private bool jumpOffPerk;
    private bool isDashOnCooldown;
    private Vector3 velocity;
    private bool canShoot = true;
    private bool teleportPerkOn;
    private GameObject prevDoors;
    private bool perkSelected = true;
    private bool doorsOpen;
    private int dmgMultiplier=1;
    private int dmgMul = 1;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //lock cursora zeby nie latał wszędzie
        player = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraController();
        JumpController();
        AtackController();
        DashController();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<Perks>().ApplyPerk(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FindObjectOfType<Perks>().ApplyPerk(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FindObjectOfType<Perks>().ApplyPerk(8);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray raycast = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(raycast, out hit, 3, transform.gameObject.layer))
            {
                if (hit.transform.gameObject.tag == "Gate")
                {
                    if (perkSelected && doorsOpen == false) //perk zebrany
                    {
                        Debug.Log(hit.transform.gameObject.name);
                        prevDoors = hit.transform.gameObject;
                        hit.transform.GetComponent<DoorController>().OpenDoors();
                        FindObjectOfType<DungeonGenerator>().CreateNextRoom();
                        doorsOpen = true;
                        perkSelected = false;
                        GateSound.Play();
                    }
                }
                else if (hit.transform.gameObject.tag == "Gate2")
                {
                    if(doorsOpen) //perk zebrany
                    {
                        Debug.Log(hit.transform.gameObject.name);
                        hit.transform.GetComponent<DoorController>().OpenSecondDoors(prevDoors.transform.parent.GetComponent<Animator>());
                        FindObjectOfType<DungeonGenerator>().SpawnEnemies();
                        GateSound.Play();
                        StartCoroutine(DestroyAnimator(prevDoors.transform.parent.GetComponent<Animator>()));
                    }
                }
                else if (hit.transform.gameObject.tag == "Perk")
                {
                    FindObjectOfType<Perks>().ApplyPerk(hit.transform.GetComponent<PiedesalController>().getPerkID());
                    perkSelected = true;
                    doorsOpen = false;
                    foreach (GameObject item in GameObject.FindGameObjectsWithTag("Perk"))
                    {
                        item.SetActive(false);
                    }
                    GameObject.Find("PerkAppear").GetComponent<AudioSource>().Play();
                }
            }
        }
        
        //ABILITY
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ability.Invoke();
        }
        
    }

    private IEnumerator DestroyAnimator(Animator bols)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(bols);
    }

    private void FixedUpdate()
    {
        MovementController();

    }

    private void CameraController()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.unscaledDeltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        camera.GetComponent<Transform>().localRotation = Quaternion.Euler(xRot, yCameraRotation, zCameraRotation);

        player.Rotate(Vector3.up * mouseX);
    }

    private void MovementController()
    {
        //WSAD MOVEMENT
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector3 velocityTemp = (player.forward * movementInput.y + player.right * movementInput.x);

        velocity.x = velocityTemp.x;
        velocity.z = velocityTemp.z;
        
        //JUMP
        if (jumpBool)
        {
            velocity.y = jumpHeight;
            jumpBool = false;
        }
        else
        {
            if (characterController.isGrounded)
            {
                velocity.y = -0.1f;
            }
            else
            {
                velocity.y -= 7f * Time.fixedDeltaTime * 0.3f;
            }
        }

        characterController.Move(velocity * speed);

        if (dashBool)
        {
            if (teleportPerkOn == false)
            {
                characterController.Move(new Vector3(velocity.x,velocity.y,velocity.z) * dashLength * speed);
                dashBool = false;
            }
            else
            {
                float x = Random.Range(-2,2);
                float z = Random.Range(-2,2);
                characterController.Move(new Vector3(velocity.x*x*15,velocity.y,velocity.z*z*15) * dashLength * speed);
                dashBool = false;
            }
            DashSound.Play();
        }
    }

    private void JumpController()
    {
        if (characterController.isGrounded && Input.GetButtonDown("Jump") && jumpOffPerk == false)
        {
            jumpBool = true;
        }
    }

    private IEnumerator DashSpeedLines()
    {
        FindObjectOfType<CanvasController>().SetSpeedLinesOn(true);
        yield return new WaitForSeconds(0.25f);
        FindObjectOfType<CanvasController>().SetSpeedLinesOn(false);
    }

    private IEnumerator DashCooldown()
    {
        isDashOnCooldown = true;
        yield return new WaitForSeconds(dashCooldown);
        isDashOnCooldown = false;
    }
    
    private void DashController()
    {
        if (Input.GetButtonDown("Dash") && isDashOnCooldown == false)
        {
            dashBool = true;
            StartCoroutine(DashSpeedLines());
            StartCoroutine(DashCooldown());
        }
    }

    private void AtackController()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        anim.SetTrigger("attackRight");
        atackAudio.Play();
        Instantiate(shootPrefab, transform.position, camera.transform.rotation).GetComponent<BallsController>().dmg = 3 * dmgMul;
        
        //shootParticle.transform.forward = new Vector3(player.transform.forward.x, transform.right, player.transform.forward.z);
        
        //ShootParticle.GetComponent<Rigidbody>().AddForce(player.forward * Time.unscaledDeltaTime * 10000);
        
        yield return new WaitForSeconds(atackTimer);
        canShoot = true;
    }


    public void TakeDamage(int dmg)
    {
        CurHP -= (dmg * dmgMultiplier);
        sadSound.Play();
        FindObjectOfType<HealthBarController>().SetHeatlhBar((float)(CurHP)/(float)(MaxHP));

        if (CurHP <= 0)
        {
            Debug.Log("ZMARŁEŚ HAHAHAHHAHAHA EZ");
            Cursor.lockState = CursorLockMode.None;
            GameObject.Find("BolsPanel").transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
            Destroy(this);
        }
    }
    
    public void MoreHpNoDashPerk()
    {
        transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(
            transform.GetChild(0).GetChild(0).transform.localScale.x* 1.25f,
            transform.GetChild(0).GetChild(0).transform.localScale.y,
            transform.GetChild(0).GetChild(0).transform.localScale.z);
        
        speed -= 0.005f;
        speed = Mathf.Clamp(speed, 0.01f, 10f);
        
        jumpOffPerk = true;
        CurHP = CurHP + 40;
        MaxHP = MaxHP + 40;
    }

    public void TeleportPerk()
    {
        teleportPerkOn = true;
    }

    public void HealToMax()
    {
        CurHP = MaxHP;
        FindObjectOfType<HealthBarController>().SetHeatlhBar((float)(CurHP)/(float)(MaxHP));
    }

    public void SlimShady()
    {
        transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(
            transform.GetChild(0).GetChild(0).transform.localScale.x* 0.75f,
            transform.GetChild(0).GetChild(0).transform.localScale.y,
            transform.GetChild(0).GetChild(0).transform.localScale.z);
        
        //speed += 0.025f;
        dmgMultiplier += 1;
    }

    public void SmallerPlayer()
    {
        speed -= 0.01f;
        speed = Mathf.Clamp(speed, 0.01f, 10f);
        
        float balls = transform.GetChild(0).transform.position.y - 0.075f;
        
        balls = Mathf.Clamp(balls, -0.25f, 1f);
        
        transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x,balls, transform.GetChild(0).transform.position.z);
    }

    public void ThirdEye()
    {
        transform.GetChild(0).GetComponent<Camera>().fieldOfView += 30;
        transform.GetChild(0).GetComponent<Camera>().fieldOfView =
            Mathf.Clamp(transform.GetChild(0).GetComponent<Camera>().fieldOfView, 20, 150);
    }
    
    public void UltimatePower()
    {
        atackTimer = atackTimer * 2;
        dmgMul += 1;
    }
}
