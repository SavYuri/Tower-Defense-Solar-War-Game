using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//класс свойств пушек в магазине
[System.Serializable]
public class TurretBlueprint
{
    public Node NodeClass;
    public NodeUI nodeUI;
    

    public GameObject samplePrefab;

    public GameObject prefab;
    public int cost;
    public float timeBuildPrefab;

    public GameObject upgradedPrefab;
    public int upgradeCost;
    public float timeUpgradePrefab;

    public GameObject upgradedPrefab2;
    public int upgradeCost2;
    public float timeUpgradePrefab2;

    public GameObject upgradedPrefab3;
    public int upgradeCost3;
    public float timeUpgradePrefab3;

    public void Start()
    {
        NodeClass = Node.nodeClass;
    }

    public int GetSellAmount()
    {
        
        if (nodeUI.target.isUpgraded3)
        {
            return (upgradeCost3 + upgradeCost2 + upgradeCost + cost) / 2;
            
        }

        else if (nodeUI.target.isUpgraded2)
        {
            return (upgradeCost2 + upgradeCost + cost) / 2;
        }

        else if (nodeUI.target.isUpgraded)
        {
            return (upgradeCost + cost) / 2;
        }

        
            return cost / 2;
        


    }

   
}
