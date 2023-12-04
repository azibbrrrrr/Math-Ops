using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Image image;

    public void ChangeScene(string scene)
    {
        StartCoroutine(FadeOut(scene));
        Debug.Log("fadeout");
    }

    private IEnumerator FadeOut(string scene)
    {
        image.GetComponent<Animator>().SetTrigger("fade_out");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(scene);
    }
}
