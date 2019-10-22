using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NontempleScript : MonoBehaviour
{
    public GameObject deadSymbol;
    private Animator anim;
    private List<GameObject>[]  collidedObjects = new List<GameObject>[4];
    public Vector3[][] positionsForPawns;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<GameController.noOfPlayers;i++){
            collidedObjects[i] = new List<GameObject>();
        }
        positionsForPawns = getPositionOfPawn();
        anim = deadSymbol.GetComponentInChildren<Animator>();
        anim.enabled=false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDeadAnim(){
        anim.enabled = true;
        deadSymbol.transform.position=new Vector3( transform.position.x , 68.2F,transform.position.z);
        anim.Play("dead animation", -1, 0f);
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
    public List<GameObject>[] getCollidedObjects(){
            return collidedObjects;
    }
    void OnCollisionExit(Collision col){
        int playerNumber = getPlayerNumber(col.gameObject);
        collidedObjects[playerNumber].Remove(col.gameObject);
        arrangeplayers();
    }
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
        int totalpawns=0;
        for(int i = 0;i<GameController.noOfPlayers;i++)
        totalpawns+=collidedObjects[i].Count;
            
        if(totalpawns>6)
            return;
        List<GameObject> fullList = new List<GameObject>();
        for(int i=0;i<GameController.noOfPlayers;i++)
        foreach(GameObject g in collidedObjects[i])
            fullList.Add(g);
        for(int i=0;i<totalpawns;i++){
            GameObject g=fullList[i];
            g.transform.position = positionsForPawns[fullList.Count-1][totalpawns- i-1];
            if(totalpawns<2)
                g.transform.localScale = new Vector3(0.4F,0.2f,0.4F);
            else
                g.transform.localScale = new Vector3(0.3F,0.2f,0.3F);
        }
        fullList.Clear();
    }
    private Vector3[][] getPositionOfPawn(){

        float x3=this.gameObject.transform.position.x+0.065F;
        float x4=x3+0.22575F;
        float x5=x4+0.22575F;
        float x2=x3-0.22575F;
        float x1=x2-0.22575F;

        float y = 64.195F;

        float z3=this.gameObject.transform.position.z+0.35F;
        float z4=z3+0.184F;
        float z2=z3-0.284F;
        float z1=z2-0.284F;

        Vector3 p11 = new Vector3(x1,y,z1);
        Vector3 p12 = new Vector3(x2,y,z1);
        Vector3 p13 = new Vector3(x3,y,z1);
        Vector3 p14 = new Vector3(x4,y,z1);
        Vector3 p15 = new Vector3(x5,y,z1);

        Vector3 p32 = new Vector3(x2,y,z3);
        Vector3 p33 = new Vector3(x3,y,z3);
        Vector3 p34 = new Vector3(x4,y,z3);

        Vector3 p41 = new Vector3(x1,y,z4);
        Vector3 p43 = new Vector3(x3,y,z4);
        Vector3 p45 = new Vector3(x5,y,z4);

        Vector3[] pawns1 = {p34};
        Vector3[] pawns2 = {p32,p34};
        Vector3[] pawns3 = {p32,p34,p12};
        Vector3[] pawns4 = {p32,p34,p12,p14};    
        Vector3[] pawns5 = {p41,p45,p33,p11,p15}; 
        Vector3[] pawns6 = {p41,p43,p45,p11,p13,p15};  

        Vector3[][] pawns = {pawns1,pawns2,pawns3,pawns4,pawns5,pawns6};
        return pawns;
    }

}
