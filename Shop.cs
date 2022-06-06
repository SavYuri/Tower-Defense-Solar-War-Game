using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;
    public TurretBlueprint airStriker;
    public TurretBlueprint slowImpulse;
    public TurretBlueprint nuclearMissle;
    public TurretBlueprint energyGenerator;


    public TurretBlueprint roadBomb;
    public TurretBlueprint attackBomb;

    public GameObject[] ActivTurretImage;

    public Button[] ShopButtons;

    public Text[] priseOfTurrets;


    public static Shop ShopClass;

    BuildManager buildManager;

    public Node node;

    NodeUI nodeUIClass;

    int[] turretsCost;
   
    public int energyGeneratorAvailableCount;
    public Text energyGeneratorAvailableText;

    private void Awake()
    {
        if (ShopClass != null)
        {
            
            return;
        }
        ShopClass = this;

        
        
    }

    private void Start()
    {
        nodeUIClass = NodeUI.nodeUI;
        turretsCost = new int[7];
        buildManager = BuildManager.instance;
        PriseTurretToText();
        initialTurretsCost();
        energyGeneratorAvailableCount = 3;
    }

    void Update()
    {
        CheckPriceEnableTurretsButtons();
        EnergyGeneratorIcon();
    }

    void EnergyGeneratorIcon()
    {
        energyGeneratorAvailableText.text = energyGeneratorAvailableCount.ToString();
        if (energyGeneratorAvailableCount <= 0)
        {
            energyGeneratorAvailableCount = 0;
            ShopButtons[6].interactable = false;
        }
        else
        {
            if (turretsCost[6] > PlayerStats.Money)
            {
                ShopButtons[6].interactable = false;
            }
            else if (nodeUIClass.ui.activeSelf == false)
            {
                ShopButtons[6].interactable = true;
            }
           
        }
    }

    void initialTurretsCost()
    {
        turretsCost[0] = standardTurret.cost;
        turretsCost[1] = missileLauncher.cost;
        turretsCost[2] = laserBeamer.cost;
        turretsCost[3] = airStriker.cost;
        turretsCost[4] = slowImpulse.cost;
        turretsCost[5] = nuclearMissle.cost;
        turretsCost[6] = energyGenerator.cost;
    }

    void PriseTurretToText()
    {
        priseOfTurrets[0].text = standardTurret.cost.ToString();
        priseOfTurrets[1].text = missileLauncher.cost.ToString();
        priseOfTurrets[2].text = laserBeamer.cost.ToString();
        priseOfTurrets[3].text = airStriker.cost.ToString();
        priseOfTurrets[4].text = slowImpulse.cost.ToString();
        priseOfTurrets[5].text = nuclearMissle.cost.ToString();
        priseOfTurrets[6].text = energyGenerator.cost.ToString();
    }

    //если нет денег на турель, кнопка покупки не активна и наоборот
    void CheckPriceEnableTurretsButtons()
    {

        for(int i = 0; i < turretsCost.Length; i++)
        {
            //кроме кнопки дорожной бомбы
            if (i == 6) continue;
            if (turretsCost[i] > PlayerStats.Money)
            {
                ShopButtons[i].interactable = false;
            }
            else if (nodeUIClass.ui.activeSelf == false )
            {
                ShopButtons[i].interactable = true;
            }
        }
    }


    public void EnableShopButtons()
    {
        for (int i = 0; i<ShopButtons.Length; i++)
        {
            ShopButtons[i].interactable = true;
        }
    }
    public void DisableShopButtons()
    {
        for (int i = 0; i < ShopButtons.Length; i++)
        {
            ShopButtons[i].interactable = false;
        }
    }

    public void SelectStandartTurret()
    {
        buildManager.SelectTurretToBuild(standardTurret);
        //активируеет фон иконки турели
        node.ActivateGizmo(true);
        nodeUIClass.headTurretText.text = "Standart Turret";

    }

    public void SelectMissileLauncher()
    {
        buildManager.SelectTurretToBuild(missileLauncher);
        node.ActivateGizmo(true);
        nodeUIClass.headTurretText.text = "Missile Launcher";
    }

    public void SelectLaserBeamer()
    {
        buildManager.SelectTurretToBuild(laserBeamer);
        node.ActivateGizmo(true);
        nodeUIClass.headTurretText.text = "Laser Turret";
    }

    public void SelectAirStriker()
    {
        buildManager.SelectTurretToBuild(airStriker);
        node.ActivateGizmo(true);
        nodeUIClass.headTurretText.text = "AirStriker Turret";

    }

    public void SelectSlowImpulse()
    {
        buildManager.SelectTurretToBuild(slowImpulse);
        node.ActivateGizmo(true);
        nodeUIClass.headTurretText.text = "Slow Impulse Turret";
    }

    public void SelectNuclearMissle()
    {
        buildManager.SelectTurretToBuild(nuclearMissle);
        node.ActivateGizmo(true);
        nodeUIClass.headTurretText.text = "Nucler Missle Turret";
    }

    public void SelectRoadBomb()
    {
        ShopButtons[6].interactable = false;
        buildManager.SelectTurretToBuild(roadBomb);
        nodeUIClass.headTurretText.text = "BOMB";
        nodeUIClass.roadBombEnable = true;
    }

    public void SelectAttackBomb()
    {
        buildManager.SelectTurretToBuild(attackBomb);
        nodeUIClass.headTurretText.text = "Attack Bombs";
    }

    public void SelectEnergyGenerator()
    {
        buildManager.SelectTurretToBuild(energyGenerator);
        node.ActivateGizmo(true);
        nodeUIClass.headTurretText.text = "Energy Generator";
    }

    //отключение всех фонов турелей
    public void DisableTurretsImages()
    {
        for (int i = 0; i < ActivTurretImage.Length; i++)
        {
            ActivTurretImage[i].SetActive(false);
        }
    }

    //фон иконок турелей
    public void ActivationImageTurret(int i)
    {
        
        DisableTurretsImages();
        ActivTurretImage[i].SetActive(true);
        if(i != 6)
        {
            ShopButtons[6].interactable = true;
        }
        
    }
}
