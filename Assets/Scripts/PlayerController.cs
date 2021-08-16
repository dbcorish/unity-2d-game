using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    public float speed = 3.0f;

    Rigidbody2D rb;
    float horizontal;
    float vertical;
    bool canLaunch = true;

    public int maxHealth = 4;
    public int health {
        get {
            return currentHealth;
        }
    }
    int currentHealth;

    HealthUI HealthUI;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public GameObject projectilePrefab;

    Vector2 direction;

    AudioSource audioSource;
    AudioSource backgroundMusicAudioSource;

    public AudioClip throwClip;
    public AudioClip hitClip;
    public AudioClip gameOver;

    public GameObject LevelChangerObject;
    public GameObject backgroundMusic;

    private PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = 1;
        HealthUI = GetComponent<HealthUI>();
        HealthUI.StartHealthBar(currentHealth, maxHealth);
        audioSource = GetComponent<AudioSource>();
        backgroundMusicAudioSource = backgroundMusic.GetComponent<AudioSource>();
        playerControls.Player.Launch.performed += _ => Launch();
        playerControls.Player.TouchLaunch.performed += _ => TouchLaunch();
    }

    // Update is called once per frame
    void Update() {
            Vector2 movementInput = playerControls.Player.Move.ReadValue<Vector2>();

            horizontal = movementInput.x;
            vertical = movementInput.y;

            if (horizontal < 0) {
                transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            } else if (horizontal > 0) {
                transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);
            }

            if (isInvincible) {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0) {
                    isInvincible = false;
                }
            }
    }

    void FixedUpdate() {
        Vector2 position = rb.position;

        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rb.MovePosition(position);
    }

    public void ChangeHealth(int amount) {
        if (amount < 0) {
            if (isInvincible) {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hitClip);
            HealthUI.AnimateHealthBar(false);
        } else if (amount > 0) {
            HealthUI.AnimateHealthBar(true);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 0) {
            backgroundMusicAudioSource.Stop();
            PlaySound(gameOver);
            canLaunch = false;
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            LevelChanger LevelChanger = LevelChangerObject.GetComponent<LevelChanger>();
            LevelChanger.FadeToLevel();
        }
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    void Launch() {
        if (canLaunch) {
            Vector2 position = playerControls.Player.Aim.ReadValue<Vector2>();
            Debug.Log(position);
            if(position.x > 375 || position.y > 270) {
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(position);
                direction = (Vector2)((worldMousePos - transform.position));
                direction.Normalize();

                GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);

                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.Launch(direction, 600);
                PlaySound(throwClip);
            }
        }
    }

    void TouchLaunch() {
        if(canLaunch) {
            Vector2 position = playerControls.Player.TouchAim.ReadValue<Vector2>();
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(position);
            direction = (Vector2)((worldMousePos - transform.position));
            direction.Normalize();

            GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(direction, 600);
            PlaySound(throwClip);
        }
    }

    public void PlaySound(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }
}