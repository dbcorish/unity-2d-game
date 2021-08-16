using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force) {
        rb.AddForce(direction * force);
    }

    void FixedUpdate() {
        if (transform.position.magnitude > 1000f) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        RobotController e = other.collider.GetComponent<RobotController>();

        if (e != null) {
            e.Dance();
        }

        Debug.Log("Projectile collision with " + other.gameObject);
        Destroy(gameObject);
    }
}