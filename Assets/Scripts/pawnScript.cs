using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawnScript : MonoBehaviour
{
    public GameObject[] buddies;
    public GameController gamecontoller;
    private BoxCollider boxcollider;

    //public float timegap;
    public int currentPosition ;
    public int positionLevel ;

    int myNumber;

    public GameObject[] homes;

    enum Directions {Up,Left,Right,Down};
    Directions myDirection;
    int startPos,endPos;
    public Transform[] wayPoints;
    public static bool finishedmoving;

    private Animator anim;
    List<GameObject>  collidedObjectsList = new List<GameObject>();
    int NumOfOtherPlayers;
    int otherPlayerCount;
    private int tilesToMove;
    GameObject valueHolder;
    [SerializeField]
    private bool finishedmove;
    private bool move5;
    private bool AnimationPlaying;
    public int toRemoveTile; 
   bool touchedThis;
   Vector3[] PawnWayPosition;
   bool movementPosible;
   public int force=100;
//    int mHelp=0;
    void Start()
    { 
        toRemoveTile=0;
        boxcollider= GetComponent<BoxCollider>();
        finishedmoving=true;
        myDirection=Directions.Right;
        AnimationPlaying=false;
        finishedmove=false;
        NumOfOtherPlayers=0;
        otherPlayerCount=0;
        tilesToMove=0;
        anim = gameObject.GetComponent<Animator>();
        myNumber=getPlayerNumber(this.gameObject);    
        positionLevel=0;
        touchedThis=false;
        boxcollider.enabled=false;
        // currentPosition=0;

       // anim.Play("New State");
        //anim.Stop("Animator");
        anim.GetComponent<Animator>().enabled = false;
        movementPosible=false;
    }
    void Update()
    {

        // if(movementPosible){
        //     float v3distance=Vector3.Distance(transform.position, target.position);
        //     float eachStep=v3distance/10;
        //     float step =  speed * Time.deltaTime; // calculate distance to move
        //     transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        //     float zvalue=-6.384f;
            
        //     // Check if the position of the cube and sphere are approximately equal.
        // if (Vector3.Distance(transform.position, target.position) >eachStep*9&&mHelp<=0)
        //     {this.transform.position = new Vector3(transform.position.x,
        //                                         64.0f,transform.position.z+0.10f);
        //     mHelp=1;
        //     }
                // movementPosible=false;
        // }
        // Move our position a step closer to the target.
        
    }
    private void moveToCenter(){
       // this.transform.position = wayPoints[currentPosition-1].transform.position;
    
        this.transform.position = new Vector3(
            wayPoints[positionLevel==1?currentPosition-1:53-currentPosition].transform.position.x,
            66.0f,
            wayPoints[positionLevel==1?currentPosition-1:53-currentPosition].transform.position.z+0.50f);
        boxcollider.enabled=true;
    }
    private void backToPosition(){
        boxcollider.enabled=false;
        this.transform.position = 
            wayPoints[positionLevel==1?currentPosition-1:53-currentPosition].transform.position;
    }

    // Update is called once per frame
   
    public void settilesToMove(int val){
        this.tilesToMove=val;
    }

    public void sendToHome(){
        if(getpositionLevel()==1) { // position level should be 1
            this.transform.position = homes[0].transform.position;
            this.setcurrentPosition(0);
            this.setpositionLevel(0);
        }
        else if(getpositionLevel()==3) {
            this.transform.position = homes[1].transform.position;
            setcurrentPosition(27);
            setpositionLevel(2);
        }
    }

    public void MoveRequestFromAI(int ttm,GameObject valHold,bool mov5){
        tilesToMove=ttm;
        move5=mov5;
        valueHolder=valHold;
        finishedmoving=true;
        OnMouseDown();
    }



    public void checkerMove(){
        tilesToMove = 1;
        move5=false;
        finishedmoving=true;
        valueHolder=null;
    }
    public void OnMouseDown(){     // if the pawn is touched, do this

        //valueHolder to change Color
        if(tilesToMove>0 && !finishedmove && finishedmoving){
            finishedmoving=false;
            touchedThis=true;
            toRemoveTile=tilesToMove;
            settilesToMove(0);
            gamecontoller.GetComponent<GameController>().stopAllAnimation();
            if(move5){
                //send move to Other mobiles
                if(GameController.gameLevel==3||GameController.gameLevel==4)
                    GameManagerGame.Instance.Movement(getPawnNumner(),toRemoveTile,true);
                // /////////////////////
                foreach(GameObject b in buddies){
                    b.GetComponent<pawnScript>().setcurrentPosition(
                        b.GetComponent<pawnScript>().getcurrentPosition()+1);
                    b.transform.position = wayPoints
                        [
                            b.GetComponent<pawnScript>().getpositionLevel()==0?
                            b.GetComponent<pawnScript>().getcurrentPosition()-1:
                            53-b.GetComponent<pawnScript>().getcurrentPosition()
                        ].transform.position;
                    b.GetComponent<pawnScript>().setpositionLevel(
                        b.GetComponent<pawnScript>().getpositionLevel()==0?1:3);
                }
                
                endOfMovement();
            }
            else{
                //send move to Other mobiles
                
                // /////////////////////
                if(getcurrentPosition()+toRemoveTile < 27){
                    setcurrentPosition(getcurrentPosition()+toRemoveTile);
                    if(GameController.gameLevel==3||GameController.gameLevel==4)
                        GameManagerGame.Instance.Movement(getPawnNumner(),toRemoveTile,false);
                    
                    movethis(currentPosition-toRemoveTile-1,currentPosition-1);
                    
                    //this.transform.position = wayPoints[currentPosition-1].transform.position;
                    setpositionLevel(1);
                }
                else if(getcurrentPosition()+toRemoveTile == 27){
                    setcurrentPosition(getcurrentPosition()+toRemoveTile);
                    if(GameController.gameLevel==3||GameController.gameLevel==4)
                        GameManagerGame.Instance.Movement(getPawnNumner(),toRemoveTile,false);
                    movethis(currentPosition-1-toRemoveTile,currentPosition-1);
                    // this.transform.position = homes[1].transform.position;
                    // setpositionLevel(2);
                }
                else if(getcurrentPosition()+toRemoveTile > 27 && getcurrentPosition()+toRemoveTile <54 
                        && (getpositionLevel()==2|| getpositionLevel()==3)){
                        setcurrentPosition(getcurrentPosition()+toRemoveTile);
                        if(GameController.gameLevel==3||GameController.gameLevel==4)
                        GameManagerGame.Instance.Movement(getPawnNumner(),toRemoveTile,false);
                    movethis(53-currentPosition+toRemoveTile,53-currentPosition);
                    //this.transform.position = wayPoints[53-currentPosition].transform.position;
                    setpositionLevel(3);
                }
                else if(getcurrentPosition()+toRemoveTile == 54){
                    setcurrentPosition(getcurrentPosition()+toRemoveTile);
                    if(GameController.gameLevel==3||GameController.gameLevel==4)
                        GameManagerGame.Instance.Movement(getPawnNumner(),toRemoveTile,false);
                    movethis(53-currentPosition+toRemoveTile,-1);
                    // this.transform.position = homes[0].transform.position;
                    // setpositionLevel(0);
                    finishedmove=true;
                }
            }
        }
        else{
            settilesToMove(0);

        }
    }

    public void endOfMovement(){
        bool canCancel=checkForExchange();
        Debug.Log("can Cancel");
        if(GameController.gameLevel==3||GameController.gameLevel==4){
            if(GameManagerGame.Instance.currentPlayerIndex==0)
            GameController.dievalue.Remove(toRemoveTile);}
        else
            GameController.dievalue.Remove(toRemoveTile);
        ////////////////////////////////////////
        if(GameController.gameLevel==3||GameController.gameLevel==4)
            GameManagerGame.Instance.RemoveDieValue(toRemoveTile);
        //////////////////////////////////////////
        gamecontoller.GetComponent<GameController>().NoMoreAvailablePlays(myNumber);
        valueHolder.GetComponent<PositionHolderTouch>().resetIt();
        valueHolder.SetActive(false);
        valueHolder.gameObject.transform.GetChild(0).gameObject.transform.GetChild(toRemoveTile==10 ? 5:toRemoveTile-1).gameObject.SetActive(false);
        valueHolder.gameObject.transform.GetChild(1).gameObject.transform.GetChild(toRemoveTile==10 ? 5:toRemoveTile-1).gameObject.SetActive(false);
        toRemoveTile=0;
        gamecontoller.GetComponent<GameController>().resetTable();
        if(!canCancel){
            finishedmoving=true;
            if(GameController.templePoints.Contains(positionLevel==1?currentPosition-1:53-currentPosition))
                gamecontoller.GetComponent<GameController>().playTempleReachedAudio();
            else{
                gamecontoller.GetComponent<GameController>().playMovementAudio();
            }
            if(currentPosition == 27){
                
            }
        }
        else{
            gamecontoller.GetComponent<GameController>().playKillAudio();
        }
        if(GameController.currentAiPlayer==getPlayerNumber(this.gameObject) || 
                            (
                                (GameController.gameLevel == 3 || GameController.gameLevel == 4) && GameController.isAI
                            ))
                            {
            gamecontoller.GetComponent<GameController>().moveit();
        }
    }
    private void movethis(int startP,int endP){
        startPos=startP;
        endPos=endP;
        if((startPos-endPos==1||startPos-endPos==-1)&&!(endPos==26||endPos==-1)){
            
        if(GameController.templePoints.Contains(
                                    positionLevel==1||positionLevel==0?startPos+1:startPos-1)){
            PawnWayPosition=wayPoints[positionLevel==1||positionLevel==0?startPos+1:startPos-1].
            gameObject.GetComponent<orgainsePlayers>().organiseForNextMove(this.gameObject);
        }
            else
            {
                PawnWayPosition=wayPoints[positionLevel==1||positionLevel==0?startPos+1:startPos-1].
                gameObject.GetComponent<NontempleScript>().organiseForNextMove(this.gameObject);
            }
            this.transform.localScale=PawnWayPosition[1];
        } 
        if(GameController.currentAiPlayer==getPlayerNumber(this.gameObject)){
            InvokeRepeating("moveit",0.5F,0.3F);
        }
        else
        {
            InvokeRepeating("moveit",0.3F,0.3F);
        }
    }
     
    void moveit(){
        if((endPos==26||endPos==-1)&&(startPos-endPos==1||endPos-startPos==1)){
            this.transform.position = homes[endPos==26?1:0].transform.position;
            setpositionLevel(endPos==26?2:0);
            CancelInvoke("moveit");
            endOfMovement();
        }
        else
        {
            // try{
            //     // Vector3 relativePos = transform.position - PawnWayPosition[0];
            //     // gameObject.GetComponent<Rigidbody>().AddForce(100 * relativePos);
            //     this.transform.position=PawnWayPosition[0];
            //     Debug.Log("run this ->"+PawnWayPosition[0].x+" "+PawnWayPosition[0].y+" "+
            //                             PawnWayPosition[0].z);
            // }
            // catch{
            //     this.transform.position = wayPoints[startPos+1].transform.position;
            // }
            if((startPos-endPos==1||startPos-endPos==-1))
            {
                this.transform.position=PawnWayPosition[0];;
            }
            else{
                gamecontoller.GetComponent<GameController>().playMovementAudio();
                this.transform.position = new Vector3(
                    wayPoints[positionLevel==1||positionLevel==0?startPos+1:startPos-1].transform.position.x+0.065F,
                    64.0f,
                    wayPoints[positionLevel==1||positionLevel==0?startPos+1:startPos-1].transform.position.z+0.35F);
            }
        }
        if(positionLevel==1)
            startPos++;
        else if (positionLevel==3)
            startPos--;
        if(startPos==endPos){
            CancelInvoke("moveit");
            endOfMovement();
        }else
        {
            if((startPos-endPos==1||startPos-endPos==-1)&&!(endPos==26||endPos==-1)){
                
                if(GameController.templePoints.Contains(positionLevel==1?startPos+1:startPos-1))
                PawnWayPosition=wayPoints[positionLevel==1?startPos+1:startPos-1].gameObject.
                                GetComponent<orgainsePlayers>().organiseForNextMove(this.gameObject);
                else
                {
                    PawnWayPosition=wayPoints[positionLevel==1?startPos+1:startPos-1].gameObject.
                                GetComponent<NontempleScript>().organiseForNextMove(this.gameObject);
                }
                this.transform.localScale=PawnWayPosition[1];
            }
        }
    }
    public bool isfinished(){
        return finishedmove;
    }
     public bool checkForExchange(){
         if((positionLevel==1 || positionLevel == 3)){ // change waypoints check to 1 to 3
              int waypointpos = positionLevel==1 ? currentPosition-1: 53-currentPosition;
            
            try{
                
                List<GameObject>[] collidedObjects= wayPoints[waypointpos].gameObject.
                                        GetComponent<NontempleScript>().getCollidedObjects();
                
                for(int i=0;i<GameController.noOfPlayers;i++){
                    if(i==myNumber)
                        continue;
                    foreach(GameObject g in collidedObjects[i])
                        collidedObjectsList.Add(g);
                
                }
                NumOfOtherPlayers=collidedObjectsList.Count;
                otherPlayerCount=NumOfOtherPlayers;
                Debug.Log(" NumOfOtherPlayers= "+NumOfOtherPlayers);
                if(NumOfOtherPlayers>0){
                    wayPoints[waypointpos].gameObject.
                                        GetComponent<NontempleScript>().PlayDeadAnim();
                    Invoke("moveOthersToHome",1.0F);
                    if(!GameController.extraPlayForCancel){
                        gamecontoller.GetComponent<GameController>().extraplay();
                        
                    }  
                    return true;  
                }
            }
            catch{

            }
            
        }
        return false;
     }
    void moveOthersToHome(){
        collidedObjectsList[otherPlayerCount-NumOfOtherPlayers].GetComponent<pawnScript>().sendToHome();
        
        NumOfOtherPlayers--;
        if(NumOfOtherPlayers>0){
            Invoke("moveOthersToHome",0.02F);
        }
        else{
            collidedObjectsList.Clear();
            finishedmoving=true;
        }
    }
     
    public void StartAnimation(int val,GameObject g,bool moveAll5){
        move5=moveAll5;
        valueHolder=g;
        if(!AnimationPlaying){
            if(positionLevel==1  || positionLevel == 3) moveToCenter();
            if(positionLevel==0 || positionLevel==2)  boxcollider.enabled=true;
        }
        settilesToMove(val);
        anim.GetComponent<Animator>().enabled = true;
        anim.Play("playertomove", -1, 0f);
        AnimationPlaying=true;
    }
    public void StopAnimation(){
        boxcollider.enabled=false;
        anim.GetComponent<Animator>().enabled = false;
        settilesToMove(0);
        if(AnimationPlaying)
            if((positionLevel==1  || positionLevel == 3)&&!touchedThis) backToPosition();
        AnimationPlaying=false;
        touchedThis=false;
       // anim.Play("New State");
    }
    public void setpositionLevel(int positionLevel){
        this.positionLevel = positionLevel;
    }
    public int getpositionLevel(){
        return positionLevel;
    }


    public void setcurrentPosition(int currentPosition){
        this.currentPosition = currentPosition;
    }
    public int getcurrentPosition(){
        return currentPosition;
    }

    private int getPlayerNumber(GameObject col){
        if(col.name.Contains("blue"))
            return 0;
        else if(col.name.Contains("purple"))
            return 1;
        else if(col.name.Contains("red"))
            return 2;
        else if(col.name.Contains("green"))
            return 3;
        else 
            return 4;
    }
    private int getPawnNumner(){
        if(gameObject.name.Contains("1"))
            return 0;
        else if(gameObject.name.Contains("2"))
            return 1;
        else if(gameObject.name.Contains("3"))
            return 2;
        else if(gameObject.name.Contains("4"))
            return 3;
        else
            return 4;
    }

}