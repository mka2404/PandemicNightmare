using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Pemain : MonoBehaviour
{
    #region Variables
    public float speed; //Kecepatan
    public Rigidbody2D rb; //nama var
    Animator anim;
    Light2D fear;
    public Vector2 posisiAwal;
    public Transform posx, posy;
    private float radiusLight;
    bool tolehKanan = true;
    public int health;
    public int itemColect = 0;
    public int itemNeeded;
    public float jumpForce, cooldownTime = 2;
    private float nextcooldown = 0, nextcooldown2 = 0;
    //----UNTUK LANTAI
    bool diLantai;
    public Transform cekLantai;
    public float cekRadius;
    public LayerMask deteksiLantai;

    //----untuk dinding
    bool isSentuhDepan;
    public Transform depanCheck;
    bool wallJumping;
    public float wallJumpTime;
    public float xWallForce;
    public float yWallForce;
    bool wallSliding;
    public float wallSlidingSpeed;

    //----- untuk serang ----
    public Transform serangPoint;
    public LayerMask enemyLayer1,enemyLayer2, enemyLayer3;
    public LayerMask cuciLayers;
    public float serangRange = 0.5f;
    public int serangDamage = 20;


    //----- efek
    public GameObject darah;
    public GameObject dropEffect; // efek drop
    public GameObject gameOver;


    //----Suara
    AudioSource source;

    public AudioClip idleSound, runSound, jumpSound, attackSound;


    //---untuk checkPoint
    private Vector2 checkpointPos;

    public string ThisLevel, NextLevel;

    //--- untuk dash
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction = 0;
    public float nilGerak;

    //-- untuk trampolin
    public float TrampolinlaunchForce;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        source.clip = idleSound;
        source.Play();
        source.loop = true;
        fear = GetComponent<Light2D>();
        radiusLight = fear.pointLightOuterRadius;

        dashTime = startDashTime;
    }

    private void Update()
    {

    }

    // Fungsi input gerak
    public void Move(float input)
    {
        //untuk bergerak
        rb.velocity = new Vector2(speed * input, rb.velocity.y);

        //untuk flip sprite
        if (input > 0 && tolehKanan == false)
        {
            Flip();
        }
        else if (input < 0 && tolehKanan == true)
        {
            Flip();
        }

        //untuk animasi
        if (input != 0)
        {
            anim.SetBool("isLari", true);
        }
        else
        {
            anim.SetBool("isLari", false);
        }
    }
    public void Jump(float input)
    {
        diLantai = Physics2D.OverlapCircle(cekLantai.position, cekRadius, deteksiLantai);
        if (diLantai == true && input == 1)
        {
            anim.SetTrigger("Lompat");
            SuaraLompat();
            rb.velocity = Vector2.up * jumpForce;
        }

        if (diLantai == true)
        {
            anim.SetBool("isLompat", false);
        }
        else
        {
            anim.SetBool("isLompat", true);
        }
    }

    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        tolehKanan = !tolehKanan;
    }

    public void Serang()
    {
        if (Time.time > nextcooldown)
        {

            anim.SetTrigger("Serang"); //trigger animasi
            SuaraSerang();

            Collider2D[] hitEnemies1 = Physics2D.OverlapCircleAll(serangPoint.position, serangRange, enemyLayer1); //deteksi musuh1
            Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(serangPoint.position, serangRange, enemyLayer2); //deteksi musuh2
             Collider2D[] hitEnemies3 = Physics2D.OverlapCircleAll(serangPoint.position, serangRange, enemyLayer3); //deteksi musuh3
            Collider2D hitTempatCuci = Physics2D.OverlapCircle(serangPoint.position, serangRange, cuciLayers); //deteksi tempat cuci tangan
            //damage musuh
            foreach (Collider2D enemy in hitEnemies1)
            {
                enemy.GetComponent<Enemy>().KenaDamage(serangDamage);
            }

            foreach (Collider2D enemy in hitEnemies2) {
                print("hit musuh archer");
                enemy.GetComponent<ArcherEnemy>().KenaDamage(serangDamage);
            }

            foreach (Collider2D enemy in hitEnemies3) {
                print("hit musuh archer");
                enemy.GetComponent<BossEnemy>().KenaDamage(serangDamage);
            }

            if (hitTempatCuci)
            {
                nonFear();
            }

            print("cooldown !");
            nextcooldown = Time.time + cooldownTime;
        }

    }

    public void Serang2()
    {
        if (Time.time > nextcooldown2)
        {

            anim.SetTrigger("Serang2"); //trigger animasi
            SuaraSerang();

            Collider2D[] hitEnemies1 = Physics2D.OverlapCircleAll(serangPoint.position, serangRange, enemyLayer1); //deteksi musuh1
            Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(serangPoint.position, serangRange, enemyLayer2); //deteksi musuh2
            Collider2D[] hitEnemies3 = Physics2D.OverlapCircleAll(serangPoint.position, serangRange, enemyLayer3); //deteksi musuh3
            Collider2D hitTempatCuci = Physics2D.OverlapCircle(serangPoint.position, serangRange, cuciLayers); //deteksi tempat cuci tangan
            //damage musuh
            foreach (Collider2D enemy in hitEnemies1)
            {
                enemy.GetComponent<Enemy>().KenaDamage(serangDamage + 20);
            }

            foreach (Collider2D enemy in hitEnemies2) {
                print("hit musuh archer");
                enemy.GetComponent<ArcherEnemy>().KenaDamage(serangDamage + 20);
            }

            foreach (Collider2D enemy in hitEnemies3) {
                print("hit musuh archer");
                enemy.GetComponent<BossEnemy>().KenaDamage(serangDamage + 20);
            }

            if (hitTempatCuci)
            {
                nonFear();
            }

            print("cooldown !");
            nextcooldown2 = Time.time + cooldownTime + 2;
        }

    }

    //---terima damage
    public void TerimaDamage(int damage)
    {

        health -= damage;
        print(health);

        Instantiate(darah, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            gameOver.SetActive(true);
            Destroy(gameObject);
        }
        else
        {
            Fear();
        }
        anim.SetTrigger("Knock");
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

    //----- trigger item
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collect"))
        {
            print("Kena");
            Destroy(collision.gameObject);
            itemColect += 1;
            if (health < 5)
            {
                health++;
            }
            if (itemColect == itemNeeded)
            {
                print("Door Terbuka");
                GameObject.FindGameObjectWithTag("Door").SetActive(false);
                // ToNextLevel();
            }
        }
        if (collision.CompareTag("FallZone"))
        {
            print("Jatuh");
            transform.position = checkpointPos;
            TerimaDamage(1);
        }
        if (collision.CompareTag("CheckPoint"))
        {
            checkpointPos = collision.transform.position;
        }
        
    }

    //--- collision
    public void OnCollisionEnter2D(Collision2D other) {
        
        if (other.gameObject.CompareTag("JumpPad"))
        {
            print("kena trampolin");
            rb.velocity = Vector2.up * TrampolinlaunchForce;
        }
    }
    //--- tutup
    void OnDrawGizmosSelected()
    {
        if (serangPoint == null) return;
        Gizmos.DrawWireSphere(serangPoint.position, serangRange);
    }


    //------ func suara
    #region Suara
    public void SuaraLari()
    {
        source.clip = runSound;
        source.Play();
        source.loop = false;
    }
    public void SuaraIdle()
    {
        source.clip = idleSound;
        source.Play();
        source.loop = true;
    }
    public void SuaraSerang()
    {
        source.clip = attackSound;
        source.Play();
        source.loop = false;
    }
    public void SuaraLompat()
    {
        source.clip = jumpSound;
        source.Play();
        source.loop = false;
    }
    #endregion
    //------ tutup

    public void Land()
    {
        Vector2 pos = new Vector2(cekLantai.position.x, cekLantai.position.y);
        Instantiate(dropEffect, pos, Quaternion.identity);
        SuaraIdle();
    }

    //------ fitur fear
    #region Fear
    public void Fear()
    {
        if (radiusLight == 5)
        {
            radiusLight = 5;
        }
        else if (radiusLight > 5)
        {
            radiusLight -= 1;
            fear.pointLightOuterRadius = radiusLight;
        }
    }

    public void nonFear()
    {
        if (radiusLight == 10)
        {
            radiusLight = 10;
        }
        else if (radiusLight < 10)
        {
            radiusLight += 1;
            fear.pointLightOuterRadius = radiusLight;
        }
    }
    #endregion

    public void Restart()
    {
        SceneManager.LoadScene(ThisLevel);
    }

    public void ToNextLevel()
    {
        SceneManager.LoadScene(NextLevel);
    }

    //------ func cooldown skill;
    // if (Time.time > nextcooldown) {
    //     print("cooldown !");
    //     nextcooldown = Time.time + cooldownTime;
    // }
}
