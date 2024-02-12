using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHilObject : MonoBehaviour
{
    public GameObject[] hils;
    public Transform spawnPoint;


    private void Start()
    {
        int randomHils = Random.Range(0, hils.Length);
        GameObject Hils = hils[randomHils];
        Instantiate(Hils, spawnPoint.position, Quaternion.identity);///
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<SpawnPlayers>().SpawnHilObj();
        }
        Destroy(gameObject);
    }
}
