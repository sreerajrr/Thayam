using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{

    public GameObject panel;
    public Button AIButton;
    public static int noOfPlayers;
    public static int gameLevel;
    public GameObject[] wayPoints;
    public Transform[] Players; // 0-4 = blue , 5-9 = purple . 10-14 = red , 15-19 -> green
    //public Transform[][] players = { bluePlayers,purplePlayers,redPlayers,greenPlayers };
    public Button button;
    public GameObject playerTomovee;
    public List<int> valuesUsed;
    public GameObject[] playerBoards;
    public static bool isAiWorking = false;
    private ColorBlock buttonColors;
    [SerializeField]
    private GameObject die;
    [SerializeField]
    private RectTransform dierect;
    [SerializeField]
    public GameObject[] dieValuePosition;
    int dieValuePositionNum;
    public static bool isAI;
    public static int playerNumber;
    public static bool dierolled;
    public static bool canrolldie;
    public static List<int> dievalue = new List<int>();
    private  List<int> Randomizedievalue = new List<int>();
    public static List<GameObject> movablePlayers = new List<GameObject>();
    public static int movesleft;
    public static List<int> templePoints= new List<int>();
    private List<int> EnemyPositions= new List<int>();
    List<int> dieSumValueList= new List<int>();
    List<int> dieSumList = new List<int>();
    List<int> ScoresPerValuePerPlayer=new List<int>();
    bool[] halfCompleted;
    int dievalueIndex=0;
    bool tileSelected;
    List<int> removedPlayer=new List<int>();
    
    public static bool extraPlayForCancel;
    public static int currentAiPlayer;
    public AudioClip[] otherClip;
    public int round;
    AudioSource audio;
    bool fromOtherPhone = false;
    public void OpenPanel(){
        panel.SetActive(true);
    }
    void Awake()
    {
        try{
            noOfPlayers=persistantdatas.Instance.noOfPlayers;
            gameLevel=persistantdatas.Instance.levelNum;
        }
        catch(Exception e){
            noOfPlayers=4;
            gameLevel=1;
        }
        round=0;
        dievalue.Clear();
        //gameLevel-> local,vs Comp,Online,with Friends;
        Debug.Log("Total number of players in this game is "+noOfPlayers);
        templePoints.Add(0);
        templePoints.Add(1);
        templePoints.Add(2);
        templePoints.Add(3);
        templePoints.Add(4);
        templePoints.Add(5);;
        templePoints.Add(10);
        templePoints.Add(15);
        templePoints.Add(20);
        templePoints.Add(25);
        templePoints.Add(26);
    }

    public void playMovementAudio(){
        audio.PlayOneShot(otherClip[0],1.0F);
    }
    public void playTempleReachedAudio(){
        audio.PlayOneShot(otherClip[1],1.0F);
    }
    public void playKillAudio(){
        audio.PlayOneShot(otherClip[2],1.0F);
    }
    // Start is called before the first frame update
    void Start()
    {   
        isAI= false;
        audio= gameObject.AddComponent<AudioSource>();
        if(gameLevel==2){
            currentAiPlayer=2;
        }
        else
            currentAiPlayer=10;
        extraPlayForCancel=false;   
        tileSelected=false;
        for(int i = 0; i <noOfPlayers*5;i++)
        {
           Players[i].gameObject.SetActive(true);
        }
        halfCompleted = new bool[noOfPlayers];
        for(int i =0;i<noOfPlayers;i++){
            halfCompleted[i]=false;
        }
        movesleft=1;
        // print("Local scale "+ all.localScale);
        // float xaxis = (all.localScale.x*Screen.width)/480;
        // Vector3 newScale = new Vector3(xaxis,all.localScale.y,all.localScale.z);
        // all.localScale = newScale;
        buttonColors =button.colors;
        buttonColors.normalColor=Color.blue;
        buttonColors.highlightedColor=Color.blue;
        buttonColors.pressedColor=Color.blue;
        button.colors=buttonColors;
        canrolldie = true;
        Randomizedievalue.Clear();
        playerNumber = 0;
        //currentAiPlayer=playerNumber;
        dieValuePositionNum=0;
        // int wpC=0;
        // foreach (GameObject wpG in wayPoints)
        // {
        //     wpC++;
        // }
        // float avg06 = 0;
        // float avg610 = 0;
        // float avg1015 = 0;
        // float avg1520 = 0;
        // float avg2025 = 0;
        // Debug.Log("wpc = "+wpC);
        // for(int wp=0;wp<wpC-1;wp++){
        //     float wpDistance= Vector3.Distance(wayPoints[wp].transform.position,wayPoints[wp+1].transform.position);
        //     Debug.Log(wp+" to " +(wp+1)+" - "+wpDistance );
        //     if(wp<5){
        //         avg06+=wpDistance;
        //     }
        //     else if(wp<10){
        //         avg610+=wpDistance;
        //     }else if(wp<15){
        //         avg1015+=wpDistance;
        //     }else if(wp<20){
        //         avg1520+=wpDistance;
        //     }else if(wp<25){
        //         avg2025+=wpDistance;
        //     }
        // }
        // Debug.Log("Average ->"+ "0-6= "+avg06/5+ " avg610= "+avg610/5+ " avg1015= "+avg1015/5+ " avg1520 "+avg1520/5
        //                                                                     + " avg2025 "+avg2025/5);

    }
    public void resetTable(){
        for(int i=0;i<16;i++){
            dieValuePosition[i].SetActive(false);
            
            for(int j=0;j<=5;j++){
                dieValuePosition[i].gameObject.transform.GetChild(0).gameObject.transform.GetChild(j).gameObject.SetActive(false);
                dieValuePosition[i].gameObject.transform.GetChild(1).gameObject.transform.GetChild(j).gameObject.SetActive(false);
            }
        }
        dieValuePositionNum=0;
        foreach(int dvalue in dievalue){
            dieValuePosition[dieValuePositionNum].SetActive(true);
            dieValuePosition[dieValuePositionNum].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            dieValuePosition[dieValuePositionNum].gameObject.transform.GetChild(1).gameObject.SetActive(false);
            dieValuePosition[dieValuePositionNum].gameObject.transform.GetChild(0).gameObject.transform
                .GetChild(dvalue==10?5:dvalue-1).gameObject.SetActive(true);
            dieValuePositionNum++;
        }
        foreach(GameObject g in dieValuePosition){
                g.GetComponent<PositionHolderTouch>().resetIt();
                g.GetComponent<PositionHolderTouch>().canTouchthis(true);
        }
        
        // try{
        //     if(playerNumber!=currentAiPlayer){
        //         dieValuePosition[0].GetComponent<PositionHolderTouch>().OnMouseDown();
        //     }
        
        // }
        // catch(Exception e){

        // }
    }
    
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////AI RUNNING/////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////
    public void callNextPlayer(){
        Invoke("FirstAI",1F);
        if(gameLevel==2)
            currentAiPlayer=playerNumber;
    }
    public void FirstAI(){
        AIrollthedie();
        // if(dievalue.Count>0){
        //     Invoke("checkAndPlay",0.3F);
        // }
    }
    public void checkAndPlay(){
        dieSumValueList.Clear();
        valuesUsed.Clear();
        dieSumList.Clear();
        ScoresPerValuePerPlayer.Clear();
        int topScoreIndex=0;
        int topScore=0;
        int pos1=0,pos2=0;;
        int pstart=playerNumber*5;
        for(int i1 =pstart;i1<pstart+5;i1++){
            if(Players[i1].gameObject.GetComponent<pawnScript>().getpositionLevel() == 0 
                && !Players[i1].gameObject.GetComponent<pawnScript>().isfinished()){
                pos1++;
            }
            if(Players[i1].gameObject.GetComponent<pawnScript>().getpositionLevel() == 2 ){
                pos2++;
            }
        }
        if((pos1 == 5 || pos2==5) && dievalue.Contains(5)){
             Invoke("moveThe5",1.0F);
             dieValuePosition[dievalue.IndexOf(5)].
                GetComponent<PositionHolderTouch>().TurnedOnColor();
        }
        else{
            getEnemyPositions();
            subsetSums(dievalue,0,dievalue.Count-1,0,new List<int>());
            dieSumList.Remove(0);
            string[] scoreString={"",""}; 
            int smallestIndex=dieSumList.Count-1;
            int indx=0;
            playerTomovee=Players[pstart].gameObject;
            SortedSet<int> dieSumSet=new SortedSet<int>();
            foreach (int dSum in dieSumList)
            {
                dieSumSet.Add(dSum);
            }
            for(int ii=pstart;ii<pstart+5;ii++){
                int pPosition=Players[ii].GetComponent<pawnScript>().getcurrentPosition();
                int pLevel=Players[ii].GetComponent<pawnScript>().getpositionLevel();
                foreach (int dSum in dieSumSet)
                {
                    if(canMoveThis(Players[ii].gameObject,dSum,pstart)&&needToAnimate(ii,pstart)){
                        int score=0;
                        switch(pLevel){
                            case 0:
                                score=800;
                                break;
                            case 1:
                                scoreString=getScore2(pPosition-1,dSum,pLevel).Split(',');
                                score=int.Parse(scoreString[1]);
                                break;
                            case 2:
                                score=800;
                                break;
                            case 3:
                                scoreString=getScore2(pPosition-1,dSum,pLevel).Split(',');
                                score=int.Parse(scoreString[1]);
                                break;
                            default: break;
                        }
                        //finds the topscore and in the top scores it will check for the least
                        // index value(which means the greatest sum) in order 
                        // to select max value holders in one move
                        // so as to eliminate the number of future possible moves;
                        if(topScore<=score){
                            if(topScore<score)
                                smallestIndex=dieSumList.Count-1;
                            topScore=score;
                            indx = dieSumList.IndexOf(dSum);
                            if(smallestIndex>=indx){
                                playerTomovee=Players[ii].gameObject;
                                smallestIndex=indx;

                            }
                        }
                        // prints score of each possible moves;
                        // Debug.Log("players to move is "+ Players[ii]+",distance= "+dSum+"Score "+scoreString[0]+" sc "+score);
                    }
                    else{
                        ScoresPerValuePerPlayer.Add(0);
                    }        
                }
            }
            // valuesUsed=dieSumValueList[smallestIndex];
            // Debug.Log("players to move is "+ playerTomovee +"  distance = "+dieSumList[smallestIndex]+" sc "+topScore);
          
            int pp=0;
            foreach (int L in dieSumValueList)
            {
                if(L==2000){
                    pp++;
                    continue;
                }
                if(pp==smallestIndex+1){
                     valuesUsed.Add(L);
                }
            } 
            moveit();
        }
        
    }
    
    public void moveit(){
        if(valuesUsed.Count>0){
            Invoke("moveAfter",1.0F);
            dieValuePosition[dievalue.IndexOf(valuesUsed[0])].
                GetComponent<PositionHolderTouch>().TurnedOnColor();
        }
        else if((gameLevel==2 && currentAiPlayer==playerNumber) || (isAI && GameManagerGame.Instance.currentPlayerIndex==0 && isAiWorking)){
            Debug.Log("I am the culprite");
            FirstAI();
        }
    }
    //move five at a time
    void moveThe5(){
         Players[playerNumber*5].gameObject.GetComponent<pawnScript>().
                            MoveRequestFromAI(5,dieValuePosition[dievalue.IndexOf(5)],true); 
        // Invoke("checkAndPlay",0.3F);
    }
    void moveAfter(){
        playerTomovee.gameObject.GetComponent<pawnScript>().
                            MoveRequestFromAI(valuesUsed[0],
                            dieValuePosition[dievalue.IndexOf(valuesUsed[0])],false);
        if(valuesUsed.Count>0)
            valuesUsed.Remove(valuesUsed[0]);
    }
    //to get the score of each possible play and use this score to figure out wich move to use;
    string getScore2(int pos,int ttm,int posLevel){//pos = pos according to waypoint=>home =-1;
        int Dangerscore=0;
        int GoToScore=0;
        int NotGoToScore=0;
        int StayHereScore=0;
        int score=0;
        string scoreString="";
        int justBehindAfterMoving=-6;
        int justBehindBeforeMoving=-6;
        bool isTemple=true;
        int positionScore=0;
        foreach (int ePos in EnemyPositions)
        {
            if(posLevel==1){
                if(ePos<=pos){
                    justBehindBeforeMoving=ePos;
                }
                if(ePos<pos+ttm){
                    justBehindAfterMoving=ePos;
                }
            }
            else{
                ttm=-ttm;
                pos=52-pos;
                if(ePos>=pos){
                    justBehindBeforeMoving=ePos;
                }
                if(ePos>pos+ttm){
                    justBehindAfterMoving=ePos;
                }
            }
        }

        Debug.Log("justBehindAfterMoving "+
            justBehindAfterMoving+" justBehindBeforeMoving "+
            justBehindBeforeMoving+"  pos  "+ pos);
        if(!templePoints.Contains(pos)){isTemple=false;scoreString+="NotTemple+";Dangerscore+=100;}
        if(templePoints.Contains(pos+ttm)){
                                                scoreString+="can Reach Temple+";GoToScore+=100;}
        else{
            if(posLevel==1){
                if(pos+ttm-justBehindAfterMoving<=5)
                    {scoreString+="NOtReachInTempleWithPeopleBehind+";NotGoToScore+=60;}
            }
            else{
                if(justBehindAfterMoving-(pos-ttm)<=5)
                    {scoreString+="NOtReachInTempleWithPeopleBehind+";NotGoToScore+=60;}
            }
        }
        
        if(posLevel==1){
            if(EnemyPositions.Contains(pos+ttm)&&!(templePoints.Contains(pos+ttm))){
                                                scoreString+="Can Kill";GoToScore+=300;}
            if(0<=pos&&pos<6) positionScore=4;
            else if(6<=pos&&pos<10)positionScore=1;
            else if(11<=pos&&pos<15)positionScore=2;
            else if(16<=pos&&pos<20)positionScore=3;
            else if(21<=pos&&pos<25)positionScore=40;
            else if(pos == 10)positionScore=3;
            else if(pos == 15)positionScore=2;
            else if(pos == 20)positionScore=1;
            else if(pos == 25)positionScore=0;
            if(isTemple){
                if(pos-justBehindBeforeMoving<=5){scoreString+="InTempleWithPeopleBehind+";
                                                    StayHereScore+=30;}
                if(pos-justBehindBeforeMoving>=9){scoreString+="InTempleWithNoPeopleBehind+";
                                                    Dangerscore+=10;}
            }
            else {if(pos-justBehindBeforeMoving<=5){scoreString+="NotInTempleWithPeopleBehind+";
                                                    Dangerscore+=30;}
                if(pos+ttm==26)GoToScore+=50;}
        }
        else{
            if(EnemyPositions.Contains(pos-ttm)&&!(templePoints.Contains(pos-ttm))){
                                                scoreString+="Can Kill";GoToScore+=300;}
            if(0<=pos&&pos<6) positionScore=0;
            else if(6<=pos&&pos<10)positionScore=0;
            else if(11<=pos&&pos<15)positionScore=4;
            else if(16<=pos&&pos<20)positionScore=3;
            else if(21<=pos&&pos<25)positionScore=2;
            else if(pos == 10)positionScore=1;
            else if(pos == 15)positionScore=2;
            else if(pos == 20)positionScore=3;
            else if(pos == 25)positionScore=4;
            if(isTemple){
                if(justBehindBeforeMoving-pos<=5){scoreString+="InTempleWithPeopleBehind+";
                                                    StayHereScore+=30;}
                if(justBehindBeforeMoving-pos>=9){scoreString+="InTempleWithNoPeopleBehind+";
                                                    Dangerscore+=10;}
            }
            else {if(justBehindBeforeMoving-pos<=5){scoreString+="NotInTempleWithPeopleBehind+";
                                                    Dangerscore+=30;}
                if(pos-ttm==26)GoToScore+=50;}
        }
        score=Dangerscore+GoToScore-NotGoToScore-StayHereScore+positionScore+500;
        if(score<0)
            return scoreString+","+10;
         return scoreString+","+score;
    }

    void subsetSums(List<int> arr, int l, int r, int sum,List<int> tt ) 
    {  
        // Print current subset 
        if (l > r) 
        { 
            dieSumList.Add(sum);

            dieSumValueList.Add(2000);
            foreach (int item in tt)
            {
                dieSumValueList.Add(item);
            }
            // dieSumValueList.Add(sum);
            
            return; 
        } 
        // Subset including arr[l] 
        tt.Add(arr[l]);
        subsetSums(arr, l + 1, r, sum + arr[l],tt); 
        tt.Remove(arr[l]);
        // Subset excluding arr[l] 
        subsetSums(arr, l + 1, r, sum,tt); 
    }

    public void AIrollthedie(){
        // while(canrolldie || movesleft>0||(movesleft>0&&extraPlayForCancel)){
        //     rollthedie();
        // }
        // Debug.Log("I am the culprite222222");
        InvokeRepeating("rollthedie",1.0F,1.0F);
    }


//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////End of AI////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {
        // if(pawnScript.finishedmoving){
        //     try{
        //         moveit();
        //     }
        //     catch(Exception e){

        //     }
        //     pawnScript.finishedmoving=false;
        // }
        //     Debug.Log(movesleft);
    }
   
    public int getdievalue(){

        if(round==3  && !Randomizedievalue.Contains(5)){
            int pos11=0;
            for(int i1 =playerNumber*5;i1<playerNumber*5+5;i1++){
            if(Players[i1].gameObject.GetComponent<pawnScript>().getpositionLevel() == 0){
                pos11++;
            }}
            if(pos11==5){
                return 5;
            }
        }


        int rndmNum=UnityEngine.Random.Range(0,205);
        int dierollvalue;
        int no1=0,no5=0,no6=0;
        foreach (int item in Randomizedievalue)
        {
            if(item==1) no1++;
            if(item==5) no5++;
            if(item==10) no6++;
        }
        if(rndmNum<=40) dierollvalue=2;
        else if(rndmNum<=80) dierollvalue=3;
        else if(rndmNum<=120) dierollvalue=4;
        else if(rndmNum<=170) dierollvalue=1;
        else if(rndmNum<=195) dierollvalue=5;
        else  dierollvalue=6;

        if(no1+no5+no6>=3){
            dierollvalue=UnityEngine.Random.Range(2,5);
        }

        if(dierollvalue==6){
            if(no6==1){
                dierollvalue=UnityEngine.Random.Range(1,7);
            }
            else if(no6>1){
                dierollvalue=UnityEngine.Random.Range(1,6);
            }
        }
        if(dierollvalue==5){
            if(no5==1){
                dierollvalue=UnityEngine.Random.Range(1,7);
            }
            else if(no5>1){
                dierollvalue=UnityEngine.Random.Range(1,6);
                dierollvalue=dierollvalue==5?6:dierollvalue;
            }
        }
        if(dierollvalue==6){
            if(no6==1){
                dierollvalue=UnityEngine.Random.Range(1,7);
            }
            else if(no6>1){
                dierollvalue=UnityEngine.Random.Range(1,5);
            }
        }
        if(dierollvalue==1){
            if(no1==2){
                dierollvalue=UnityEngine.Random.Range(1,7);
            }
            else if(no1>2){
                dierollvalue=UnityEngine.Random.Range(2,7);
            }
        }
            
        return  dierollvalue;
    }
    public void rollthedie()
    {
        int i;
        // if condition may change 
        if (canrolldie || movesleft>0||(movesleft>0&&extraPlayForCancel))
        {
            
            // dievalue.Clear();dievalue.Add(5);dievalue.Add(1);dievalue.Add(2);dievalue.Add(3);dievalue.Add(4);dievalue.Add(2);dievalue.Add(3);
            // dievalue.Add(4);dievalue.Add(10);dievalue.Add(10);dievalue.Add(10);
            i = getdievalue();
            if(i==6)
                i=10;
            dievalue.Add(i);
            Debug.Log("the i value is "+ i+" playerNumber is "+playerNumber);

            // send dievalueto other players
            if(gameLevel==3||gameLevel==4)
                GameManagerGame.Instance.SendDieValue(i);
            
            Randomizedievalue.Add(i);
            try{
            panel.GetComponent<PanelScript>().addDieValues(i);
            }
            catch{}
            dieValuePositionNum=0;
            foreach(int dvalue in dievalue){
                dieValuePosition[dieValuePositionNum].SetActive(true);
                dieValuePosition[dieValuePositionNum].gameObject.transform.GetChild(0).gameObject.SetActive(true);
                dieValuePosition[dieValuePositionNum].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                dieValuePosition[dieValuePositionNum].transform
                    .GetChild(0).GetChild(dvalue==10?5:dvalue-1).gameObject.SetActive(true);
                dieValuePositionNum++;
                if(dieValuePositionNum==16){
                    dierolled=true;
                    canrolldie = false;
                    movesleft--;
                    // dierollCompleted();
                }
            }
            if(extraPlayForCancel){
                dierolled = true;
                canrolldie = false;
                playerBoards[playerNumber].GetComponent<playerBoard>().canTouch(false);
                movesleft--;
                NoMoreAvailablePlays(playerNumber);
                
            }
            else if(!(i==1 || i == 5 || i==10)){
                dierolled = true;
                canrolldie = false;
                movesleft--;
                playerBoards[playerNumber].GetComponent<playerBoard>().canTouch(false);
                NoMoreAvailablePlays(playerNumber);
                //Debug.Log(movesleft);
                // dierollCompleted();
             }
             int pos1=0;
             int pos2=0;
            if(playerNumber!=currentAiPlayer){
                
                for(int ip=playerNumber*5;ip<(playerNumber*5)+5;ip++){
                    if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 0 
                   && !Players[i].gameObject.GetComponent<pawnScript>().isfinished()){
                    pos1++;
                    }
                    if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 2 ){
                        pos2++;
                    }
                    if(!tileSelected){
                        if((pos1 == 5 || pos2==5) && i == 5){
                            // dieValuePosition[dievalue.IndexOf(i)].GetComponent<PositionHolderTouch>().OnMouseDown();
                            tileSelected=true;
                            break;
                        }
                        if(canMoveThis(Players[ip].gameObject,i,playerNumber*5)){
                            // dieValuePosition[dievalue.IndexOf(i)].GetComponent<PositionHolderTouch>().OnMouseDown();
                            tileSelected=true;
                            break;
                        }
                    }
                }
             }
        }
        else if((gameLevel==3 || gameLevel == 4) && GameManagerGame.Instance.currentPlayerIndex == 0){
            CancelInvoke("rollthedie");
            Debug.Log("Cancelling InvokeInvokeInvokeInvokeInvokeInvokeInvokeInvokeInvokeInvokeInvoke");
            if(dievalue.Count>0){
            Invoke("checkAndPlay",0.7F);
        }
        }
    }

    public void extraplay(){
        extraPlayForCancel=true;
        movesleft++;  
        Debug.Log("extra play");
        playerBoards[playerNumber].GetComponent<playerBoard>().canTouch(true);
    }

    //chilappol deleteyanam.. avishya, varla
    public void checkChanceCompleted(){
         for(int i=0;i<16;i++){
            if(dieValuePosition[i].active){
            return;
        }
        nextPlayer();  
        if(gameLevel==3||gameLevel==4){
            GameManagerGame.Instance.callNextPlayer();   
        }

        }
    }
    private void getEnemyPositions(){
        EnemyPositions.Clear();
        int[] pArray;
        for(int i = 0;i<26;i++){
            if(templePoints.Contains(i)){
                pArray= wayPoints[i].GetComponent<orgainsePlayers>().GetNoOfEachPlayers();
            }
            else{
                pArray= wayPoints[i].GetComponent<NontempleScript>().GetNoOfEachPlayers();
            }
            for(int j=0;j<noOfPlayers;j++){
                if(j!=playerNumber)
                    if(pArray[j]>0){
                        EnemyPositions.Add(i);
                        break;
                    }
            }
        }
    }

    public void SetAnimation(int dieVal,GameObject g){
            int pos1=0,pos2=0;;
            int pstart=playerNumber*5;
            for(int i =pstart;i<pstart+5;i++){
                if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 0 
                   && !Players[i].gameObject.GetComponent<pawnScript>().isfinished()){
                    pos1++;
                }
                if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 2 ){
                    pos2++;
                }
            }
            if((pos1 == 5 || pos2==5) && dieVal == 5){
                for(int i =pstart;i<pstart+5;i++){
                    movablePlayers.Add(Players[i].gameObject);
                    Players[i].gameObject.GetComponent<pawnScript>().StartAnimation(dieVal,g,true);
                //move all at once is the true at the end
                }
            }
            else{
                for(int i =pstart;i<pstart+5;i++){
                    if(canMoveThis(Players[i].gameObject,dieVal,pstart) && needToAnimate(i,pstart)){
                        movablePlayers.Add(Players[i].gameObject);
                        Players[i].gameObject.GetComponent<pawnScript>().StartAnimation(dieVal,g,false);
                    }
                    else{
                        Players[i].gameObject.GetComponent<pawnScript>().StopAnimation();
                        Players[i].gameObject.GetComponent<pawnScript>().settilesToMove(0);
                    }
                }
            }
    }
    public void NoMoreAvailablePlays(int playerNum){
        int pstart=playerNum*5;
        int pos1=0,pos2=0;
        for(int i =pstart;i<pstart+5;i++){
            if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 0 ){
                pos1++;
            }
            if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 2 ){
                pos2++;
            }
        }
        if((pos1 == 5 || pos2==5)){
            foreach(int i in dievalue){
                if(i==5 || i==1){
                    return;
                }
            }
        }
        else{
            foreach(int i in dievalue){
                for(int j=pstart;j<pstart+5;j++){
                    if(canMoveThis(Players[j].gameObject,i,pstart)){
                       
                        return;
                    }
                }
            }
        }
        if(movesleft==0){
            dievalue.Clear();
            CancelInvoke("rollthedie");
            Debug.Log("Cancel inoke because of next player ");
            isAiWorking = false;
            Invoke("nextPlayer",1);
            if(gameLevel==3||gameLevel==4){
                   Invoke("callNextPlayerOnNetwork",0.6f);
        }
        }
        
    }
    void callNextPlayerOnNetwork(){
        GameManagerGame.Instance.callNextPlayer();
    }

    bool needToAnimate(int i ,int ps){
        switch(i-ps){
            case 0:
                    return true;
            case 1:
                if(pos(i-1) == pos(i)) 
                    return false;
                else
                    return true;
            case 2:
                if(pos(i-1) == pos(i) || (pos(i-2) == pos(i))) 
                    return false;
                else
                    return true;
            case 3:
                if(pos(i-1) == pos(i) || (pos(i-2) == pos(i)) || (pos(i-3) == pos(i))) 
                    return false;
                else
                    return true;
            case 4: 
                if(pos(i-1) == pos(i) || pos(i-2) == pos(i) || pos(i-3) == pos(i) || pos(i-4) == pos(i)) 
                    return false;
                else
                    return true;
            default:return false;
        }
        

    }

    int pos(int i){
        return Players[i].GetComponent<pawnScript>().getcurrentPosition();
    }

    bool canMoveThis(GameObject p,int val,int pstartt){
         if((p.GetComponent<pawnScript>().getpositionLevel() == 0 && 
            !p.GetComponent<pawnScript>().isfinished())|| 
            (p.GetComponent<pawnScript>().getpositionLevel() == 2 && 
            isHalfComplete(p,pstartt))){
                if(val==1 ){
                    return true;
                }
                else{
                    return false;
                }
            }
        else if(p.GetComponent<pawnScript>().getpositionLevel() == 1){
            int pos =p.GetComponent<pawnScript>().getcurrentPosition();
            if(pos+val<=27)
                return true;
        }
        else if(p.GetComponent<pawnScript>().getpositionLevel() == 3){
            int pos =p.GetComponent<pawnScript>().getcurrentPosition();
            if(pos+val<=54)
                return true;
        }
        
        return false;
    }
    public void stopAllAnimation(){
        Debug.Log("stoping all animations");
        foreach(GameObject g in movablePlayers){
                g.GetComponent<pawnScript>().StopAnimation();
        }
        foreach(GameObject g in dieValuePosition){
                g.GetComponent<PositionHolderTouch>().canTouchthis(false);
        }
        movablePlayers.Clear();
    }
//below checks weathher the player completed half the game.
    public bool isHalfComplete(GameObject p,int pstartt){
        int pos2=0;
        if(!halfCompleted[pstartt/5]){
            for(int i = pstartt;i<pstartt+5;i++){
                if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 2 ){
                    pos2++;
                }
            }
            if(pos2==5)
                halfCompleted[pstartt/5]=true; 
        }
        return halfCompleted[pstartt/5];
        

    }
    // if player at level 0/2 .. it cant move untill 1/5 or if it is 6 check for new dievalue
    // if player is at level 1/3 player cant move if the dieValue otherthan 1/5/6 exceeds the pathway
    //no dieValue contains 1 or 5 its passed to next player
    //if we cancel nother player .we get an extra play
    bool canMove(){ 
        int p=0;
        foreach(int i in dievalue){
            if(i==1 || i==5)
                {
                    p=1;
                    return true;    
                }
        }
        if(p==0)
            return false;
        int pstart=playerNumber*5;
        int NoofplayersAt0or2 =0, NoofPlayersAt1or3=0,NoofPlayersCantmove=0;
        for(int i =pstart;i<pstart+5;i++){
            if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 0 || 
                Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 2){
                NoofplayersAt0or2++;
            }
        }
       
        if(!(dievalue[dievalueIndex]==1 || dievalue[dievalueIndex] == 5 || dievalue[dievalueIndex]==10)){
            if(NoofplayersAt0or2 == 5){
                return false;
            }
            else
            {
                for(int i =pstart;i<pstart+5;i++){
                    if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 1){
                        NoofPlayersAt1or3++;
                        int pos =Players[i].gameObject.GetComponent<pawnScript>().getcurrentPosition();
                        if(pos+dievalue[0]>27){
                            NoofPlayersCantmove++;
                        }
                    }
                    else if(Players[i].gameObject.GetComponent<pawnScript>().getpositionLevel() == 3){
                        NoofPlayersAt1or3++;
                        int pos =Players[i].gameObject.GetComponent<pawnScript>().getcurrentPosition();
                        if(pos+dievalue[0]>54){
                            NoofPlayersCantmove++;
                        }
                    }
                }

                if(NoofPlayersAt1or3==NoofPlayersCantmove){
                    return false;
                }

            }
        }
        return true;
    }

    public void next(){
        fromOtherPhone = true;
        nextPlayer();
    }
    void nextPlayer(){
        Debug.Log("Current      == " + GameManagerGame.Instance.currentPlayerIndex+ " "+ fromOtherPhone);
        tileSelected=false;
        Randomizedievalue.Clear();
        extraPlayForCancel=false;
        // playerNumber++;
        // while(removedPlayer.Contains(playerNumber)){
        //     playerNumber++;
        //     if(playerNumber==noOfPlayers){
        //         playerNumber = 0;
        //         round++;
        //     }
        // }
        if(gameLevel==1||gameLevel==2){
            if(playerNumber==noOfPlayers-1){
                    playerNumber = 0;
                    round++;
            }
            else{
                playerNumber++;
            }
            
            changeDieColor(playerNumber);
            changePlayBoard(playerNumber);
            Debug.Log("Current Player = "+ playerNumber);
        }
        if((gameLevel==3||gameLevel==4)&&
                                GameManagerGame.Instance.currentPlayerIndex==0){
                round++; 
            }
        dievalue.Clear();
        for(int i=0;i<16;i++){
            dieValuePosition[i].SetActive(false);
            for(int j=1;j<=6;j++)
                dieValuePosition[i].transform.GetChild(0).GetChild(j-1).gameObject.SetActive(false);
        }
        dieValuePositionNum=0;
        // if(gameLevel ==1 || gameLevel == 2)
        // {
        canrolldie = true;
        movesleft=1;
        // }
        
        

        if(gameLevel==2 && playerNumber==2){

            playerBoards[playerNumber].GetComponent<playerBoard>().isEnabled=false;
            callNextPlayer();
        }
        else {
            if(isAI && noOfPlayers-1-GameManagerGame.Instance.currentPlayerIndex == 0 && fromOtherPhone){
                Debug.Log("workinh00000000000000000000000000000000000");
                // canrolldie = true;
                // movesleft=1;
                // FirstAI();
            }
            fromOtherPhone = false; 
        }
        
    }
    public void ChangeToAi(){
        isAI = !isAI;
        if(isAI){
            AIButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            AIButton.GetComponent<Image>().color = Color.blue;
        }
    }
    private void changePlayBoard(int playerNumber){
        int pBi =0;
        foreach (GameObject pboard in playerBoards)
        {
            if(pBi==playerNumber){
                pboard.GetComponent<playerBoard>().TrunONButton();
            }
            pBi++;
        }
    }
    private void changeDieColor(int playerNumber){
        Color[] b = {Color.blue,Color.magenta,Color.red,Color.green};
        buttonColors.normalColor=b[playerNumber];
         buttonColors.highlightedColor=b[playerNumber];
        buttonColors.pressedColor=b[playerNumber];
        //Debug.Log("color changed" + b[playerNumber]);
        button.colors=buttonColors;
    }
}
