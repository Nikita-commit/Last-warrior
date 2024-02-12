using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    public GameObject panel;
    public GameObject panelKillEveryone;

    public GameObject SpawnHilObject;
    private Transform spawnPoint;
    public Transform[] spawnHilPoints;

    public Shield ShieldTimerImage;

    public GameObject thorn;

    public GameObject panelDeath;
    public GameObject panelWin;
    public GameObject panelQuit;
    public GameObject effect;
    public GameObject resultPanel;

    AudioSource source;
    public AudioClip btnSound;

    PhotonView view;

    private void Start()
    {
        int randomNumber = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[randomNumber];
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);

        Invoke("panelDelate", 0.41f);
        Invoke("panelDelate2", 4.1f);
        Invoke("SpawnHilObj", 1f);
        Invoke("Thorn1", 1f);

        MusicTransition.instance.destroy();

        source = GetComponent<AudioSource>();

        view = GetComponent<PhotonView>();

        resultPanel.SetActive(false);
    }

    void panelDelate()
    {
        panel.SetActive(false);
    }

    void panelDelate2()
    {
        Destroy(panelKillEveryone);
    }

    public void SpawnHilObj()
    {
        Invoke("Spawn", 10f);
    }

    void Spawn()
    {
        int randomHilNumber = Random.Range(0, spawnHilPoints.Length);
        Transform spawnPointHil = spawnHilPoints[randomHilNumber];
        Instantiate(SpawnHilObject, spawnPointHil.position, Quaternion.identity);
    }

    public void Thorn1()
    {
        Invoke("Thorn2", 10f);
    }

    public void Thorn2()
    {
        thorn.SetActive(true);
        Invoke("Thorn3", 6.3f);
    }

    public void Thorn3()
    {
        thorn.SetActive(false);
        Invoke("Thorn1", 1f);
    }

    public void DeathPanel()
    {
        /*if (view.Owner.IsLocal)*/ panelDeath.SetActive(true);
    }

    public void WinPanel()
    {
        panelWin.SetActive(true);
    }

    public void ClickHome()
    {
        if (view.Owner.IsLocal)
        {
            source.clip = btnSound;
            source.Play();
            panelQuit.SetActive(true);
            PhotonNetwork.LeaveRoom();
            Invoke("scene", 1.2f);
        }
            
    }

    void scene()
    {
        MusicTransition.instance.instantiate();
        SceneManager.LoadScene("LoadingScene");
    }

    public void results()
    {
        resultPanel.SetActive(true);
    }

    public void close()
    {
        resultPanel.SetActive(false);
    }
}
