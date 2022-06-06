using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretStatistic : MonoBehaviour
{
    public Text HeadTurretName;
    public Text rangeTurret;
    public Text damageTurret;
    public Text fireRateTurret;
    public Text healthTurret;

    public static TurretStatistic turretStatisticClass;

    private void Awake()
    {
        if (turretStatisticClass != null) return;
        else turretStatisticClass = this;
    }

    void Start()
    {
        

    }

   
}
