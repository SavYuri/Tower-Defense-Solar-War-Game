using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public GameObject turretPrefab;
    public Transform TurretPrefab;
    public float gizmoRange;
    public bool gizmoSlow = false;


    // Start is called before the first frame update
    void Start()
    {
        //зависимость масштаба главного объекта от дочернего
        float x = 2/turretPrefab.transform.localScale.x;

        if (!gizmoSlow)
        {
            Turret playerScript = turretPrefab.GetComponent<Turret>();
            gizmoRange = playerScript.range * x;
            transform.position = TurretPrefab.position;
            transform.localScale = new Vector3(gizmoRange, gizmoRange, 1);
        }
        else
        {
            TurretSlow playerScript = turretPrefab.GetComponent<TurretSlow>();
            gizmoRange = playerScript.range * x;
            transform.position = TurretPrefab.position;
            transform.localScale = new Vector3(gizmoRange, gizmoRange, 1);
        }
    }

    
}

