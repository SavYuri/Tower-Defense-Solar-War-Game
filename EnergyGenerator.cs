using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGenerator : MonoBehaviour
{
    public GameObject rotationPart;
    public int speed = 5;
    public int speedWorld = 5;
    float timer;
    public float cycleTime;
    public int energyReward;
    public Text rewardText;
    public GameObject rewardObj;
    public bool samplePrefab;

    // Start is called before the first frame update
    void Start()
    {
        timer = cycleTime;
        rewardObj.SetActive(false);
    }

    void Update()
    {
        Rotation();
        timer -= Time.deltaTime;
        if (samplePrefab)
        {
            return;
        }
        else
        cycleTimer();
    }

    void Rotation()
    {
        rotationPart.transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
        rotationPart.transform.Rotate(0, speedWorld * Time.deltaTime, 0, Space.World);
    }

    void cycleTimer()
    {
        if (timer <= 0)
        {
            timer = cycleTime;
            PlayerStats.Energy += energyReward;
            rewardText.text = energyReward.ToString();
            rewardObj.SetActive(true);
            Invoke("DisableRewardObj", 3);
        }
    }

    void DisableRewardObj()
    {
        rewardObj.SetActive(false);
    }
}
