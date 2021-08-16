using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {
    public Animator animator;

    public void FadeToLevel () {
        animator.SetTrigger("FadeOut");
        StartCoroutine(Delay());
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene("unity-2d-game");
    }
}
