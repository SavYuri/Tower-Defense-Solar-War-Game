using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBuildUI : MonoBehaviour
{
    
    Node nodeClass;
    BuildManager buildManager;
    NodeUI nodeUI;
    public GameObject confirmBuildPanelUI;
    public Text confirmUIText;
    public static ConfirmBuildUI confirmBuildUIClass;
    private void Awake()
    {
        
        if (confirmBuildUIClass != null)
        {
            return;
        }
        confirmBuildUIClass = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nodeClass = Node.nodeClass;
        buildManager = BuildManager.instance;
    }

    
    
  
}
