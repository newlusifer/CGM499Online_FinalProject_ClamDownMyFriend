using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goPlay()
    {
        SceneManager.LoadScene("gamePlay");
    }

    public void goTutorial()
    {
        SceneManager.LoadScene("tutorial");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("title");
    }
}
