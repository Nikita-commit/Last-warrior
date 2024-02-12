using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject spriteMask;
    public GameObject Panel1;
    public GameObject Panel2;
    public GameObject effect;
    public Transform spawnEffect;
    public GameObject ButtonConnect;

    AudioSource source;
    public AudioClip btnSound;

    public GameObject textObj;
    private bool internetBool;
    public GameObject btn;

    private void Start()
    {
        Invoke("PanelFalse", 1.31f);
        source = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) internetBool = false;
        else internetBool = true;
    }

    public void OnClickConnect()
    {
        source.clip = btnSound;
        source.Play();
        Instantiate(effect, spawnEffect.position, Quaternion.identity);
        ButtonConnect.SetActive(false);
        FindObjectOfType<CameraShake>().Shake();
        Invoke("StartConnect", 2.01f);
        Invoke("PanelFalse2", 1f);
        spriteMask.SetActive(true);
    }

    void StartConnect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        if (internetBool == false)
        {
            textObj.SetActive(true);
            btn.SetActive(true);
        }
    }

    void PanelFalse()
    {
        Destroy(Panel1);
    }
    void PanelFalse2()
    {
        Panel2.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void restart()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
