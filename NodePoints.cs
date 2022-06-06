using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePoints : MonoBehaviour
{
    [HideInInspector]
    public static Transform[] nodesPointsArray;
    [HideInInspector]
    public static NodePoints nodePointsClass;
    public GameObject nodeForTurret;
    GameObject [] nodesArray;
    List<GameObject> nodesList = new List<GameObject>();
    List<Transform> nodesPointsList = new List<Transform>();

    Vector3 uPposition = new Vector3 (0,30,0);




    void Start()
    {
        if (nodePointsClass != null) return; else nodePointsClass = this;

        InitialNodPoint();
        CreateNodes();
    }

    void InitialNodPoint()
    {
        nodesPointsArray = new Transform[transform.childCount];
        nodesArray = new GameObject[nodesPointsArray.Length];
        for (int i = 0; i < nodesPointsArray.Length; i++)
        {
            nodesPointsArray[i] = transform.GetChild(i);
            
        }
       
       
    }

    

    void CreateNodes()
    {
        for (int i = 0; i < nodesPointsArray.Length; i++)
        {
            GameObject _node = Instantiate(nodeForTurret, nodesPointsArray[i].position, Quaternion.identity);
            nodesArray[i] = _node;
           
        }
    }

   

    public Transform FreeNodePoint()
    {

        for (int i = 0; i < nodesPointsArray.Length; i++)
        {
            bool pointIsBusy = nodesPointsArray[i].gameObject.GetComponent<RepairDronePoint>().pointIsBusy;

            if (!pointIsBusy)
            {
                nodesPointsArray[i].gameObject.GetComponent<RepairDronePoint>().pointIsBusy = true;
                return nodesPointsArray[i];
            }

        }
        return null;
    }


    public void MoveAllNodes()
    {
        StartCoroutine(MoveNodesToAnotherPlace());
    }

    IEnumerator MoveNodesToAnotherPlace()
    {
        UpAllNodes();
        yield return new WaitForSeconds(2);
        ToMoveAllNodes();
        yield return new WaitForSeconds(2);
        DownAllNodes();
        yield break;
    }

   

    void UpAllNodes()
    {

        Transform target = null;
        for (int i = 0; i < nodesPointsArray.Length; i++)
        {
            target.position = nodesArray[i].transform.position - uPposition;
            nodesArray[i].GetComponent<RemovableNode>().target = target;

        }
    }

    void DownAllNodes()
    {
        Transform target = null; 
        for (int i = 0; i < nodesPointsArray.Length; i++)
        {
            target.position = nodesArray[i].transform.position - uPposition;
            nodesArray[i].GetComponent<RemovableNode>().target = target;

        }
    }

    public void ToMoveAllNodes()
    {
        nodesList.AddRange(nodesArray);
        nodesPointsList.AddRange(nodesPointsArray);
       

        for (int i = 0; i < nodesPointsArray.Length; i++)
        {
            nodesPointsList[i].gameObject.GetComponent<NodePointPosition>().pointIsBusy = false;
        }

        while (nodesList.Count > 0)
        {
            for (int i = 0; i < nodesPointsList.Count; i++)
            {
                int random = Random.Range(0, nodesPointsList.Count);
                if (nodesPointsList[random].gameObject.GetComponent<NodePointPosition>().pointIsBusy)
                {
                    nodesPointsList.RemoveAt(random);
                    continue;
                }
              
                nodesPointsList[random].gameObject.GetComponent<NodePointPosition>().pointIsBusy = true;
               
                nodesList[0].GetComponent<RemovableNode>().target = nodesPointsList[random];
                nodesPointsList.RemoveAt(random);
                nodesList.RemoveAt(0);
                break;
            }

             
            

        }
    }
}
