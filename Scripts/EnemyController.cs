using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3;
    public ParticleSystem smokeEffect;


    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool alive = true;


    AudioSource walkingSource;

    public AudioClip kaboomClip;


    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        walkingSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //remember ! inverse the test
        if (!alive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
           
            if (Random.value < 0.5f)
                direction = -1;
            else
                direction = 1;

            if (Random.value < 0.5f)
                vertical = true;
            else
                vertical = false;


            timer = changeTime = Random.Range(1.0f, 10.0f);
        }
    }
        
    void FixedUpdate()
        {

        if (!alive)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;
        if (vertical)
        {

            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            position.y = position.y + Time.deltaTime * speed * direction;
        }
        else
        {

            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
            position.x = position.x + Time.deltaTime * speed * direction;
        }

        rigidbody2D.MovePosition(position);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Kill()
    {
        alive = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Dead");
        walkingSource.Stop();
        AudioSource.PlayClipAtPoint(kaboomClip, this.transform.position);
        Destroy(gameObject, 1);
    }

}
