using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class rolldie : MonoBehaviour
{ public int j,i=0;
   // public bool dierolled;
    public Text text;
    public Transform[] endpointRoad; // Tranforms of starting and end base points
    public Transform[] halfendpointRoad; //Tranform of the other base points

    int start;
    // Start is called before the first frame update
    void Start()
    {
        start=0;
        // j=0;
       GameController.dierolled = false;
        Debug.Log("Working aane");
        // foreach(Transform t in endpointRoad){
        //          t.gameObject.AddComponent<endpointScripts>();
        // }

        // invoking this below method will run all pawns from 1 point to another one by one
        InvokeRepeating("movePlayers",2F,0.5F);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    // public void rollthedie()
    // {
        
    //     if (GameController.canrolldie)
    //     {
    //         i = Random.Range(1, 7);
    //          GameController.dievalue.Add(i);
    //          if(!(i==1 || i == 5 || i==6)){
    //             GameController.dierolled = true;
    //             GameController.canrolldie = false;
    //          }
             
    //         Debug.Log("sssdddddddddddddddddddaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+i);
    //     }
 
    // }

    public GameObject[] pawnObjects; // game objects of all pawn

    public Transform[] waypoint; // Transform of roadways

    /*x=0 -> blue player (player 1);
    x=1 -> purple player (player 2);
    x=2 -> red player (player 3);
    x=3 -> green player (player 4); */
    public void movePlayers(){
        Debug.Log("insidemovePlayers");
        if(j<26){
        Vector3 p = new Vector3(waypoint[j].position.x,waypoint[j].position.y,waypoint[j].position.z);
        Debug.Log("level 1");
        // for(int m=0;m<20;m++)
            pawnObjects[i].transform.position = p;
        }
        else if(j==26){
            pawnObjects[i].transform.position = halfendpointRoad[getPlayerNumber(pawnObjects[i])].position;
            Debug.Log("level 2");
        }
        else if(j>26&&j<53){
            Vector3 p = new Vector3(waypoint[52-j].position.x,waypoint[52-j].position.y,waypoint[52-j].position.z);
            Debug.Log("level 3");
        pawnObjects[i].transform.position = p;
        //  i++;
        //     pawnObjects[i].transform.position = p;
        }
        else if (j==53){
            pawnObjects[i].transform.position = endpointRoad[getPlayerNumber(pawnObjects[i])].position;
        }
        // Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+waypoint[j]);
        // pawnObjects[i].GetComponent<pawnScript>().checkForExchange(j<26?j:52-j);
         i++;
        
        if(i>=20){
           i=0;
           j++;
       }
       if(j==54){
           j=0;
       }

    }
    void movePlayerss(){
         CancelInvoke();
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

}
