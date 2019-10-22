using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerBoard : MonoBehaviour
{
    public GameObject gameController;
    public GameObject[] otherBoards;
    public int playerNumber;
    public bool isEnabled;
    public GameObject[] buttonModes; 
    public GameObject turnIndicator;
    public TextMesh pName;
    SpriteRenderer renderer;
    public void setText(string m ){
        pName.text=m;
    }
    public string getText( ){
        return  pName.text;
    }
    public void canTouch(bool check){
        // CancelInvoke();
        buttonModes[0].SetActive(check);
        buttonModes[1].SetActive(!check);
        isEnabled=check;
    }
    // Start is called before the first frame update
    void Start()
    {
        renderer=GetComponent<SpriteRenderer>();
        
        if(playerNumber==0){
            buttonModes[0].SetActive(true);
            buttonModes[1].SetActive(false);
            turnIndicator.SetActive(true);
            isEnabled=true;
        }
        else{
            buttonModes[0].SetActive(false);
            buttonModes[1].SetActive(true);
            turnIndicator.SetActive(false);
            isEnabled=false;
        }
        if(playerNumber>=GameController.noOfPlayers){
            this.gameObject.SetActive(false);
        }
        if(GameController.noOfPlayers==2){
            this.gameObject.transform.position= new Vector3(
                0.0F,this.gameObject.transform.position.y,this.gameObject.transform.position.z);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        // if(GameController.movesleft>0 && GameController.playerNumber == playerNumber){
        //     isEnabled = true;
        // }
    }
    public void OnMouseDown(){
        if(isEnabled){
            gameController.GetComponent<GameController>().rollthedie();
            buttonModes[0].SetActive(false);
            buttonModes[1].SetActive(true);
            Invoke("TrunOnWithOutEnable",0.1F);
        }
    }
    public void TrunOnWithOutEnable(){
        buttonModes[0].SetActive(true);
        buttonModes[1].SetActive(false);
        if(!isEnabled){
        buttonModes[0].SetActive(false);
        buttonModes[1].SetActive(true);
        turnIndicator.GetComponent<Animator>().enabled=false;
        }
    }

    public void TrunONButton(){
        foreach (GameObject ob in otherBoards)
            {
                ob.GetComponent<playerBoard>().TurnOFFButton();
            }
        buttonModes[0].SetActive(true);
        buttonModes[1].SetActive(false);
        turnIndicator.SetActive(true);
        isEnabled=true;
    }
    public void TurnOnOnlyAnimation(){
        foreach (GameObject ob in otherBoards)
            {
                ob.GetComponent<playerBoard>().TurnOFFButton();
            }
        // animator.GetComponent<Animator>().enabled = true;
         buttonModes[0].SetActive(true);
        buttonModes[1].SetActive(false);
        turnIndicator.SetActive(true);
        isEnabled=false;
    }
    public void TurnOFFButton(){
        // animator.GetComponent<Animator>().enabled = false;
        // Debug.Log(gameObject.name);
        buttonModes[0].SetActive(false);
        buttonModes[1].SetActive(true);
        turnIndicator.SetActive(false);
        isEnabled=false;
        // renderer.color=new Color(1.0F,1.0F,1.0F);
    }

}
