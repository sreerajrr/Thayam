using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance=null;
    public TextMesh PLayerstext;
    public string MyNickName;
    bool isConnectedToPhoton;
    bool isConnectedToRoom;
    public Player[] playersAvailableOnLoad;
    // Start is called before the first frame update
    void Awake(){
        if(Instance==null){
            Instance=this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        isConnectedToPhoton=false;
        isConnectedToRoom=false;
        PLayerstext.text=" ";
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void ConnectToPhoton(){
        persistantdatas.Instance.levelNum =3;
        PhotonNetwork.NickName="Srs"+UnityEngine.Random.Range(0,205);
        MyNickName=PhotonNetwork.NickName;
        PhotonNetwork.GameVersion="0.0.0";    //to set a player name
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();   
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master!");
        isConnectedToPhoton=true;
        int maxPL=persistantdatas.Instance.noOfPlayers;
        byte maxBp=(byte)maxPL;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = 
                new ExitGames.Client.Photon.Hashtable() { { "m", 0 } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties,maxBp);

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
        isConnectedToPhoton=false;
    }

    // public void OnClickConnectToRoom()
    // {
    //     if (!PhotonNetwork.IsConnected)
    //         return;
    //     PLayerstext.text+="ConnectedToMaster \n";
    //     // foreach (RoomInfo roon in PhotonNetwork.GetRoomList())
    //     // {
            
    //     // }
    //     //PhotonNetwork.CreateRoom("Peter's Game 1"); //Create a specific Room - Error: OnCreateRoomFailed
    //     //PhotonNetwork.JoinRoom("Peter's Game 1");   //Join a specific Room   - Error: OnJoinRoomFailed  
    //     PhotonNetwork.JoinRandomRoom();               //Join a random Room     - Error: OnJoinRandomRoomFailed  
    // }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        isConnectedToRoom=false;
        Debug.Log(message.ToString());
        //no room available
        //create a room (null as a name means "does not matter")
        int maxPL=persistantdatas.Instance.noOfPlayers;
        byte maxBp=(byte)maxPL;
        RoomOptions roomOptions = new RoomOptions();
        string[] csl=new string[1];
        csl[0]="m";
        roomOptions.CustomRoomPropertiesForLobby = csl;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "m", 0 } };
        roomOptions.MaxPlayers = maxBp;
        roomOptions.PlayerTtl = 150000;
        PhotonNetwork.CreateRoom(null, roomOptions);
        // ExitGames.Client.Photon.Hashtable CustomRoom = 
        //         new ExitGames.Client.Photon.Hashtable() { { "m", 0 } };
        // PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxBp ,
        //                                                 PlayerTtl = 10000});
        Debug.Log("Creating new room");
        PLayerstext.text+="Failed to join and Creating New room\n";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
    }

    public override void OnJoinedRoom()
    {
        isConnectedToRoom=true;
        base.OnJoinedRoom();
        playersAvailableOnLoad=PhotonNetwork.PlayerList;
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + 
            " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + 
            " | RoomName: " + PhotonNetwork.CurrentRoom.Name + 
            " Region: " + PhotonNetwork.CloudRegion);
            PLayerstext.text+="Master: " + PhotonNetwork.IsMasterClient +" \n";
            PLayerstext.text+=" Region: " + PhotonNetwork.CloudRegion +" \n";
            PLayerstext.text+=" Region: " +" \n";
        foreach(Player pl in PhotonNetwork.PlayerList)
        {
            PLayerstext.text+=pl.NickName+" "+PhotonNetwork.CurrentRoom.PlayerCount+"\n";
        }
        
        
        LoadLeveln();
        //if(PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name != "Network")
        //    PhotonNetwork.LoadLevel("Network");
        // if(PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name != "Main")
        //     
    }   
    public void setDebug(string s){
        PLayerstext.text+=s+"\n";
    }
    public void setDebug(string s,bool p){
        if(p)
            PLayerstext.text=s+"\n";
    }
    public void LoadLeveln(){
        if(PhotonNetwork.IsMasterClient && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "GameScreen")
            PhotonNetwork.LoadLevel("GameScreen");
    }
    public void OnLeavePressed(){
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        
    }
}
