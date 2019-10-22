using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to check and verify new ideas
public class checking : MonoBehaviour
{

    public int playerNumber;
    public List<GameObject> pawns = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnMouseDown(){
        for(int i = playerNumber*5 ; i< (playerNumber*5)+5;i++){
            pawns[i].GetComponent<pawnScript>().checkerMove();
        }
    }
}
