using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {
    public float speed;
    public bool vertical;

    Rigidbody2D rb;

    public float changeTime = 3.0f;
    float timer;
    int direction = 1;

    Animator animator;

    bool walking = true;

    AudioSource audioSource;

    public AudioClip dancingClip;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        timer -= Time.deltaTime;

        if (timer < 0) {
            timer = changeTime;
            direction = -direction;
        }
    }

    void FixedUpdate() {
        if (!walking) {
            return;
        }

        Vector2 position = rb.position;

        if (vertical) {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0f);
            animator.SetFloat("Move Y", direction);
        } else {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0f);
        }

        rb.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other) {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null) {
            player.ChangeHealth(-1);
        }
    }

    public void Dance()
    {
        walking = false;
        rb.simulated = false;
        animator.SetTrigger("Dancing");
        PlaySound(dancingClip);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.spatialBlend = 0.0f;
        audioSource.PlayOneShot(clip);
    }
}
