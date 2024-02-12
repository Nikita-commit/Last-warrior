using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    GameObject clone;
    public GameObject lightningEffect;
    public Transform spawnEffect;

    private void Update()
    {

        Destroy(clone, 0.5f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        clone = Instantiate(lightningEffect, spawnEffect.position, Quaternion.identity);

    }
    /*
    public Transform tel;
    public GameObject player;
    public float distance;
    public float distance2;
    public GameObject lightningEffect;
    public Transform spawnEffect;
    GameObject clone;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {

            Destroy(clone, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            clone = Instantiate(lightningEffect, spawnEffect.position, Quaternion.identity);

            if (Vector2.Distance(transform.position, player.transform.position) > distance)
            {
                StartCoroutine(TeleportPortal());
                FindObjectOfType<Player>().portalTO();
            }
        }
    }

    IEnumerator TeleportPortal()
    {
        yield return new WaitForSeconds(0.51f);
        if (Vector2.Distance(transform.position, player.transform.position) < distance2)
        {
            player.transform.position = new Vector2(tel.position.x, tel.position.y);
            FindObjectOfType<Player>().portalFROM();
        }
        
    }
    ----------------------------------------------------------------------------------------------------------
    private Transform destination;
    public bool isteleport1;
    public float distance;

    void Start()
    {
        if(isteleport1 == false)
        {
            destination = GameObject.FindGameObjectWithTag("Teleport1").GetComponent<Transform>();
        }
        else
        {
            destination = GameObject.FindGameObjectWithTag("Teleport2").GetComponent<Transform>();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
      
        if(Vector2.Distance(transform.position, other.transform.position) > distance)
        {
            other.transform.position = new Vector2(destination.position.x, destination.position.y);
        }
    }*/
}
