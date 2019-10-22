using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistantdatas : MonoBehaviour
{
    public static persistantdatas Instance=null;
    public int levelNum;
    public int noOfPlayers;
    void Awake(){
        levelNum=0;
        noOfPlayers=0;
        if(Instance==null){
            Instance=this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
