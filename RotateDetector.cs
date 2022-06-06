using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDetector : MonoBehaviour
{
    public bool rotateIsCorrect;
    public Transform enemyFixedTatget;

    void OnTriggerEnter(Collider other)
    {
        if (enemyFixedTatget == null) return;

        if (other == enemyFixedTatget.GetComponent<Collider>())
        {
            rotateIsCorrect = true;
        }

        
    }

  void OnTriggerExit(Collider other)
    {
        if (enemyFixedTatget == null) return;

        if (other == enemyFixedTatget.GetComponent<Collider>() || enemyFixedTatget == null)
        {
            rotateIsCorrect = false;
        }

        
    }

}
