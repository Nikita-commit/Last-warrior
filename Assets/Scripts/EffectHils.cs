using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHils : MonoBehaviour
{
    public GameObject effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Instantiate(effect, gameObject.transform.position, Quaternion.identity);
        }
    }
}
