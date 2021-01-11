using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rintangan : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.tag == "Player") {
            Debug.Log("Kena Duri");
            collision.GetComponent<Pemain>().TerimaDamage(damage);
        }
    } 
}
