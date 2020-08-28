using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator fadeAnimator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            //LoadNextLevel();
        }
        
    }

    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadSceneByID(int id) {
        StartCoroutine(LoadLevel(id));
    }

    IEnumerator LoadLevel(int sceneIndex) {


        fadeAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneIndex);
    }
}
