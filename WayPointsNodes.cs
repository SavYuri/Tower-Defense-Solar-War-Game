using UnityEngine;
using System.Collections;

public class WayPointsNodes : MonoBehaviour
{
    public Transform[] points;
    public bool CreateNodeAnable;
    public GameObject nodePrefabe;
    public int countOfNodes;

    private void Awake()
    {
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }

    public void Start()
    {
        if (CreateNodeAnable)
        {
            StartCoroutine(NodesCreator());
        }
    }

    IEnumerator NodesCreator()
    {
        for (int i = 0; i < countOfNodes; i++)
        {
            GameObject node = Instantiate(nodePrefabe, transform.GetChild(0).gameObject.transform.position, Quaternion.identity);
            node.GetComponent<NodeMovement>().wayPointsNodes = this;
            yield return new WaitForSeconds(3);
        }
       

        yield break;
    }

}