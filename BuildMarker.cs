using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMarker : MonoBehaviour
{
    GameObject[] buildNodes;
    GameObject[] buildRoadNodes;
    public static BuildMarker BM;
   
    void Start()
    {
        if (BM != null) return; else BM = this;

        buildNodes = GameObject.FindGameObjectsWithTag("Node");
        buildRoadNodes = GameObject.FindGameObjectsWithTag("roadNode");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowBuildNodes()
    {
        buildNodes = GameObject.FindGameObjectsWithTag("Node");
        BuildRoadNodeLightOff();
        StartCoroutine("BuildNodeLight"); 
    }

    IEnumerator BuildNodeLight()
    {
        for (int i = 0; i<2; i++)
        {
            foreach (GameObject buildNode in buildNodes)
            {
                GameObject childBuildNode = buildNode.transform.Find("Point light").gameObject;
                if (buildNode.GetComponent<Node>().turretInstaled == false)
                {
                    childBuildNode.SetActive(false);
                }
                else
                {
                    childBuildNode.SetActive(true);
                }
               
                
            }
            yield return new WaitForSeconds(0.5f);

            foreach (GameObject buildNode in buildNodes)
            {
                GameObject childBuildNode = buildNode.transform.Find("Point light").gameObject;
                childBuildNode.SetActive(true);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }

    public void BuildRoadNodeLightOn()
    {
        foreach (GameObject buildRoadNode in buildRoadNodes)
        {
           
            if (buildRoadNode.GetComponent<Node>().bombInstaled == false)
            {
                buildRoadNode.GetComponent<RoadNode>().buildLight.SetActive(true);
            }
            else
            {
                buildRoadNode.GetComponent<RoadNode>().buildLight.SetActive(false);
            }
        }
    }

    public void BuildRoadNodeLightOff()
    {
       
        foreach (GameObject buildRoadNode in buildRoadNodes)
        {
            buildRoadNode.GetComponent<RoadNode>().buildLight.SetActive(false);
        }
    }
}
