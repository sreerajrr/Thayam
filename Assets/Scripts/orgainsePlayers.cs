using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orgainsePlayers : MonoBehaviour
{
    
    private List<GameObject>[]  collidedObjects = new List<GameObject>[4];
    public Vector3[][] positionsForPawns;
   // private int[] NoOfPawns =new int [4];
    /* NoOfPawns[x] -> x= which player entered , i[x] = how may of each player entered
x=0 -> blue player (player 1);
x=1 -> purple player (player 2);
x=2 -> red player (player 3);
x=3 -> green player (player 4); */
    void Start(){
        for(int j = 0;j<GameController.noOfPlayers;j++)
        {   
            collidedObjects[j]= new List<GameObject>();
        }
        positionsForPawns = getPositionOfPawn();
       }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int[] GetNoOfEachPlayers(){
        int[] noOfPawns= new int[GameController.noOfPlayers];
        for(int i=0;i<GameController.noOfPlayers;i++){
            noOfPawns[i]= collidedObjects[i].Count;
        }
        return noOfPawns;
    }
   
    void OnCollisionEnter(Collision col)
    {
        int playerNumber = getPlayerNumber(col.gameObject);
        collidedObjects[playerNumber].Add(col.gameObject);
        arrangeplayers();

    }
        // if(NoOfPawns[playerNumber] == 0)
        //     currentlyEmpty[playerNumber]=true;
        // NoOfPawns[playerNumber]++;
        // if(currentlyEmpty[playerNumber] && NoOfPawns[playerNumber] > 0)
        // {
        //     totalPlayerInWaypont++;
        //     currentlyEmpty[playerNumber]=false;
        // }
    // }

    void OnCollisionExit(Collision col){
        int playerNumber = getPlayerNumber(col.gameObject);
        collidedObjects[playerNumber].Remove(col.gameObject);
/************************** TO-DO ************************* */
         arrangeplayers();
       //error while Running above code;
/********************************************************** */
    }

        // if( NoOfPawns[playerNumber] > 0)
        //     NoOfPawns[playerNumber]--;
        // if(!currentlyEmpty[playerNumber]&& NoOfPawns[playerNumber] !=0)
        // {
        //     totalPlayerInWaypont--;
        //     currentlyEmpty[playerNumber]=true;
        // }

    // }


    private int getPlayerNumber(GameObject col){
        if(col.gameObject.name.Contains("blue"))
            return 0;
        else if(col.gameObject.name.Contains("purple"))
            return 1;
        else if(col.gameObject.name.Contains("red"))
            return 2;
        else if(col.gameObject.name.Contains("green"))
            return 3;
        else 
            return 4;
    }

    public Vector3[] organiseForNextMove(GameObject toCome){
        int totalpawns=0;
        for(int i = 0;i<GameController.noOfPlayers;i++)
            totalpawns+=collidedObjects[i].Count;
        totalpawns++;
        int toCOmeplayerNumber=getPlayerNumber(toCome);
        int positionForToCome=0;
        List<GameObject> fullList = new List<GameObject>();
        for(int i=0;i<GameController.noOfPlayers;i++)
            foreach(GameObject g in collidedObjects[i]){
                fullList.Add(g);
            }
        for(int i=GameController.noOfPlayers-1;i>=0;i--)
        {
            if(i==toCOmeplayerNumber)
                    break;
            foreach(GameObject g in collidedObjects[i])
            {
                positionForToCome++;
            }
        }
        Vector3 scl=new Vector3(0.4F,0.2f,0.4F);
        for(int i=0;i<totalpawns-1;i++){
            GameObject g=fullList[i];
            // int pos=i<(positionForToCome)?totalpawns-i-2:totalpawns-i-1;
            int pos = (totalpawns-i-1)>positionForToCome?totalpawns-i-1:totalpawns-i-2;
            g.transform.position = positionsForPawns[totalpawns-1]
                                [pos];
            if(totalpawns<2){
                g.transform.localScale = new Vector3(0.4F,0.2f,0.4F);
                }
            else{
                g.transform.localScale = new Vector3(0.3F,0.2f,0.3F);
                }
        }
        if(totalpawns<2){
            scl=new Vector3(0.4F,0.2f,0.4F);
        }
        else
        {
            scl=new Vector3(0.3F,0.2f,0.3F);
        }
        fullList.Clear();
        Vector3[] rn={positionsForPawns[totalpawns-1][positionForToCome],scl};
        return rn;
    }
    private void arrangeplayers(){
        
        // int totalpawns = NoOfPawns[0]+NoOfPawns[1]+NoOfPawns[2]+NoOfPawns[3];
        int totalpawns=0;
        for(int i = 0;i<GameController.noOfPlayers;i++)
        totalpawns+=collidedObjects[i].Count;
                    
        List<GameObject> fullList = new List<GameObject>();
        for(int i=0;i<GameController.noOfPlayers;i++)
        foreach(GameObject g in collidedObjects[i])
            fullList.Add(g);

        for(int i=0;i<totalpawns;i++){
            GameObject g=fullList[i];
            g.transform.position = positionsForPawns[fullList.Count-1][totalpawns- i-1];
            if(totalpawns<2){
                g.transform.localScale = new Vector3(0.4F,0.2f,0.4F);
                }
            else{
                g.transform.localScale = new Vector3(0.3F,0.2f,0.3F);
                }
        }

        fullList.Clear();
    }

    private Vector3[][] getPositionOfPawn(){

        float x3=this.gameObject.transform.position.x+0.065F;
        float x4=x3+0.22575F;
        float x5=x4+0.22575F;
        float x2=x3-0.22575F;
        float x1=x2-0.22575F;

        float y = 64.194F;

       
        float z3=this.gameObject.transform.position.z+0.35F;
        float z4=z3+0.284F;
        float z2=z3-0.284F;
        float z1=z2-0.284F;

        Vector3 p11 = new Vector3(x1,y,z1);
        Vector3 p12 = new Vector3(x2,y,z1);
        Vector3 p13 = new Vector3(x3,y,z1);
        Vector3 p14 = new Vector3(x4,y,z1);
        Vector3 p15 = new Vector3(x5,y,z1);

        Vector3 p21 = new Vector3(x1,y,z2);
        Vector3 p22 = new Vector3(x2,y,z2);
        Vector3 p23 = new Vector3(x3,y,z2);
        Vector3 p24 = new Vector3(x4,y,z2);
        Vector3 p25 = new Vector3(x5,y,z2);
        
        Vector3 p31 = new Vector3(x1,y,z3);
        Vector3 p32 = new Vector3(x2,y,z3);
        Vector3 p33 = new Vector3(x3,y,z3);
        Vector3 p34 = new Vector3(x4,y,z3);
        Vector3 p35 = new Vector3(x5,y,z3);
        
        Vector3 p41 = new Vector3(x1,y,z4);
        Vector3 p42 = new Vector3(x2,y,z4);
        Vector3 p43 = new Vector3(x3,y,z4);
        Vector3 p44 = new Vector3(x4,y,z4);
        Vector3 p45 = new Vector3(x5,y,z4);
        
        
        Vector3 e3_681 = new Vector3(0,0,0.079F);
        Vector3 e3_779 = new Vector3(0,0,-0.11F);  
        Vector3 e2_556 = new Vector3(0,0,0.058F); 
        Vector3 e3_757 = new Vector3(0,0,-0.10F); 

        Vector3[] pawns1 = {p34};
        Vector3[] pawns2 = {p32,p34};
        Vector3[] pawns3 = {p32,p34,p12};
        Vector3[] pawns4 = {p32,p34,p12,p14};    
        Vector3[] pawns5 = {p41,p45,p33,p11,p15}; 
        Vector3[] pawns6 = {p41,p43,p45,p11,p13,p15};    
        Vector3[] pawns7 = {p41,p43,p45,p32+e3_681 ,p11,p13,p15};        
        Vector3[] pawns8 = {p41,p43,p45,p32+e3_681 ,p34+e3_681 ,p11,p13,p15};        
        Vector3[] pawns9 = {p41,p43,p45,p31+e3_779,p33+e3_779,p35+e3_779,p11,p13,p15};        
        Vector3[] pawns10= {p41,p43,p45,p32,p34,p22+e2_556 ,p24+e2_556 ,p11,p13,p15};        
        Vector3[] pawns11= {p41,p43,p45,p32,p33,p34,p22+e2_556 ,p24+e2_556 ,p11,p13,p15};        
        Vector3[] pawns12= {p41,p43,p45,p32,p33,p34,p22,p23,p24,p11,p13,p15};        
        Vector3[] pawns13= {p41,p43,p45,p32,p33,p34, p31+e3_757 ,p22,p23,p24,p11,p13,p15};        
        Vector3[] pawns14= {p41,p43,p45,p32,p33,p34, p31+e3_757, p35+e3_757, p22,p23,p24,p11,p13,p15};        
        Vector3[] pawns15= {p41,p43,p45,p31,p32,p33,p34, p35+e3_757, p21,p22,p23,p24,p11,p13,p15};        
        Vector3[] pawns16= {p41,p43,p45,p31,p32,p33,p34,p35,p21,p22,p23,p24,p25,p11,p13,p15};        
        Vector3[] pawns17= {p41,p42,p43,p45,p31,p32,p33,p34,p35,p21,p22,p23,p24,p25,p11,p13,p15};        
        Vector3[] pawns18= {p41,p42,p43,p44,p45,p31,p32,p33,p34,p35,p21,p22,p23,p24,p25,p11,p13,p15};        
        Vector3[] pawns19= {p41,p42,p43,p44,p45,p31,p32,p33,p34,p35,p21,p22,p23,p24,p25,p11,p12,p13,p15};        
        Vector3[] pawns20= {p41,p42,p43,p44,p45,p31,p32,p33,p34,p35,p21,p22,p23,p24,p25,p11,p12,p13,p14,p15};

        Vector3[][] pawns = {pawns1,pawns2,pawns3,pawns4,pawns5,pawns6,pawns7,pawns8,pawns9,pawns10,
                             pawns11,pawns12,pawns13,pawns14,pawns15,pawns16,pawns17,pawns18,pawns19,
                             pawns20 };     

        return pawns;
    }
}