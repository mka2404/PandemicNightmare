using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    public float speed;
    public int damage = 1;
    public float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Pemain>().TerimaDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.tag == "Lantai"){
            Destroy(gameObject);
        }
    }
}
