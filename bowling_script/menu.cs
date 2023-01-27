using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class menu : MonoBehaviour
{
 
    

    public void quit()
    {
        Application.Quit();

    }
   public void scene1()
    {
        SceneManager.LoadScene(1);

    }
}
