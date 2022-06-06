using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelected : MonoBehaviour
{
    public GameObject point;
   // public Button button;


   public void EnablePoint()
    {
        GameObject[] buttons;
        buttons = GameObject.FindGameObjectsWithTag("SWLevelButton");
        foreach (GameObject button in buttons)
        {
            button.GetComponent<ButtonSelected>().point.SetActive(false);
        }
        point.SetActive(true);
    }



    
}
