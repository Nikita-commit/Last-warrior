using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;


public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text playerName;

    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;

    Player player;

    private RipplePostProcessor camRipple;

    PhotonView view;

    AudioSource source;
    public AudioClip btnSound;

    private void Start()
    {
        camRipple = Camera.main.GetComponent<RipplePostProcessor>();
        view = GetComponent<PhotonView>();

        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = 0;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

        source = GetComponent<AudioSource>();

        //playerName.text = view.Owner.NickName;
    }

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(_player);
    }

    public void ApplyLocalChanges()
    { 
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
        gameObject.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
    }
    
    public void OnClickLeftArrow()
    {
        source.clip = btnSound;
        source.Play();
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        camRipple.RippleEffect();
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow()
    {
        source.clip = btnSound;
        source.Play();
        if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        camRipple.RippleEffect();
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
        }

    }
}
