using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdMenu : MonoBehaviour
{
    public GameObject AdPanel;
    public Button AdButton;
    //время до активации кнопки рекламы
    float aDcountdown;
    public float startAdCountdown;
    public Text AdCountdownText;
    public Text AdAlarm;
    public RewardAdsSystem rewardSystem;
    [Space]
    [Header("Reward Text In Menu")]
    public Text rewardMoneyText;
    public Text rewardEnergyText;
    public Text rewardDiamondText;

    // Start is called before the first frame update
    void Start()
    {
        aDcountdown = startAdCountdown;
        AdAlarm.gameObject.SetActive(false);


    }


    // Update is called once per frame
    void Update()
    {
        CheckAdButtonActivation();
        
    }

    void ActivateAdAlarm()
    {
        AdAlarm.gameObject.SetActive(true);
        Invoke("AdAlarmOff", 5);
    }

    void AdAlarmOff()
    {
        AdAlarm.gameObject.SetActive(false);
    }

    public void OpenAdMenu()
    {
        AdPanel.SetActive(true);
        Time.timeScale = 0f;
        rewardMoneyText.text = "+ " + rewardSystem.rewardMoney[rewardSystem.countOfReward].ToString();
        rewardEnergyText.text = "+ " + rewardSystem.rewardEnergy[rewardSystem.countOfReward].ToString();
        rewardDiamondText.text = "+ " + rewardSystem.rewardDiamond[rewardSystem.countOfReward].ToString();

    }

    public void CloseAdMenu()
    {
       AdPanel.SetActive(false);
       Time.timeScale = 1f;
    }

    public string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void CheckAdButtonActivation()
    {
        if (rewardSystem.countOfReward >= 7)
        {
            AdButton.interactable = false;
            AdCountdownText.text = FormatTime(0);
            return;
        }

        if (aDcountdown <= 0)
        {
            AdButton.interactable = true;
            AdCountdownText.text = FormatTime(0);
            if (!AdAlarm.gameObject.activeSelf) AdAlarm.gameObject.SetActive(true);
            return;
        }
        else
        {
            AdButton.interactable = false;
        }
        aDcountdown -= Time.deltaTime;
        AdCountdownText.text = FormatTime(aDcountdown);
    }

    public void AdButtonPush()
    {
        CloseAdMenu();
        aDcountdown = startAdCountdown;
    }
}
