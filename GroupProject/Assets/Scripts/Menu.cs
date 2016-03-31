using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
    public Canvas Main;
    public Canvas Options;

    void Awake()
    {
        Options.enabled = false;
    }
    public void OptionsOn()
    {
        Options.enabled = true;
        Main.enabled = false;
    }
    public void ReturnsOn()
    {
        Options.enabled = false;
        Main.enabled = true;
    }
    public void LoadOn()
    {
        Application.LoadLevel (1);
    }
}
