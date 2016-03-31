using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
    public Canvas Main;
    public Canvas Options;
    public Canvas Rules;


    void Awake()
    {
        Options.enabled = false;
        Rules.enabled = false;
    }
    public void OptionsOn()
    {
        Options.enabled = true;
        Main.enabled = false;
        Rules.enabled = false;
    }
    public void ReturnsOn()
    {
        Options.enabled = false;
        Main.enabled = true;
        Rules.enabled = false;
    }

    public void RulesOn()
    {
        Rules.enabled = true;
        Main.enabled = false;
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void LoadOn()
    {
        Application.LoadLevel (1);
    }
}
