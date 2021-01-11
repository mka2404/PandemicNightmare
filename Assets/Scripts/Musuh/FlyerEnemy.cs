using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerEnemy : MonoBehaviour
{
    #region variables

    public int maxHP = 100;
    public Pemain siPemain;
    public GameObject darah;
    public GameObject player;
    private Transform playerpos;
    private Vector2 currentPos;
    private Animator enemyAnim;
    public float distance; //jarak antar pemain dan musuh
    public int damage = 2;
    int currentHP;

    public float attackDistance; //jarak serang
    public float moveSpeed;
    public Transform leftLimit;
    public Transform rightLimit;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerpos = player.GetComponent<Transform>();
        currentPos = GetComponent<Transform>().position;
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (siPemain == null)
        {
            return;
        }
        else
        {
            if (Vector2.Distance(transform.position, playerpos.position) < distance)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerpos.position, moveSpeed * Time.deltaTime);
                enemyAnim.SetBool("Chase", true);
            }
            else
            {
                if (Vector2.Distance(transform.position, currentPos) <= 0)
                {
                    enemyAnim.SetBool("Chase", false);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, currentPos, moveSpeed * Time.deltaTime);
                    enemyAnim.SetBool("Chase", false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            siPemain.TerimaDamage(damage);
            Destroy(gameObject);
        }

    }
}
