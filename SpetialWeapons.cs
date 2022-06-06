using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpetialWeapons : MonoBehaviour
{

    
    [Header("Back Time Weapon")]
    public float timeBackMove;
    public Button BackTimeButton;
    public AudioClip backTimeSound;



    private void Start()
    {
       

        timeBackMove = PlayerPrefs.GetInt("BackTimeSWLevel") + 1;
    }

    public void BackTimeWeapon()
    {
        AudioSource x = gameObject.GetComponent<AudioSource>();
        x.Play();
       StartCoroutine (BackTime());
    }

   

    IEnumerator BackTime()
    {
        GameObject[] allEnemies;

        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
       
      

        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i] == null)
            {
                continue;
                //yield return null;
            }
           
            else
            {
                allEnemies[i].GetComponent<EnemyMovement>().GetBackWaypoint();
                allEnemies[i].GetComponent<EnemyMovement>().enebleBackTime = true;
            }
            BackTimeButton.interactable = false;

        }

        GameObject[] allFlyEnemies;
        allFlyEnemies = GameObject.FindGameObjectsWithTag("FlyEnemy");

        for (int i = 0; i < allFlyEnemies.Length; i++)
        {
            if (allFlyEnemies[i] == null)
            {
                yield return null;
            }
            else
            {if (allFlyEnemies[i].GetComponent<PathCreation.Examples.PathFollower>() != null)
                allFlyEnemies[i].GetComponent<PathCreation.Examples.PathFollower>().speed *= -1;
               // allFlyEnemies[i].GetComponent<FlyMovement>().enebleBackTime = true;
            }
            BackTimeButton.interactable = false;

        }

        yield return new WaitForSeconds(timeBackMove);


        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i] == null)
            {
                continue;
                //yield return null;
            }
            else
            {
                allEnemies[i].GetComponent<EnemyMovement>().GetNextWaypoint();
                allEnemies[i].GetComponent<EnemyMovement>().enebleBackTime = false;
            }
            BackTimeButton.interactable = true;

        }

        for (int i = 0; i < allFlyEnemies.Length; i++)
        {
            if (allFlyEnemies[i] == null)
            {
                yield return null;
            }
            else
            {
                if (allFlyEnemies[i].GetComponent<PathCreation.Examples.PathFollower>() != null)
                    allFlyEnemies[i].GetComponent<PathCreation.Examples.PathFollower>().speed *= -1;
                //  allFlyEnemies[i].GetComponent<FlyMovement>().enebleBackTime = false;
            }
            BackTimeButton.interactable = true;

        }
        yield break;
    }
}
