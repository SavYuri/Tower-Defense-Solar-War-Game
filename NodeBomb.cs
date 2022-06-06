using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeBomb : MonoBehaviour

{
    public GameObject turret;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //создание некликабельной области у иконок башен
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {

        }


    }
}
