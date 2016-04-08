using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
    public Canvas Main;
    public Canvas Options;
    public Canvas Rules;
    public Canvas Loading;
  

    void Awake()
    {
        Options.enabled = false;
        Rules.enabled = false;
        Loading.enabled = false;
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
        StartCoroutine(waiter());
        Main.enabled = false;
        Loading.enabled = true;
    }

    IEnumerator waiter()
    {
        int wait_time = Random.Range(5, 15);
        yield return new WaitForSeconds(wait_time);
        print("I waited for " + wait_time + "sec");
        Application.LoadLevel(1);
    }
}
