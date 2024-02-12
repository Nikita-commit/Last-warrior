using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
    float speed;
    public float spd;
    Rigidbody2D rb;

    bool facingRight = true;

    bool isGrounded;
    public Transform groundChek;
    public float checkRadius;
    public LayerMask whatIsGrounded;
    public float jumpForce;

    //-----------------------------------Аудио
    Animator anim;
    AudioSource source;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip damageSound;
    public AudioClip damageSound2;
    public AudioClip damageShieldSound;
    public AudioClip heartSound;
    public AudioClip shieldSound;
    public AudioClip teleportSound;

    public int health;

    public float timeBetweenAttack;
    float nextAttackTime;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;
    public int damage;


    public GameObject StartEffect;
    public Transform StartEffectPos;
    public GameObject blood;
    private Transform posPlayer;
    //-----------------------------------Телепорт/Возрождение
    public float distance;
    private Transform teleport1;
    private Transform teleport2;
    private Transform teleport3;
    private Transform teleport4;
    private Transform spawnP;
    //-----------------------------------
    PhotonView view;
    private CinemachineVirtualCamera vcam;
    //-----------------------------------Щит
    public GameObject shield;
    public Shield shieldTimer;
    [HideInInspector] public SpawnPlayers i;

    public Text nickName;

    public GameObject healthEffect;
    public GameObject shieldEffect;

    [HideInInspector] public PostProcessVolume _PostProcessVolume;
    private Vignette _Vignette;

    private int countPlayerObj;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        posPlayer = gameObject.transform;
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        
        if (view.Owner.IsLocal) vcam.m_Follow = posPlayer;
        

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        teleport1 = GameObject.FindGameObjectWithTag("Teleport1").GetComponent<Transform>();
        teleport2 = GameObject.FindGameObjectWithTag("Teleport2").GetComponent<Transform>();
        teleport3 = GameObject.FindGameObjectWithTag("Teleport3").GetComponent<Transform>();
        teleport4 = GameObject.FindGameObjectWithTag("Teleport4").GetComponent<Transform>();

        Instantiate(StartEffect, StartEffectPos.position, Quaternion.identity);

        i = GameObject.Find("SpawnPlayers").GetComponent<SpawnPlayers>();
        shieldTimer = i.ShieldTimerImage;

        nickName.text = view.Owner.NickName;

        spawnP = GameObject.FindGameObjectWithTag("point").GetComponent<Transform>();

        _PostProcessVolume = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<PostProcessVolume>();
        _PostProcessVolume.profile.TryGetSettings(out _Vignette);

        Invoke("countPlayers", 3f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (view.IsMine)
        {
            if (collision.tag == "Teleport1")
            {
                if (Vector2.Distance(transform.position, teleport1.transform.position) > distance)
                {
                    anim.SetBool("isTeleport", true);
                    source.clip = teleportSound;
                    source.Play();
                    StartCoroutine(TeleportPortal1());
                }
            }

            if (collision.tag == "Teleport2")
            {
                if (Vector2.Distance(transform.position, teleport2.transform.position) > distance)
                {
                    anim.SetBool("isTeleport", true);
                    source.clip = teleportSound;
                    source.Play();
                    StartCoroutine(TeleportPortal2());
                }
            }

            if (collision.tag == "Teleport3")
            {
                if (Vector2.Distance(transform.position, teleport3.transform.position) > distance)
                {
                    anim.SetBool("isTeleport", true);
                    source.clip = teleportSound;
                    source.Play();
                    StartCoroutine(TeleportPortal3());
                }
            }

            if (collision.tag == "Teleport4")
            {
                if (Vector2.Distance(transform.position, teleport4.transform.position) > distance)
                {
                    anim.SetBool("isTeleport", true);
                    source.clip = teleportSound;
                    source.Play();
                    StartCoroutine(TeleportPortal4());
                }
            }

        }

        if (view.Owner.IsLocal)
        {
            if (collision.tag == "Shield")
            {
                _Vignette.color.Override(Color.blue);
                Invoke("Vignette", 0.3f);
                FindObjectOfType<CameraShake>().Shake();
                source.clip = shieldSound;
                source.Play();

                if (!shield.activeInHierarchy)
                {
                    if (view.IsMine) shield.SetActive(true);//////
                    shieldTimer.gameObject.SetActive(true);
                    shieldTimer.isCooldown = true;
                    Destroy(collision.gameObject);
                }
                else
                {
                    shieldTimer.ResetTimer();
                    if (view.IsMine) shield.SetActive(false);//////
                    Destroy(collision.gameObject);
                }

            }

            else if (collision.tag == "Health")
            {
                FindObjectOfType<CameraShake>().Shake();
                source.clip = heartSound;
                source.Play();
                if (health < 5)
                {
                    health += 1;
                }
                Destroy(collision.gameObject);
            }

            else if (collision.tag == "trap")
            {
                Instantiate(blood, StartEffectPos.position, Quaternion.identity);
                source.clip = damageSound;
                source.Play();
                transform.position = new Vector2(spawnP.position.x, spawnP.position.y);
                health -= damage;
                _Vignette.color.Override(Color.red);//////
                Invoke("Vignette", 0.15f);
            }

            else if (collision.tag == "Thorn")
            {
                FindObjectOfType<CameraShake>().Shake();
                rb.velocity = Vector2.up * 10;
                damage = 1;
                TakeDamage(damage);
                source.clip = damageSound;
                source.Play();
                _Vignette.color.Override(Color.red);//////
                Invoke("Vignette", 0.15f);
            }
        }
    }

    void Vignette()
    {
        _Vignette.color.Override(Color.black);
    }

    IEnumerator TeleportPortal1()
    {
        yield return new WaitForSeconds(0.51f);
        if (view.IsMine)
        {
            transform.position = new Vector2(teleport2.position.x, teleport2.position.y);
            anim.SetBool("isTeleport", false);
        }
    }

    IEnumerator TeleportPortal2()
    {
        yield return new WaitForSeconds(0.51f);
        if (view.IsMine)
        {
            transform.position = new Vector2(teleport1.position.x, teleport1.position.y);
            anim.SetBool("isTeleport", false);
        }
    }

    IEnumerator TeleportPortal3()
    {
        yield return new WaitForSeconds(0.51f);
        if (view.IsMine)
        {
            transform.position = new Vector2(teleport4.position.x, teleport4.position.y);
            anim.SetBool("isTeleport", false);
        }
    }

    IEnumerator TeleportPortal4()
    {
        yield return new WaitForSeconds(0.51f);
        if (view.IsMine)
        {
            transform.position = new Vector2(teleport3.position.x, teleport3.position.y);
            anim.SetBool("isTeleport", false);
        }
    }

    public void Right()
    {
        if (view.Owner.IsLocal) speed = spd;   
    }

    public void Left()
    {
        if (view.Owner.IsLocal) speed = -spd;
    }

    public void Stop()
    {
        if (view.Owner.IsLocal) speed = 0f;
    }

    public void Attack()
    {
        if (view.IsMine)
        {
            anim.SetTrigger("attack");
        }
        FindObjectOfType<CameraShake>().Shake();
        source.clip = attackSound;
        source.Play();
        nextAttackTime = Time.time + timeBetweenAttack;
    }
    
    public void AttackPlayer()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D col in enemiesToDamage)
        {
            col.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    public void Jump()
    {
        if (isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            source.clip = jumpSound;
            source.Play();
        }/*
        if (view.Owner.IsLocal)
        {
            if (isGrounded == true)
            {
                rb.velocity = Vector2.up * jumpForce;
                source.clip = jumpSound;
                source.Play();
            }
        }
        if (isGrounded == true)
        {
            source.clip = jumpSound;
            source.Play();
        }*/
    }

    public void countPlayers()
    {
        //countPlayerObj = PhotonNetwork.CurrentRoom.PlayerCount;
        Invoke("countInvoke", 0.1f);
    }

    void countInvoke()
    {
        countPlayerObj = PhotonNetwork.CurrentRoom.PlayerCount;
        countPlayers();
    }

    public void Update()
    { 

        if (view.IsMine)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);

            if (speed > 0 && facingRight == false)
            {
                Flip();

            }
            else if (speed < 0 && facingRight == true)
            {
                Flip();

            }

            if (speed != 0)
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }

            isGrounded = Physics2D.OverlapCircle(groundChek.position, checkRadius, whatIsGrounded);


            if (isGrounded == true)
            {
                anim.SetBool("isJumping", false);
            }
            else
            {
                anim.SetBool("isJumping", true);
            }

            if (health == 0)
            {
                i.DeathPanel();
                PhotonNetwork.LeaveRoom();
            }

        }

       

        if (countPlayerObj == 1)
        {
            i.WinPanel();
        }
        /*
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            i.WinPanel();
        }*/
    }

    public void Flip()
    {
        if (view.IsMine)
        { 
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            facingRight = !facingRight;
            nickName.transform.Rotate(0f, 180f, 0f);
        }
    }

    public void TakeDamage(int damage)
    {
        
            if (!shield.activeInHierarchy)
            {
                
                Instantiate(blood, StartEffectPos.position, Quaternion.identity);

                if (view.Owner.IsLocal)
                {
                    _Vignette.color.Override(Color.red);
                    Invoke("Vignette", 0.15f);      
                    health -= damage;
                    print(health);
                    source.clip = damageSound2;
                    source.Play();
                }
            }

            else if (shield.activeInHierarchy && damage > 0)
            {
                if (view.Owner.IsLocal)
                {
                    shieldTimer.ReduceTime(damage);
                    source.clip = damageShieldSound;
                    source.Play();
                }
            }
        
            /*
            if (health == 0)
            {
                i.DeathPanel();
                PhotonNetwork.LeaveRoom();
            }*/
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public static implicit operator PlayerController(Photon.Realtime.Player v)
    {
        throw new NotImplementedException();
    }
}