using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeNodePositions : MonoBehaviour
{
    
    
    [HideInInspector] public GameObject[] nodes;
    [HideInInspector] public Transform[] targetFinal;
    List<GameObject> freeTargetNodesList = new List<GameObject>();
    bool nodeMovemEntenable;

    private void Update()
    {
        
    }

}




