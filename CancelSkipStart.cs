using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelSkipStart : MonoBehaviour
{
    public bool firstTouch;
    public GameObject skip;
    


    private void OnTriggerEnter()
    {
        if (!firstTouch)
        {
            firstTouch = true;
            skip.SetActive(false);
            Time.timeScale = 1f;
        }
       

    }




   
}
