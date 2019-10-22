using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endpointScripts : MonoBehaviour
{
    public Transform[] otherEndpoints;
    List<GameObject> pawns = new List<GameObject>();
    public int[] noOfPawns;
    // Start is called before the first frame update
    void Start()
    {
        noOfPawns= new int[GameController.noOfPlayers];
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnCollisionEnter(Collision col){
            pawns.Add(col.gameObject);
            Arrangepawns();
    }
    public void OnCollisionExit(Collision col){
            pawns.Remove(col.gameObject);
    }
    private void Arrangepawns(){
        int j=0;
        for(int i =0;i<GameController.noOfPlayers;i++){
            noOfPawns[i]=0;
        }
        foreach(GameObject g in pawns){
            Vector3 po = new Vector3(otherEndpoints[j].position.x+0.07491F,64.23F,
                                    otherEndpoints[j].position.z+0.2689F);
            g.transform.position = po;
            g.transform.localScale = new Vector3(0.5F,0.2f,0.5F);
            j++;
            noOfPawns[getPlayerNumber(g)]++;
        }
    }
    private Vector3[] posotions(){
        float x = this.gameObject.transform.position.x+0.097F;
        float x1 = x-0.62F;
        float x2 = x+0.6096F;

        float y = 63.7F;

        float z = this.gameObject.transform.position.z+0.301945F;
        float z1 = z+0.68991F;
        float z2 = z-0.58F;

        Vector3 p1 = new Vector3(x,y,z);
        Vector3 p2 = new Vector3(x1,y,z1);
        Vector3 p3 = new Vector3(x2,y,z1);
        Vector3 p4 = new Vector3(x1,y,z2);
        Vector3 p5 = new Vector3(x2,y,z2);

        Vector3[] poss = {p1,p2,p3,p4,p5};

        return poss;
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

}
