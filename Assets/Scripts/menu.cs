using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class menu : MonoBehaviour
{

    public void PlayGame()
    {
        manage.loadScene(1);

    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
