using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class GameManagerGame : MonoBehaviourPunCallbacks
{
    public GameObject panel;
    private int MYindex;
    public int currentPlayerIndex;
    public GameObject Loading;
    public GameObject[] Players;
    public GameObject[] dieValuePosition;
    public static GameManagerGame Instance;
    public GameObject[] pBoards;
    private int currentNumberofPlayers;
    private int newBoardPostion;
    public GameController gamecontoller;
    private List<int> RemovedPlayer=new List<int>();
    // private List<byte> removedIds= new List<byte>();
    int noOfPlayers;
    int roomMax;

    // Start is called before the first frame update
    void Awake(){
        RemovedPlayer.Clear();
        currentPlayerIndex=0;
        if(Instance==null){
            Instance=this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start(){
        int i =0;
        
        int masterIndex=0;
        int levelNumber=GameController.gameLevel;
        noOfPlayers=GameController.noOfPlayers;
         if(levelNumber==3||levelNumber==4){
             Loading.SetActive(true);
            roomMax=PhotonNetwork.CurrentRoom.MaxPlayers;
            foreach (GameObject board in pBoards)
            {
                board.SetActive(false);
                pBoards[0].GetComponent<playerBoard>().TurnOFFButton();
            }
            i=0;
            foreach(Player pl in PhotonNetwork.PlayerList)
            {
                if(pl.NickName==NetworkManager.Instance.MyNickName){
                    MYindex=i;
                    pBoards[0].SetActive(true);
                    pBoards[0].GetComponent<playerBoard>().setText(pl.NickName.ToString());
                }
                i++;
            }

            NetworkManager.Instance.setDebug("myIndex= "+MYindex);
            for(i=1;i<=MYindex;i++){
                
                pBoards[noOfPlayers-i].SetActive(true);
                pBoards[noOfPlayers-i].GetComponent<playerBoard>().
                        setText(PhotonNetwork.PlayerList[MYindex-i].NickName.ToString());
                if(MYindex-i==0)
                    pBoards[noOfPlayers-i].GetComponent<playerBoard>().TurnOnOnlyAnimation();
            }
            if(PhotonNetwork.IsMasterClient){
                pBoards[0].GetComponent<playerBoard>().TurnOnOnlyAnimation();
                //pBoards[0].GetComponent<playerBoard>().canTouch(true);
            }
            currentNumberofPlayers=MYindex;
            Debug.Log("playerCount1 - "+noOfPlayers);
            Debug.Log("playerCount2 - "+PhotonNetwork.CurrentRoom.PlayerCount);
            currentPlayerIndex=noOfPlayers-MYindex;//board Idex
            currentPlayerIndex=currentPlayerIndex==noOfPlayers?0:currentPlayerIndex;
            if(PhotonNetwork.CurrentRoom.PlayerCount==noOfPlayers){
                
                StratGame();
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {   
        // if(PhotonNetwork.IsMasterClient){
            // NetworkManager.Instance.setDebug("i am master   " +roomMax,true);
        // }
            
    }
    void StratGame(){
        NetworkManager.Instance.PLayerstext.text="";
        PhotonNetwork.CurrentRoom.SetCustomProperties
                    (new ExitGames.Client.Photon.Hashtable {{"m", 1}});
        Destroy(Loading.gameObject);
        if(PhotonNetwork.IsMasterClient){
                pBoards[0].GetComponent<playerBoard>().canTouch(true);
            }
    }
    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void SendDieValue(int dValue){
        if(currentPlayerIndex!=0) return;
        string dv= GameController.dievalue.Count+"-"+dValue;
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RecieveDieValue", RpcTarget.All, dv);
    }
    [PunRPC]
     public void RecieveDieValue(string dValue){
         
        if(currentPlayerIndex!=0){
            string[] dv = dValue.Split('-');
            GameController.dievalue.Add(int.Parse(dv[1]));
            panel.GetComponent<PanelScript>().addOpp(currentPlayerIndex,int.Parse(dv[1]));
            gamecontoller.GetComponent<GameController>().resetTable();
        }
     }
    public void RemoveDieValue(int dValue){
        if(currentPlayerIndex!=0)return;
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RecieveDieValueToRemove", RpcTarget.All, dValue);
    }
    [PunRPC]
     public void RecieveDieValueToRemove(int dValue){
         
        if(currentPlayerIndex!=0){
            GameController.dievalue.Remove(dValue);
            gamecontoller.GetComponent<GameController>().resetTable();
        }
    }
    public void Movement(int PawnNo,int toMove,bool move5){
        if(currentPlayerIndex!=0)return;
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("moveOtherPlayer", RpcTarget.All, PawnNo,toMove,move5);
    }
    [PunRPC]
    void moveOtherPlayer(int PawnNo,int toMove,bool move5){
        if(currentPlayerIndex!=0){
            Players[(currentPlayerIndex*5)+PawnNo].GetComponent<pawnScript>().
                                MoveRequestFromAI(toMove,dieValuePosition[
                                GameController.dievalue.IndexOf(toMove)],move5);
        }
    }
    public void callNextPlayer(){
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("nextPlayer", RpcTarget.All, PhotonNetwork.NickName);
    }
    [PunRPC]
    void nextPlayer(string a){
        gamecontoller.GetComponent<GameController>().next();
        currentPlayerIndex= currentPlayerIndex==noOfPlayers-1?0:currentPlayerIndex+1;
        while(RemovedPlayer.Contains(currentPlayerIndex)){
            currentPlayerIndex=currentPlayerIndex==noOfPlayers-1?0:currentPlayerIndex+1;
            gamecontoller.GetComponent<GameController>().next();
        }
        // NetworkManager.Instance.setDebug(" "+currentPlayerIndex);
        if(currentPlayerIndex==0){
            // canmove
            if(GameController.isAI){
                GameController.isAiWorking = true;
                gamecontoller.GetComponent<GameController>().callNextPlayer();
            }
            foreach(GameObject g in dieValuePosition){
                g.GetComponent<PositionHolderTouch>().canTouchthis(true);
            }
            // NetworkManager.Instance.setDebug("canMoveNow");
            pBoards[0].GetComponent<playerBoard>().TrunONButton();

        }
        else{
            pBoards[currentPlayerIndex].GetComponent<playerBoard>().TurnOnOnlyAnimation();
        }
        
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)	{
        newBoardPostion=newBoardPostion+1;
        if(RemovedPlayer.Count>0){
            newBoardPostion=RemovedPlayer[0];
            RemovedPlayer.Remove(newBoardPostion);
        }
        pBoards[newBoardPostion].SetActive(true);
        pBoards[newBoardPostion].GetComponent<playerBoard>().
            setText(PhotonNetwork.PlayerList[currentNumberofPlayers+newBoardPostion].NickName.ToString());
        if(PhotonNetwork.CurrentRoom.PlayerCount==noOfPlayers){
            StratGame();
        }
        
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer){
        roomMax=PhotonNetwork.CurrentRoom.PlayerCount;
        NetworkManager.Instance.setDebug("Player Left"+otherPlayer.NickName);
        Debug.Log("player to remove="+otherPlayer.NickName);
        if(PhotonNetwork.CurrentRoom.PlayerCount==1){
            NetworkManager.Instance.setDebug("Only Man standing..YOU won");
            Debug.Log("last Man");
        }
        else{
            Debug.Log("No of Players left "+PhotonNetwork.CurrentRoom.PlayerCount);
            NetworkManager.Instance.setDebug("No of Players left "+
                                            PhotonNetwork.CurrentRoom.PlayerCount);
        }
        int i=0;
        foreach (GameObject pb in pBoards)
        {
            if(pb.GetComponent<playerBoard>().getText()!=null &&
                string.Compare(pb.GetComponent<playerBoard>().getText(),otherPlayer.NickName)==0){
                RemovedPlayer.Add(i);
                pb.SetActive(false);
                pb.GetComponent<playerBoard>().setText(null);
                if(i==currentPlayerIndex && PhotonNetwork.IsMasterClient){
                    callNextPlayer();
                }

            }
            i++;
        }
    }
    void OnApplicationQuit(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
        }
        callNextPlayer();
        NetworkManager.Instance.OnLeavePressed();
    }


}
