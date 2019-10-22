using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour
{
    private Text myDie,o1,o2,o3;
    // Start is called before the first frame update
    void Start()
    {
        myDie=gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        o1=gameObject.transform.GetChild(2).gameObject.GetComponent<Text>();
        o2=gameObject.transform.GetChild(3).gameObject.GetComponent<Text>();
        o3=gameObject.transform.GetChild(4).gameObject.GetComponent<Text>();
        Debug.Log("22222222222222222222222222222222222222222222222222");
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void closeButton(){
       this.gameObject.SetActive(false);
   }
   private void addDieValue(Text tx , int val){
       
        // string txt = tx.text+"";
        // Debug.Log(val);
        // string sVal = val+"";
        tx.text += "  , "+ val;
        // tx.text = txt;
   }

    public void addDieValues(int val){
        addDieValue(myDie,val);
    }
    
    public void addOpp(int currentPlayer,int val){
        if(currentPlayer==1){
            addDieValue(o1,val);
        }
        else if(currentPlayer==2){
            addDieValue(o2,val);
        }
        else if(currentPlayer==3){
            addDieValue(o3,val);
        }
        
    }
}
