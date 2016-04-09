using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour {

    public Canvas QuitCanvas;
    public Canvas Main;

    void Awake()
    {
         QuitCanvas.enabled = false;
    }
  
    public void BackMenu()
    {
        QuitCanvas.enabled = true;
        Main.enabled = false;
    }
    public void BackMenuQuit()
    {
        Application.LoadLevel(0);
    }
    public void BackMenuCancel()
    {
        QuitCanvas.enabled = false;
        Main.enabled = true;
    }
}
