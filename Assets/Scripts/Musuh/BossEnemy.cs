using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    #region Public Variables
    public int maxHP;
    public GameObject darah;
    public int damage = 1;
    public int damage2 = 2;
    int currentHP;

    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; //jarak serang
    public float moveSpeed;
    public float timer; //Timer antar serang
    public float timerKebal;
    public Transform leftLimit;
    public Transform rightLimit;
    #endregion

    #region Private Variables
    private Pemain siPemain;
    private RaycastHit2D hit;
    private Transform target;
    private Animator anim;
    private float distance; //jarak antar pemain dan musuh
    private bool attackMode;
    private bool inRange; //Check jika pemain di range
    private bool cooling; //Check jika bisa serang
    private bool kebal;
    private bool isAttack;
    private float intTimer;
    private float intKebal;
    #endregion

    private void Awake()
    {
        SelectTarget();
        intTimer = timer;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHP = maxHP;
        siPemain = FindObjectOfType<Pemain>();
    }

    private void Update()
    {
        if (siPemain == null)
        {
            return;
        }
        else
        {
            if (!attackMode)
            {
                Move();
            }

            if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack")) //jika tidak dalam ruang patroli dan tidak mode serang
            {
                SelectTarget();
            }

            if (inRange)
            {
                hit = Physics2D.Raycast(rayCast.position, -transform.right, rayCastLength, raycastMask);
                RaycastDebugger();
            }

            //When Player is detected
            if (hit.collider != null)
            {
                EnemyLogic();
            }
            else if (hit.collider == null)
            {
                inRange = false;
            }

            if (inRange == false)
            {
                StopAttack();
            }

            if (currentHP <= 150)
            {
                moveSpeed = 3;
                damage = damage2;
                attackDistance = 4;
                timer = 1.25f;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            target = trig.transform;
            inRange = true;

            Flip();

            if (isAttack == true)
            {
                siPemain.TerimaDamage(damage);
            }
        }
        if (trig.gameObject.tag == "FallZone")
        {
            Mati();
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        anim.SetBool("canWalk", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer; //Reset timer
        attackMode = true; //check bisa serang

        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    void RaycastDebugger() //untuk mendeteksi jarak menggunakan raycast
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, -transform.right * rayCastLength, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, -transform.right * rayCastLength, Color.green);
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 0;
        }
        else
        {
            rotation.y = 180;
        }

        transform.eulerAngles = rotation;
    }

    public void KenaDamage(int damage1)
    {
        currentHP -= damage1;

        anim.SetTrigger("hurt");
        Debug.Log("Sisa Darah" + currentHP);

        //jalankan animasi kena damage
        Instantiate(darah, transform.position, Quaternion.identity);
        if (currentHP <= 0)
        {
            GameObject.FindGameObjectWithTag("FinalDoor").SetActive(false);
            Mati();
        }
    }


    void attackplayer()
    {
        isAttack = true;
    }
    void stopattackplayer()
    {
        isAttack = false;
    }

    public void Mati()
    {
        Debug.Log("Musuh Mati !");
        //animasi mati
        anim.SetBool("isDead", true);
        Hapus();
    }

    public void Hapus()
    {
        Destroy(gameObject);
    }
}
