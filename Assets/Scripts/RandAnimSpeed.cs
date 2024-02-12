using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandAnimSpeed : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(0.75f, 1.25f);
    }

}
