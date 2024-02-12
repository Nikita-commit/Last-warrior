using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainManu : MonoBehaviourPunCallbacks
{
    public InputField roomInputField;
    public GameObject lobbiPanel;
    public GameObject roomPanel;
    public Text roomName;
    public InputField nameInput;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton;
    public GameObject panel1;
    public GameObject panel2;

    [HideInInspector] public int countPlayer;
    public Text countPlayerText;
    private int randlevel;


    AudioSource source;
    public AudioClip btnSound;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
        Invoke("panelDestroy", 1.31f);

        randlevel = Random.Range(2, 4);

        nameInput.text = PlayerPrefs.GetString("name");


        source = GetComponent<AudioSource>();
    }
    void panelDestroy()
    {
        panel1.SetActive(false);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }

        countPlayer = PhotonNetwork.CurrentRoom.PlayerCount;

        countPlayerText.text = countPlayer.ToString() + "/5";

        if (PhotonNetwork.CurrentRoom.PlayerCount == 0)
        {
            Destroy(GameObject.FindWithTag("card"));
        }
        

    }

    public void OnClickPlayButton()
    {
        source.clip = btnSound;
        source.Play();
        Invoke("PlayGame", 1.01f);
        panel2.SetActive(true);
    }

    void PlayGame()
    {
        PhotonNetwork.LoadLevel("Level1");
    }

    public void OnClickCreate()
    {
        source.clip = btnSound;
        source.Play();
        panel2.SetActive(true);
        Invoke("Create", 1f);
    }

    void Create()
    {
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 5, BroadcastPropsChangeToAll = true });
        }
        if (nameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = nameInput.text;
            PlayerPrefs.SetString("name", nameInput.text);
        }

    }

    public override void OnJoinedRoom()
    {
        lobbiPanel.SetActive(false);
        roomPanel.SetActive(true);
        panel1.SetActive(true);
        panel2.SetActive(false);
        Invoke("panelDestroy2", 1.31f);
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    void panelDestroy2()
    {
        panel1.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }

    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        if (nameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = nameInput.text;
            PlayerPrefs.SetString("name", nameInput.text);
        }
    }

    public void OnClickLeaveRoom()
    {
        source.clip = btnSound;
        source.Play();
        panel2.SetActive(true);
        Invoke("leave", 1.01f);
    }

    void leave()
    {
        panel2.SetActive(false);
        panel1.SetActive(true);
        PhotonNetwork.LeaveRoom();
        Invoke("panelDestroy3", 1.31f);
    }

    void panelDestroy3()
    {
        panel1.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbiPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }

        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
}
