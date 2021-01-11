using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : MonoBehaviour
{
    #region Public Variables
    public int maxHP = 100;
    public GameObject darah;
    public GameObject spit;

    public float timeBetweenShots;
    float nextShotTime;
    public Transform spitPoint;

    public int damage = 1;
    int currentHP;

    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; //jarak serang
    public float moveSpeed;
    public float timer; //Timer antar serang
    public Transform leftLimit;
    public Transform rightLimit;

    public Rigidbody2D rb; //nama var
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction = 0;
    public float nilGerak;
    public float retreatDistance;
    public float cooldownTimeDash = 2;
    private float nextcooldown = 0;

    public Transform player;
    #endregion

    #region Private Variables
    private Pemain siPemain;
    private RaycastHit2D hit;
    public Transform target;
    private Animator anim;
    private float distance; //jarak antar pemain dan musuh
    private bool attackMode;
    private bool inRange; //Check jika pemain di range
    private bool cooling; //Check jika bisa serang
    private bool isAttack;
    private float intTimer;
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
        player = siPemain.transform;
    }

    private void Update()
    {

        Dash(0);

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

        

        //Jika pemain terdeteksi
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
        
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance) {
            
            if(Time.time > nextcooldown) {
            Dash(1);
            nextcooldown = Time.time + cooldownTimeDash;
        }
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

    public void Spit() {
        Instantiate(spit, spitPoint.position, transform.rotation);
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
            nilGerak = 1;
        }
        else
        {
            target = rightLimit;
            nilGerak = -1;
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

    public void Dash(int input)
    {
        if (direction == 0)
        {
            if (nilGerak == -1 && input == 1)
            {
                direction = 1;
                print("set kiri");
            }
            else if (nilGerak == 1 && input == 1)
            {
                direction = 2;
                print("set kanan");
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
                print("reset dash");
            }
            else
            {
                dashTime -= Time.deltaTime;

                if (direction == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                    print("dash ke kiri");
                }
                else if (direction == 2)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                    print("dashke kanan");
                }
            }
        }
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
