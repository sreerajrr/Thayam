using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWinners : MonoBehaviour
{
    public GameObject[] medals;
    public Transform players;
    public Transform[] tropyPostions;
    public static List<int> playersWon = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject m in medals){
            m.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GiveTrophy(){
        int place = playersWon.Count; // which is the lastet win position
        int playerNumber = playersWon[place-1];
        if(place<4){
            medals[place].transform.position = new Vector3(tropyPostions[playerNumber].position.x,
                                    65.0F,tropyPostions[playerNumber].position.z);
        }
    }

    public void ChecForWinner(){
        for(int i = 0 ; i< 4;i++){
            for(int j=0;j<5;j++){
                if(
                    players.GetChild(((i*5)+j)).GetComponent<pawnScript>().currentPosition != 27 
                    || 
                    playersWon.Contains(i)){ 
                    // not finished or already won
                    break;
                }
                playersWon.Add(i);
                GiveTrophy();
            }
        }
    }
}
