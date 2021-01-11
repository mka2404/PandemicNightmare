using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KontrolSentuh : MonoBehaviour
{
    private Pemain siPemain;
    public bool isGerak = false; //kondisi
    public float nilaiGerak; 

    public int jumlahDarah;
    public Image[] darah;
    public Sprite fullDarah;
    public Sprite brokenDarah;
    public GameObject itemscol;
    public float cooldownTimeDash = 2;
    private float nextcooldown = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        siPemain = FindObjectOfType<Pemain>();
    }

    void Update () {
        if (siPemain == null) {
            for (int i = 0; i < darah.Length; i++) {
                if (i < jumlahDarah){
                    darah[i].enabled = true;
                } else {
                    darah[i].enabled = false;
                }
                if (i < siPemain.health) {
                    darah[i].sprite = fullDarah;
                } else {
                    darah[i].sprite = brokenDarah;
                }
            }
        } 
        else {
            itemscol.gameObject.GetComponent<Text>().text = ("x"+siPemain.itemColect+"/"+siPemain.itemNeeded);
            
            if (isGerak){
            siPemain.Move(nilaiGerak); //jika bergerak
            }
            else if (!isGerak){
                siPemain.Move(0); //jika diam
            }

            siPemain.Jump(0);
            siPemain.Dash(0);

            if (siPemain.health > jumlahDarah) {
                siPemain.health = jumlahDarah;
            }

            for (int i = 0; i < darah.Length; i++) {
                if (i < jumlahDarah){
                    darah[i].enabled = true;
                } else {
                    darah[i].enabled = false;
                }
                if (i < siPemain.health) {
                    darah[i].sprite = fullDarah;
                } else {
                    darah[i].sprite = brokenDarah;
                }
            }
            
        }
        
    }

    public void PanahKiri(){
        nilaiGerak = -1;
        siPemain.nilGerak = nilaiGerak;
        isGerak = true;
        siPemain.SuaraLari();
    }

    public void PanahKanan(){
        nilaiGerak = 1;
        siPemain.nilGerak = nilaiGerak;
        isGerak = true;
        siPemain.SuaraLari();
    }

    public void TidakGerak(){
        if (siPemain != null) {
            isGerak = false;
            siPemain.SuaraIdle();
        }
    }

    public void Jump(){   
        siPemain.Jump(1);
    }

    public void Serang(){
        siPemain.Serang();
    }

    public void Serang2(){
        siPemain.Serang2();
    }

    public void Pause(){
        siPemain.Restart();
    }

    public void Dash(){
        if(Time.time > nextcooldown) {
            siPemain.Dash(1);
            nextcooldown = Time.time + cooldownTimeDash;
        }
    }

    //------ func cooldown skill;
    // if (Time.time > nextcooldown) {
    //     print("cooldown !");
    //     nextcooldown = Time.time + cooldownTime;
    // }

    
}
