using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Image image = GetComponent<Image>();
        image.fillAmount = 0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Image image = GetComponent<Image>();
        image.fillAmount = Mathf.MoveTowards(image.fillAmount, 1f, Time.deltaTime * .10f);   
        if (image.fillAmount == 1f)
        {
            image.fillAmount = 0f;
        }
    }
}
