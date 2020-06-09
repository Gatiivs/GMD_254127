using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);


    public GameObject projectilePrefab;
    public ParticleSystem damageParticles;


    AudioSource audioSource;
   public AudioClip throwSound;
   public AudioClip playerHitSound;


    // Start is called before the first frame update
    void Start()
    {

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //   QualitySettings.vSyncCount = 0;
        //  Application.targetFrameRate = 10;

        currentHealth = maxHealth;
        //   currentHealth = 1;

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //   Debug.Log(horizontal);

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
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

        /*
               Vector2 position = rigidbody2d.position;
               //  Vector2 position = transform.position;
               position.x = position.x + speed * horizontal * Time.deltaTime;
               position.y = position.y + speed * vertical * Time.deltaTime;
               //    transform.position = position;

               rigidbody2d.MovePosition(position);

    */
        //Launch arrow with X
        if (Input.GetKeyDown(KeyCode.X))
               {
                   Launch();
               }


        //If you press E then you talk
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
            }
        }

    }

    
    void FixedUpdate()
    {

        Vector2 position = rigidbody2d.position;
        //  Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        //    transform.position = position;

        rigidbody2d.MovePosition(position);

    }
    
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)

                return;
          //  animator.SetTrigger("Hit");
            PlaySound(playerHitSound);
            Instantiate(damageParticles, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity); 
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        //  projectile.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);

        float angle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

        // float rotation = lookDirection.x * -90 + (lookDirection.y) * 180;
      //  projectile.transform.Rotate(Vector3.forward * rotation);

        projectile.Launch(lookDirection, 3000);

        animator.SetTrigger("Launch");
        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        //  audioSource.PlayOneShot(clip);
        AudioSource.PlayClipAtPoint(clip, this.transform.position);
    }
}

