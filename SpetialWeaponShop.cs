using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpetialWeaponShop : MonoBehaviour
{
    // 0-BackMove; 1-MissleAttack; 2-Bomb; 3-AttackBomb; 4-WarShip

    public Button [] shopPlusBuy;
    public Button[] spetialWeaponsButtons;
    public Text[] swCountText;
    public Text[] swPriceText;
    public int[] swPrice;

    public int[] swCount = new int[6];
    float timerHideShopButtons;

    public GameObject shopPlusPanel;
    public GameObject shopPlusPricePanel;
    public GameObject spetialWeaponPanel;
    public GameObject swButton;

    bool activeShopButtons;
    bool SWPanel;

    public SpetialWeaponBlueprint MissleAttack;
    public SpetialWeaponBlueprint AttackBomb;
    public SpetialWeaponBlueprint BattleShip;
    public SpetialWeaponBlueprint RoadBomb;

    public static SpetialWeaponShop spetialWeaponShop;
    public MisslePoints misslePointsClass;

    string PlayerPrefName;
    SpetialWeaponBlueprint weapon;

    public Sprite openShopButtonSprite;
    public Sprite closeShopButtonSprite;
    public Image ShopButtonSWImage;

    public Text repairDronesCountText;

    private void Start()
    {
       
        //УДАЛИТЬ
        for (int i =0; i < swCount.Length; i++)
        {
            if (swCount[i] == 0)
            {
                swCount[i] = 1;
                swCountText[i].text = swCount[i].ToString();
            }
            
        }

       
        if (spetialWeaponShop != null)
        {
            return;
        }
        else
        {
            spetialWeaponShop = this;
        }

        SWPanel = true;
        activeShopButtons = false;
        shopPlusPanel.SetActive(false);
        shopPlusPricePanel.SetActive(false);
        SWPriceToText();
       // swPriceText[0].text = "555";
    }

    void Update()
    {
        checkDisableShopPlusButtons();
        checkDisableSWButtons();
        TimerHideShopButtons();
        SWCountToText();
        CheckCountRepairDrones();


    }

    void CheckCountRepairDrones()
    {
        GameObject[] repairDrones;
        repairDrones = GameObject.FindGameObjectsWithTag("RepairDrone");
        repairDronesCountText.text = repairDrones.Length.ToString();
        if (repairDrones.Length >= 6 || swCount[5] <= 0)
        {
            spetialWeaponsButtons[5].interactable = false;
        }
        else if (swPrice[5] < PlayerStats.Energy)
        {
            spetialWeaponsButtons[5].interactable = true;
        }
    }

    void SWPriceToText()
    {
        for (int i=0; i < swPriceText.Length; i++)
        {
            swPriceText[i].text = swPrice[i].ToString();
        }
    }

    void checkDisableSWButtons()
    {
        //дизактивация кнопок спецоружия если количество 0
        for (int i = 0; i < spetialWeaponsButtons.Length; i++)
        {
            if (swCount[i] <= 0)
            {
                spetialWeaponsButtons[i].interactable = false;
            }
            
        }
    }


    void checkDisableShopPlusButtons()
    {
        //дизактивация кнопок покупки спецоружия если ресурсов не хватает
        for (int i =0; i< shopPlusBuy.Length; i++)
        {
            if (swPrice[i] > PlayerStats.Energy)
            {
                shopPlusBuy[i].interactable = false;
            }
            else
            {
                shopPlusBuy[i].interactable = true;
            }
       
        }
    }

    void TimerHideShopButtons()
    {
        timerHideShopButtons -= Time.deltaTime;

        if (timerHideShopButtons < 0)
        {
            if(activeShopButtons == false)
            {
                return;
            }
            else
            {
                activeShopButtons = false;
                shopPlusPanel.SetActive(false);
                shopPlusPricePanel.SetActive(false);
                ShopButtonSWImage.sprite = openShopButtonSprite;
            }
        }
    }

    public GameObject MissleLaunch()
    {
        PlayerPrefName = "MissleLaunchSWLevel";
        weapon = MissleAttack;

        return SpetialObjectPrefab();

    }

    public GameObject RoadBombObj()
    {
        PlayerPrefName = "RoadBombUpgrade";
        weapon = RoadBomb;

        return SpetialObjectPrefab();

    }



    GameObject SpetialObjectPrefab()
    {
        int upgradeIndex = PlayerPrefs.GetInt(PlayerPrefName);

        if (upgradeIndex == 0)
        {
            
            return weapon.prefab;

        }
        else if (upgradeIndex == 1)
        {
           
            return weapon.upgradedPrefab;

        }
        else if (upgradeIndex == 2)
        {
            
            return weapon.upgradedPrefab2;

        }
        else if (upgradeIndex == 3)
        {
            
            return weapon.upgradedPrefab3;

        }
        else return null;
    }

    //переключатель кнопок покупки оружия
    public void ActivateShopButtons()
    {
        activeShopButtons = !activeShopButtons;

        if (activeShopButtons == true)
        {
            shopPlusPanel.SetActive(true);
            shopPlusPricePanel.SetActive(true);
            //таймер закрытия кнопок магазина спецоружия
            timerHideShopButtons = 10f;
            ShopButtonSWImage.sprite = closeShopButtonSprite; 
        }
        else
        {
            shopPlusPanel.SetActive(false);
            shopPlusPricePanel.SetActive(false);
            ShopButtonSWImage.sprite = openShopButtonSprite;
        }
    }

    
    
    public void HideOpenSpetialWeaponPanel()
    {
        SWPanel = !SWPanel;
        if (SWPanel == true)
        {
            spetialWeaponPanel.GetComponent<Animator>().SetBool("SWPanel", true);
            swButton.GetComponent<Animator>().SetBool("SWButton", true);
            
        }
        else
        {
            if (activeShopButtons == true)
            {
                activeShopButtons = false;
                shopPlusPanel.SetActive(false);
                shopPlusPricePanel.SetActive(false);
            }

            spetialWeaponPanel.GetComponent<Animator>().SetBool("SWPanel", false);
            swButton.GetComponent<Animator>().SetBool("SWButton", false);
        }
    }

    public void MinusCountSW(int index)
    {
        swCount[index]--;
        swCountText[index].text = swCount[index].ToString();
        
    }

    public void PlusCountSW(int index)
    {
        swCount[index]++;
        swCountText[index].text = swCount[index].ToString();
        spetialWeaponsButtons[index].interactable = true;
    }

    public void MinusSWCoast(int index)
    {
        PlayerStats.Energy -= swPrice[index];
    }

    void SWCountToText()
    {
        for (int i = 0; i < swCountText.Length; i++)
        {
            swCountText[i].text = swCount[i].ToString();
        }
    }

}
