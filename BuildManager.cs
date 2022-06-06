using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;

        turretToBuild = null;
    }

   

    public GameObject buildEffect;
    public GameObject sellEffect;
   


    public TurretBlueprint turretToBuild;
    public Node selectedNode;

    public NodeUI nodeUI;
    

   public bool CanBuild { get { return turretToBuild != null; } }
   public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

    public Shop shop;
    Node node;

    private void Start()
    {
        node = Node.nodeClass;
      
        
    }

   

    //меню при выборе пушки
    public void SelectNode (Node node)
    {
        //если уже открыто меню пушки, закрываем меню
        if (selectedNode == node)
        {
            
            DeselectNode();
            return;
        }

        
       

        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void PreBuildNode(Node node)
    {
        selectedNode = node;
        turretToBuild = null;
        nodeUI.PreBuildSetTarget(node);
    }



    public void DeselectNode()
    {
        
        selectedNode = null;
        nodeUI.Hide();
    }

   public void SelectTurretToBuild (TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }
}
