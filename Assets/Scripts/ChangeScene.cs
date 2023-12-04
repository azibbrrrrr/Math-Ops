using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startgame : MonoBehaviour
{
    public void changeScene(String scene)
    {
        SceneManager.LoadScene(scene);
    }
}
