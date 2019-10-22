using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHolderTouch : MonoBehaviour
{


    public GameObject[] otherHolders;
    public GameObject contoller;
    // public GameObject[] touchTurns;
    Renderer renderer;

    bool canTouch;
    
    // Start is called before the first frame update
    void Start()
    {
        if(GameController.gameLevel==1||GameController.gameLevel==2)
            canTouch=true;
        if(GameController.gameLevel==3||GameController.gameLevel==4)
            if(GameManagerGame.Instance.currentPlayerIndex==0)
                canTouch=true;
        renderer = GetComponentInChildren<MeshRenderer>();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.gameLevel==3||GameController.gameLevel==4)
            if(GameManagerGame.Instance.currentPlayerIndex!=0)
                canTouch=false;
    }
    
    public void OnMouseDown() {
        if(canTouch){
            
            int val = getValue();
            TurnedOnColor();
            contoller.GetComponent<GameController>().SetAnimation(val,gameObject);
            // renderer.material.shader = Shader.Find("_Color");
        }
    }

    public void TurnedOnColor(){
        int val = getValue();
        foreach (GameObject item in otherHolders)
        {
            if(item.name!=this.gameObject.name && item.active)
            item.GetComponent<PositionHolderTouch>().resetIt();
        }
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        if(val!=6){
            gameObject.transform.GetChild(1).gameObject.transform.GetChild(val==10?5:val-1).gameObject.SetActive(true);
            gameObject.transform.GetChild(0).gameObject.transform.GetChild(val==10?5:val-1).gameObject.SetActive(false);
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void resetIt(){
        if(this.gameObject.active){
            int val = getValue();
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            if(val!=6){
                gameObject.transform.GetChild(0).gameObject.transform.GetChild(val==10?5:val-1).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.transform.GetChild(val==10?5:val-1).gameObject.SetActive(false);
            }
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            
            // this.renderer.material.color=new Color(1.0F,1.0F,1.0F);
        }
    }
    public void canTouchthis(bool canTouchit){
        canTouch=canTouchit;
    }
    int getValue(){
        if(gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.active || 
                        gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.active){
            return 1;
        }
        else if(gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.active || 
                        gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.active){
            return 2;
        }
        else if(gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.active || 
                        gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.active){
            return 3;
        }
        else if(gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.active || 
                        gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).gameObject.active){
            return 4;
        }
        else if(gameObject.transform.GetChild(0).gameObject.transform.GetChild(4).gameObject.active || 
                        gameObject.transform.GetChild(1).gameObject.transform.GetChild(4).gameObject.active){
            return 5;
        }
        else if(gameObject.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject.active || 
                        gameObject.transform.GetChild(1).gameObject.transform.GetChild(5).gameObject.active){
            return 10;
        }
        return 6;
    }
}
