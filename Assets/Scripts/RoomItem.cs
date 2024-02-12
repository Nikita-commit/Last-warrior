using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    public Text count;
    MainManu manager;

    AudioSource source;
    public AudioClip btnSound;

    PhotonView view;

    private void Start()
    {
        manager = FindObjectOfType<MainManu>();
        view = GetComponent<PhotonView>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (view.IsMine) count.text = manager.countPlayer.ToString() + "/5";
        Debug.Log(count);
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;   
    }

    public void OnClickItem()
    {
        source.clip = btnSound;
        source.Play();
        manager.JoinRoom(roomName.text);
    }
}
