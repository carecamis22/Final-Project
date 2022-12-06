using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public TextMeshProUGUI FixText;
    public GameObject winText;
    public GameObject loseText;
    public TextMeshProUGUI ammoText;
    
    
    public int maxHealth = 5;
    public static int currentAmmo;
    public static int robots = 0;
    
    public GameObject projectilePrefab;
    public GameObject damage;
    public GameObject healthincrease;
    
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip frogSound;
    public AudioClip collisionSound;

    public int ammo { get { return currentAmmo; } }
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
        robots = 0;
       // SetCountText ();
       currentAmmo = 5;

    }

    // Update is called once per frame
    void Update()
    {
        winText.SetActive(false);
        loseText.SetActive(false);
        ChangeScore();
        LoseGame();
        AmmoCount();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C) && ammo > 0)
        {
            currentAmmo--;
            Launch();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                //if(robots >= 5 && SceneManager.GetActiveScene().name != "Untitled")
                //{
                //    SceneManager.LoadScene("Untitled");
                //}
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    //NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    character.DisplayDialog();
                    PlaySound(frogSound);
                }
            }
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

     public void ChangeScore()
     {
        FixText.text = "Robots Fixed: " + robots + "/5";

        if (robots >= 5)
        {
            winText.SetActive(true);
            PlaySound(winSound);

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
                robots = 0;
            }
        }
     }

     public void OnCollisionEnter2D(Collision2D collision)
     {
        if (collision.gameObject.tag == "lilcat")
        {
            PlaySound(collisionSound);
        }
        if (collision.gameObject.tag == "slowcat")
        {
            speed = 1.5f;
        }
        if(collision.gameObject.tag == "faststar")
        {
            speed = 4.0f;
           Destroy (GameObject.FindWithTag("faststar"));
       }

             if(collision.gameObject.tag == "faststarr")
        {
            speed = 4.0f;
           Destroy (GameObject.FindWithTag("faststarr"));
       }      

       if(collision.gameObject.tag == "faststarrr")
        {
            speed = 4.0f;
           Destroy (GameObject.FindWithTag("faststarrr"));
       }

      if(collision.gameObject.tag == "Pickup")
         {
            currentAmmo += 1;
         Destroy (GameObject.FindWithTag("Pickup"));
       }

             if(collision.gameObject.tag == "pickupp")
         {
            currentAmmo += 1;
         Destroy (GameObject.FindWithTag("pickupp"));
       }

             if(collision.gameObject.tag == "pickuppp")
         {
            currentAmmo += 1;
         Destroy (GameObject.FindWithTag("pickuppp"));
       }

      if(collision.gameObject.tag == "pickupppp")
         {
            currentAmmo += 1;
         Destroy (GameObject.FindWithTag("pickupppp"));
       }

                   if(collision.gameObject.tag == "pickupup")
         {
            currentAmmo += 1;
         Destroy (GameObject.FindWithTag("pickupup"));
       }
     }



    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            GameObject projectileObject = Instantiate(damage, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            PlaySound(hitSound);
        }


      currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);  

       if (currentHealth <= 0)
       {
           loseText.SetActive(true);
       }


        if (currentHealth >+ 1)
       {
             GameObject projectileObject = Instantiate(healthincrease, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
     }

    }


    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        
        PlaySound(throwSound);
    } 
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void LoseGame()
    {
                if (currentHealth <= 0)
        {
            loseText.SetActive(true);
            speed = 0f;
            PlaySound(loseSound);

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
                robots = 0;
            }
        }
    }

    public void AmmoCount()
    {
        ammoText.text = "Cogs: " + currentAmmo;
    }


}

