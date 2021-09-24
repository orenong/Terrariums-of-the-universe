using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager2 : MonoBehaviour
{
    public void Back()
    {

        SceneManager.LoadScene("Menu");
        Debug.Log("BACK TO MENU");


    }
}
