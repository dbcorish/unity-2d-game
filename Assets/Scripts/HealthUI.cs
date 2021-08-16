using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthUI : MonoBehaviour {
    VisualElement HealthBar;
    VisualElement[] Hearts;

    VisualElement nextHeart;

    // Start is called before the first frame update
    void Start() {
        HealthBar = GetComponent<UIDocument>().rootVisualElement.Q("Container");
        Hearts = HealthBar.Children().ToArray();
    }

    public void StartHealthBar(int currentHealth, int maxHealth) {
        for (int i = maxHealth - 1; i >= currentHealth; i--) {
            Hearts[i].style.visibility = Visibility.Hidden;
        }
    }

    public void AnimateHealthBar(bool increaseHealth) {
        if (increaseHealth) {
            nextHeart = Hearts.Where(x => !x.visible).First();
            nextHeart.style.visibility = Visibility.Visible;
        } else {
            nextHeart = Hearts.Where(x => x.visible).Last();
            nextHeart.style.visibility = Visibility.Hidden;
        }
    }
}