using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public Color selectSWButtonColor;
    public Color unSelectSWButtonColor;

    public Button [] SWShopMenu;
    
    public SWLevel[] sWLevel;

    public SWUpgradeCost [] sWUpgradeCost;

    public SWLevelsStatistic[] sWLevelStat;

    public string[] sWName;

    string [] pPrefsSWUpgradeName;

    string [] SWlevelStat;

    public Text SWNameText;

    public Text SWNameSpecificText;

    public Color upgradedColor;

    public Text[] stat;

    public string [] sWspecification;

    public Text UpgradeButtonText;
    public Text UpgradeButtonPriceText;

    public GameObject upgradeButtonObject;
    public GameObject maximumApgradeObject;
    public GameObject diamondScorObj;

    public Text diamondScore;

    public Text upgrdButtnPriceText;
    private void Awake()
    {
        InitialisepPrefsSWLevelUpgName();
        InitialiseSWSpecification();
    }
    private void Start()
    {
      

    }

    private void Update()
    {
        diamondScore.text = PlayerPrefs.GetInt("Diamonds").ToString();
    }

    void InitialisepPrefsSWLevelUpgName()
    {
        pPrefsSWUpgradeName = new string[6];

        pPrefsSWUpgradeName[0] = "BackTimeSWLevel";
        pPrefsSWUpgradeName[1] = "MissleLaunchSWLevel";
        pPrefsSWUpgradeName[2] = "RoadBombSWLevel";
        pPrefsSWUpgradeName[3] = "AttackDronsSWLevel";
        pPrefsSWUpgradeName[4] = "WarShipSWLevel";
        pPrefsSWUpgradeName[5] = "RepairDronesSWLevel";



    }

    void InitialiseSWSpecification()
    {
        sWspecification = new string[6];

        sWspecification[0] = "All enemies moves back for short time.";
        sWspecification[1] = "Several missiles attack random enemies.";
        sWspecification[2] = "Ability to install road bomb.";
        sWspecification[3] = "Several drones comes from finish towards to enemies and ditonate when contact with them.";
        sWspecification[4] = "One warship fly some time around the field and shoot enemies.";
        sWspecification[5] = "Drones fly around the field and repair turrets with damage. The maximum number of drones on the field: 6";
    }

    public void ChooseSW(int index)
    {
        InitialisepPrefsSWLevelUpgName();
        int prefsInt = PlayerPrefs.GetInt(pPrefsSWUpgradeName[index]);

        
        InitialiseSWSpecification();
        ChangeSWButtonColor(index);
        ChangeSWLevelPanel(index);
        ChangeSWUpgradFieldColor(index, prefsInt);
        sWIndex = index;
        SWLevelStat(prefsInt);
        sWLevel[index].SWLevels[prefsInt].GetComponent<ButtonSelected>().EnablePoint();
        ToUpgradeButtonText(index, prefsInt);
        if (prefsInt < 3)
        {
            upgrdButtnPriceText.text = sWUpgradeCost[index].price[prefsInt + 1].ToString();
        }
        
    }

    void ToUpgradeButtonText(int index, int prefsInt)
    {
        if (PlayerPrefs.GetInt(pPrefsSWUpgradeName[index]) == 3)
        {
            upgradeButtonObject.SetActive(false);
            maximumApgradeObject.SetActive(true);
        }
        else
        {
            upgradeButtonObject.SetActive(true);
            maximumApgradeObject.SetActive(false);
            UpgradeButtonText.text = "Upgrade to LEVEL " + (prefsInt + 1).ToString();
        }
       
    }


    void ChangeSWButtonColor(int index)
    {
        for (int i = 0; i < SWShopMenu.Length; i++)
        {
            SWShopMenu[i].GetComponent<Image>().color = unSelectSWButtonColor;
        }
        SWShopMenu[index].GetComponent<Image>().color = selectSWButtonColor;
    }

    //field - вид оружие, cell - уровень апгрейда
    public void ChangeSWUpgradFieldColor(int field, int cell)
    {
        for(int i=0; i < cell+1; i++)
        {
            sWLevel[field].SWLevels[i].GetComponent<Image>().color = upgradedColor;
        }


    }


    public void ChangeSWLevelPanel(int index)
    {
        for (int i = 0; i < sWLevel.Length; i++)
        {
            sWLevel[i].Levels.SetActive(false);
        }

        sWLevel[index].Levels.SetActive(true);
        SWNameText.text = sWName[index];
        SWNameSpecificText.text = sWspecification[index];
    }
    public void UpgradeSWButton()
    {
            int [] levelOfUpgrade = new int [sWLevel.Length];
            string[] prefsName = new string[sWLevel.Length];
            prefsName[0] = "BackTimeSWLevel";
            prefsName[1] = "MissleLaunchSWLevel";
            prefsName[2] = "RoadBombSWLevel";
            prefsName[3] = "AttackDronsSWLevel";
            prefsName[4] = "WarShipSWLevel";
            prefsName[5] = "RepairDronesSWLevel";

        for (int x = 0; x < sWLevel.Length; x++)
            {
                levelOfUpgrade[x] = PlayerPrefs.GetInt(prefsName[x]);
            }

        //i - Sweapon, b - level of upgrade
        for (int i = 0; i < sWLevel.Length; i++)
        {
            if (sWLevel[i].Levels.activeSelf == true)
            {
                for (int b = 0; b < sWLevel.Length; b++)
                {
                    if (levelOfUpgrade[i] == b)
                    {
                        if (levelOfUpgrade[i] == 3)
                        {
                            Debug.Log("Maximum upgrade");
                            
                        }
                        else
                        {
                            int diamonds = PlayerPrefs.GetInt("Diamonds");
                            if (diamonds < sWUpgradeCost[i].price[b + 1])
                            {
                                Animator anim = diamondScorObj.GetComponent<Animator>();
                                if (!anim.enabled) anim.enabled = true;
                                    anim.SetTrigger("In");
                                
                                return;
                            }
                            if (b < 2)
                            {
                                upgrdButtnPriceText.text = sWUpgradeCost[i].price[b + 2].ToString();
                            }
                            
                            diamonds -= sWUpgradeCost[i].price[b + 1];
                            PlayerPrefs.SetInt("Diamonds", diamonds);
                            ChangeSWUpgradFieldColor(i, b+1);
                            PlayerPrefs.SetInt(prefsName[i], b+1);
                        }

                        ToUpgradeButtonText(i, b+1);
                        SWLevelStat(b+1);
                        sWLevel[i].SWLevels[b+1].GetComponent<ButtonSelected>().EnablePoint();
                    }
                }

            }
        } 
    }

    int sWIndex;

    public void InputSWIndex (int index)
    {
        sWIndex = index;
    } 

    public void SWLevelStat (int sWUpgradeLavelIndex)
    {
        //BackTime
        if (sWIndex == 0)
        {
            //Level 1
            if (sWUpgradeLavelIndex == 0)
            {
                stat[0].text = "Target: Land & Air";
                stat[1].text = "Time: 1 sec";
                stat[2].text = "";
                stat[3].text = "";
            }

            //Level 2
            if (sWUpgradeLavelIndex == 1)
            {
                stat[0].text = "Target: Land & Air";
                stat[1].text = "Time: 2 sec";
                stat[2].text = "";
                stat[3].text = "";
            }

            //Level 3
            if (sWUpgradeLavelIndex == 2)
            {
                stat[0].text = "Target: Land & Air";
                stat[1].text = "Time: 3 sec";
                stat[2].text = "";
                stat[3].text = "";
            }

            //Level 4
            if (sWUpgradeLavelIndex == 3)
            {
                stat[0].text = "Target: Land & Air";
                stat[1].text = "Time: 4 sec";
                stat[2].text = "";
                stat[3].text = "";
            }
        }
        //Missile Launcher
        if (sWIndex == 1)
        {
            //Level 1
            if (sWUpgradeLavelIndex == 0)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Missiles: 5";
                stat[2].text = "Damage: 200";
                stat[3].text = "Explode: 12";
            }

            //Level 2
            if (sWUpgradeLavelIndex == 1)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Missiles: 9";
                stat[2].text = "Damage: 250";
                stat[3].text = "Explode: 12";
            }

            //Level 3
            if (sWUpgradeLavelIndex == 2)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Missiles: 13";
                stat[2].text = "Damage: 300";
                stat[3].text = "Explode: 12";
            }

            //Level 4
            if (sWUpgradeLavelIndex == 3)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Missiles: 17";
                stat[2].text = "Damage: 350";
                stat[3].text = "Explode: 12";
            }
        }

        //RoadBomb
        if (sWIndex == 2)
        {
            //Level 1
            if (sWUpgradeLavelIndex == 0)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Bomb: 1";
                stat[2].text = "Damage: 800";
                stat[3].text = "Explode: 12";
            }

            //Level 2
            if (sWUpgradeLavelIndex == 1)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Bomb: 1";
                stat[2].text = "Damage: 1200";
                stat[3].text = "Explode: 13";
            }

            //Level 3
            if (sWUpgradeLavelIndex == 2)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Bomb: 1";
                stat[2].text = "Damage: 1600";
                stat[3].text = "Explode: 14";
            }

            //Level 4
            if (sWUpgradeLavelIndex == 3)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Bomb: 1";
                stat[2].text = "Damage: 2000";
                stat[3].text = "Explode: 15";
            }
        }

        //Attack Drones
        if (sWIndex == 3)
        {
            //Level 1
            if (sWUpgradeLavelIndex == 0)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Drones: 6";
                stat[2].text = "Damage: 200";
                stat[3].text = "Explode: 6";
            }

            //Level 2
            if (sWUpgradeLavelIndex == 1)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Drones: 8";
                stat[2].text = "Damage: 300";
                stat[3].text = "Explode: 7";
            }

            //Level 3
            if (sWUpgradeLavelIndex == 2)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Drones: 10";
                stat[2].text = "Damage: 400";
                stat[3].text = "Explode: 8";
            }

            //Level 4
            if (sWUpgradeLavelIndex == 3)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "Drones: 12";
                stat[2].text = "Damage: 500";
                stat[3].text = "Explode: 9";
            }
        }

        //WarShip
        if (sWIndex == 4)
        {
            //Level 1
            if (sWUpgradeLavelIndex == 0)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "1 WarShip";
                stat[2].text = "Weapon: 1x Land Missiles";
                stat[3].text = "";
            }
            
            //Level 2
            if (sWUpgradeLavelIndex == 1)
            {
                stat[0].text = "Target: Land";
                stat[1].text = "1 WarShip";
                stat[2].text = "Weapon: 1x Land Missiles &  1x Laser";
                stat[3].text = "";
            }

            //Level 3
            if (sWUpgradeLavelIndex == 2)
            {
                stat[0].text = "Target: Land & Air";
                stat[1].text = "1 WarShip";
                stat[2].text = "Weapon: 2x Land Missiles & 2x Air Missiles & 2x Laser";
                stat[3].text = "";
            }

            //Level 4
            if (sWUpgradeLavelIndex == 3)
            {
                stat[0].text = "Target: Land & Air";
                stat[1].text = "1 WarShip";
                stat[2].text = "Weapon: 3x Land Missiles & 3x Air Missiles & 3x Laser";
                stat[3].text = "";
            }
        }

        //RepairDrones
        if (sWIndex == 5)
        {
            //Level 1
            if (sWUpgradeLavelIndex == 0)
            {
                stat[0].text = "Target: Turrets";
                stat[1].text = "2 Repair Drones";
                stat[2].text = "Repair speed: 20/sec";
                stat[3].text = "Total Repair: 800";
            }

            //Level 2
            if (sWUpgradeLavelIndex == 1)
            {
                stat[0].text = "Target: Turrets";
                stat[1].text = "2 Repair Drones";
                stat[2].text = "Repair speed: 30/sec";
                stat[3].text = "Total Repair: 1200";
            }

            //Level 3
            if (sWUpgradeLavelIndex == 2)
            {
                stat[0].text = "Target: Turrets";
                stat[1].text = "2 Repair Drones";
                stat[2].text = "Repair speed: 40/sec";
                stat[3].text = "Total Repair: 1600";
            }

            //Level 4
            if (sWUpgradeLavelIndex == 3)
            {
                stat[0].text = "Target: Turrets";
                stat[1].text = "2 Repair Drones";
                stat[2].text = "Repair speed: 50/sec";
                stat[3].text = "Total Repair: 2000";
            }
        }


    }

    

}

//магазин супероружия
[System.Serializable]
public class SWLevel
{
    //поля уровней field
    public GameObject Levels;
    //ячейка уровня каждого поля cell
    public GameObject[] SWLevels;
}

//Кнопки статистики уровней оружия 
[System.Serializable]
public class SWLevelsStatistic
{
    public Button [] level;

}

//цена апгрейда
[System.Serializable]
public class SWUpgradeCost
{
    public string SuperWeapon;
    public int[] price;

    
}

