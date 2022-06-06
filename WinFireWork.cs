using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinFireWork : MonoBehaviour
{
    Transform [] firePoint;
    public Transform GObjFirePoint;
    public GameObject fireWorkPrefab;
    public static WinFireWork winFireWork;

    private void Awake()
    {
        firePoint = new Transform[GObjFirePoint.childCount];
        for (int i = 0; i < firePoint.Length; i++)
        {
            firePoint[i] = GObjFirePoint.GetChild(i);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (winFireWork != null) return;
        else winFireWork = this;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void StartWinFireWork()
    {
        StartCoroutine(FireWorkLaunch());
       
    }

    IEnumerator FireWorkLaunch()
    {
        GameObject[] fireWork = new GameObject[firePoint.Length];
        
        for (int i = 0; i < firePoint.Length; i++)
        {
           // int randomInt = Random.Range(0, firePoint.Length);
            //i = randomInt;
            yield return new WaitForSeconds(0.001f);
            fireWork[i] = (GameObject)Instantiate(fireWorkPrefab, firePoint[i].position, firePoint[i].rotation);
        }
        yield break;
    }
}
