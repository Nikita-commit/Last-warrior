using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonController : MonoBehaviour
{

    public void stop()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Stop();
    }
    public void left()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Left();
    }
    public void right()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Right();
    }
    public void attack()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Attack();
    }
    public void jump()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Jump();
    }
}
