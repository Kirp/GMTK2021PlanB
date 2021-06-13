using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{


    public void LoadStage1()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadStage2()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadStage3()
    {
        SceneManager.LoadScene(3);
    }



}
