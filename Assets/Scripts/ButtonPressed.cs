using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ButtonPressed : MonoBehaviour
{

    public GameObject selectNoOfPlayersCube;
    public GameObject buffering;
    public GameObject[] textMeshToHide;

    int timer;
    // Start is called before the first frame update
    void Start()
    {
        timer=0;
       //selectNoOfPlayersCube.SetActive(false);
    }
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(selectNoOfPlayersCube.active){
                selectNoOfPlayersCube.SetActive(false);
                EorDtextMesh(true);

           }
           else if(timer==0){
               timer=1;
               Invoke("StopTimer",1.0f);
           }
           else if(timer==1){
                Application.Quit();
           }
        }
    }
    void StopTimer(){
        timer=0;
    }
    private void EorDtextMesh(bool check){
        foreach (GameObject item in textMeshToHide)
        {
            item.SetActive(check);
        }
    }
     private void LoadLevel(){
        
        //non online game
        if(persistantdatas.Instance.levelNum ==1 || persistantdatas.Instance.levelNum==2 ){
            buffering.SetActive(true);
            SceneManager.LoadScene("GameScreen");
        }
        if(persistantdatas.Instance.levelNum==3){
            buffering.SetActive(true);
            NetworkManager.Instance.ConnectToPhoton();
        }
        
    }
    public void OnMouseDown()
    {
        switch (gameObject.name)
        {
            case("one"):
                persistantdatas.Instance.noOfPlayers=2;
                LoadLevel();
                break;
            case("two"):
                persistantdatas.Instance.noOfPlayers=3;
                LoadLevel();
                break;
            case("three"):
                persistantdatas.Instance.noOfPlayers=4;
                LoadLevel();
                break;
            default:break;
        }
        /// SelectNoOfplayers is not workiong so have put try catch block
        try{
            if(!selectNoOfPlayersCube.active)
            {
                switch (gameObject.name)
                {
                    case("board"):   
                        persistantdatas.Instance.levelNum=1;
                        EorDtextMesh(false);
                        break;
                    case("board1"):
                        persistantdatas.Instance.levelNum=2;
                        EorDtextMesh(false);
                        break;
                    case("board2"):
                        persistantdatas.Instance.levelNum=3;
                        EorDtextMesh(false);
                        break;
                    case("board3"):
                        persistantdatas.Instance.levelNum=4;
                        EorDtextMesh(false);
                        break;
                    default:
                        break;
                }
                if(persistantdatas.Instance.levelNum>0){
                    selectNoOfPlayersCube.SetActive(true);
                }
            }
        }
        catch(Exception e){
        }
    }
}
