using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScripts : MonoBehaviour
{
    private Animator anim;
    AudioSource audio;
    [SerializeField]
    private AudioClip clip;
    bool isMenuOpened;
    int i=0;
    // Start is called before the first frame update
    void Start()
    {
        audio= gameObject.AddComponent<AudioSource>();
        isMenuOpened=false;
        anim=this.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&i==0)
        {
            i=1;
            if(!isMenuOpened){
                audio.PlayOneShot(clip,1.0F);
                anim.Play("menubar", -1, 0f);
                isMenuOpened=!isMenuOpened;
            }
            else
            {
                anim.Play("menubar 0", -1, 0f);
                isMenuOpened=!isMenuOpened;
            }
            Invoke("changeIvalue",0.50f);
        }
    }
    void changeIvalue(){
        i=0;
    }
    public void OpenMenu(){
        if(i==0){
            i=1;
            if(!isMenuOpened){
                audio.PlayOneShot(clip,1.0F);
                anim.Play("menubar", -1, 0f);
                isMenuOpened=!isMenuOpened;
            }
            else
            {
                anim.Play("menubar 0", -1, 0f);
                isMenuOpened=!isMenuOpened;
            }
            Invoke("changeIvalue",0.50f);
        }
    }
    public void OnSettingsButtonPressed(){
        audio.PlayOneShot(clip,1.0F);
    }
    public void OnHelpButtonPressed(){
        audio.PlayOneShot(clip,1.0F);
    }
    public void OnLeaveButtonPressed(){
        audio.PlayOneShot(clip,1.0F);
        if(GameController.gameLevel==3||GameController.gameLevel==4){
            NetworkManager.Instance.OnLeavePressed();
        }
        SceneManager.LoadScene("MainScreen");
    }
}
