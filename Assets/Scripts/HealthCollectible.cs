using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour {
    public AudioClip collectible;

    void OnTriggerEnter2D(Collider2D other) {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null) {
            if (controller.health < controller.maxHealth) {
                controller.ChangeHealth(1);
                Destroy(gameObject);

                controller.PlaySound(collectible);
            }
        }
    }
}