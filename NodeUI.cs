using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//отображение меню апгрейда и продажи пушки
public class NodeUI : MonoBehaviour
{
    public GameObject ui;

    public GameObject RepearButtonObj;
    public GameObject upgradeButtonObj;
    public GameObject sellButton;
    public GameObject buildButton;
    public GameObject cancelHide;
    public GameObject cancelBuild;

    public Text repairText;
    public Text repairNameText;

    public Text upgradeCost;

    public Text headTurretText;

    public Button upgradeButton;
    public GameObject CancelSelectionButton;

    public Text sellAmount;

    public Node target;

    BuildManager buildManager;

    public static NodeUI nodeUI;

    float buildTime;

    public bool roadBombEnable;

    public Image healthTurretBar;


    private void Awake()
    {
        if (nodeUI != null) return;
        else nodeUI = this;
    }

    private void Start()
    {
        
        CancelSelectionButton.SetActive(false);
        buildManager = BuildManager.instance;
        Hide();


    }

    private void Update()
    {

       
        UpgradeTurretAuto();
        if (target == null)
        {
            return;
        }
        else if(target.turret == null)
        {
            Hide();
        }
        if (!roadBombEnable)
        {
            ShowRepairCost();
        }
        
    }

    void ShowRepairCost()
    {
        if (target == null || target.bombInstaled)
        {
            return;
        }
        
        if (target.turret == null )
        {
            return;
        }
        
        Enemy stat = target.turret.GetComponent<Enemy>();
        
        int damageHealth = (int)(stat.startHealth - stat.health);
        repairText.text = "$" + (damageHealth / 10).ToString();
        healthTurretBar.fillAmount = stat.health / stat.startHealth;

        if (target.turret.GetComponent<Enemy>().repairHealthInProcess)
        {
            repairNameText.text = "STOP";
        }
        else
        {
            repairNameText.text = "REPAIR";
        }
        
    }

    public void UpgradeTurretAuto()
    {
        if (target == null) return;

        if (target.turretBlueprint == null)
        {
            return;
        }


        if (!target.isUpgraded)
        {

            if (PlayerStats.Money < target.turretBlueprint.upgradeCost)
            {


                upgradeButton.interactable = false;

                return;
            }
            upgradeButton.interactable = true;
            return;
        }

        else if (!target.isUpgraded2)
        {

            if (PlayerStats.Money < target.turretBlueprint.upgradeCost2)
            {

                upgradeButton.interactable = false;
                return;
            }
            upgradeButton.interactable = true;
            return;
        }

        else if (!target.isUpgraded3)
        {

            if (PlayerStats.Money < target.turretBlueprint.upgradeCost3)
            {

                upgradeButton.interactable = false;
                return;
            }
            upgradeButton.interactable = true;
            return;
        }
        upgradeButton.interactable = false;


    }

    public void BuildUIButtonsActivate()
    {
        RepearButtonObj.SetActive(false);
        upgradeButtonObj.SetActive(false);
        sellButton.SetActive(false);
        buildButton.SetActive(true);
        cancelBuild.SetActive(true);
        cancelHide.SetActive(false);
    }
    public void UpgradeUIButtonsActivate()
    {
        RepearButtonObj.SetActive(true);
        upgradeButtonObj.SetActive(true);
        sellButton.SetActive(true);
        buildButton.SetActive(false);
        cancelBuild.SetActive(false);
        cancelHide.SetActive(true);
    }

    public void SetTarget(Node _target)
    {

        target = _target;

        transform.position = target.GetBuildPosition();
        if (!target.isUpgraded)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;



        }
        else if (!target.isUpgraded2)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost2;
            upgradeButton.interactable = true;
        }
        else if (!target.isUpgraded3)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost3;
            upgradeButton.interactable = true;
        }

        else
        {
            upgradeCost.text = "DONE";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

        ui.SetActive(true);
    }




    public void PreBuildSetTarget(Node _target)
    {
        target = _target;
        transform.position = target.GetBuildPosition();
    }

    //убирает меню апгрейда
    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Repair()
    {
        target.turret.GetComponent<Enemy>().repairHealthInProcess = !target.turret.GetComponent<Enemy>().repairHealthInProcess;
        target.turret.GetComponent<Enemy>().RepairTurret();
    }

    public void Upgrade()
    {
        if (target.turret.GetComponent<TurretSlow>() != null)
        {
            target.turret.GetComponent<TurretSlow>().UpgradeTurret();
        }
        else
        target.turret.GetComponent<Turret>().UpgradeTurret();
        //target.UpgradeTurret();
        // BuildManager.instance.DeselectNode();
        // Shop.ShopClass.EnableShopButtons();
    }

    public void Sell()
    {


        target.SellTurret();
        BuildManager.instance.DeselectNode();
        target.isUpgraded = false;
        target.isUpgraded2 = false;
        target.isUpgraded3 = false;
        target.ActivateGizmo(false);
        Shop.ShopClass.EnableShopButtons();
    }

    public void CancelHide()
    {
        target.ActivateGizmo(false);
        buildManager.DeselectNode();
        Shop.ShopClass.EnableShopButtons();
        return;
    }

    public void Build()
    {


        Hide();
        Shop.ShopClass.EnableShopButtons();
        if (nodeUI.roadBombEnable)
        {
            target.BuildTurretButton();
        }
        else if (target.turret.GetComponent<TurretSlow>() != null)
        {
            target.turret.GetComponent<TurretSlow>().BuildTurret();
        }
        else
        {
          
            target.turret.GetComponent<Turret>().BuildTurret();
        }
        


            //target.BuildTurretButton();
           // Shop.ShopClass.EnableShopButtons();
       

       
    }


    public void Cancel()
    {
        target.CanselTurretButton();
        buildManager.DeselectNode();
        Shop.ShopClass.EnableShopButtons();
    }

    public void CancelSelection ()
    {
        buildManager.turretToBuild = null;
        Shop.ShopClass.node.ActivateGizmo(false);
        buildManager.DeselectNode();
        Shop.ShopClass.EnableShopButtons();
        Shop.ShopClass.DisableTurretsImages();
        Shop.ShopClass.node.CancelSelectionTurret();
        DisableCanselSelectionButton();
    }

    public void EnableCanselSelectionButton()
    {
        if (CancelSelectionButton.activeSelf) return;

        else
        {
            CancelSelectionButton.SetActive(true);
        }
    }

    public void DisableCanselSelectionButton()
    {
        CancelSelectionButton.SetActive(false);
    }
}
