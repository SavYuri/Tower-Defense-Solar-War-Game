using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Node : MonoBehaviour
{
    //цвет квадратика при наведении на него курсором
    public Color hoverColor;
    //цвет квадратика при недостатке денег при строительстве
    public Color notEnoughMoneyColor;
    //позиция пушки при строительстве
    public Vector3 positionOffset;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded;
    public bool isUpgraded2;
    public bool isUpgraded3;

    private Renderer rend;
    private Color startColor;
   public BuildMarker buildMarker;
    ConfirmBuildUI confirmBuildUI;
    BuildManager buildManager;
    Shop shop;
    public  Turret turretClass;
    public static Node nodeClass;

    public bool roadNode;
    public bool roadBombEnable;
    public bool turretInstaled;
    public bool bombInstaled;
    public AudioClip destroyTurret;
    public AudioSource audioSource;
    public GameObject cameraObj;

    

    // public TurretStatistic turretStatistic;
    NodeUI nodeUIClass;
    public BombSpawner bombSpawnerClass;

    private void Awake()
    {
        if (nodeClass != null)
        {

            return;
        }
        nodeClass = this;

       
    }

   


    private void Start()
    {

        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");


        bombInstaled = false;
        turretInstaled = false;
        bombSpawnerClass = BombSpawner.bombSpawner;
        nodeUIClass = NodeUI.nodeUI;

    isUpgraded = false;
    isUpgraded2 = false;
    isUpgraded3 = false;

       // turretStatistic = TurretStatistic.turretStatisticClass;

        turretClass = Turret.turretClass;
        confirmBuildUI = ConfirmBuildUI.confirmBuildUIClass;
        rend = GetComponent<Renderer>();
       
        buildManager = BuildManager.instance;
        shop = Shop.ShopClass;
        //buildManager.turretToBuild = null;
        //обычный цвет квадратика
        if(rend != null)
        {
            startColor = rend.material.color;
        }
        
        //PlayerPrefs.SetInt("RoadBombUpgrade", 2);
        //roadBombEnable = true;


    }

    private void Update()
    {
        
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    Vector3 pushDownPosition;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        pushDownPosition = cameraObj.transform.position;
        Debug.Log("Node MouseDown");
    }
    //нажатие клик
    private void OnMouseUp()
    {
        Debug.Log("Node MouseUP1");
        if (Vector3.Distance(cameraObj.transform.position, pushDownPosition) >= 3f)
        {
            return;
        }
        Debug.Log("Node MouseUP2");
        // if (cameraObj.transform.position != pushDownPosition) return;
        //создание некликабельной области у иконок башен
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        Debug.Log("Node MouseUP3");
        if (buildManager.GetTurretToBuild() == shop.roadBomb && roadNode == false)
        {
           return;
        }
        Debug.Log("Node MouseUP4");

        if (buildManager.GetTurretToBuild() == shop.roadBomb && roadNode == true)
        {
            Debug.Log("Road Bomb Selected");
            nodeUIClass.roadBombEnable = true;
            SetRoadBombStat();


        }
        Debug.Log("Node MouseUP5");
        if (turret != null)
        {
            if(turret.GetComponent<TurretSlow>() != null)
            {
                if (turret.GetComponent<TurretSlow>().buildInProcess == true)
                {
                    return;
                }
            }
            else
            if (turret.GetComponent<Turret>().buildInProcess == true)
                return;
        }
        Debug.Log("Node MouseUP6");
        if (nodeUIClass.ui.activeSelf) return;
        Debug.Log("Node MouseUP7");
        if (turret != null && roadNode == false)
        {
           
            shop.DisableTurretsImages();
            buildManager.SelectNode(this);
            ActivateGizmo(true);
            shop.DisableShopButtons();
            //присваиваем статистику пушки
            if (turret.GetComponent<TurretSlow>() != null)
            {
                turret.GetComponent<TurretSlow>().SetTurretStatistic();
            }
            else
            {
                turret.GetComponent<Turret>().SetTurretStatistic();
            }
            nodeUIClass.UpgradeUIButtonsActivate();
            nodeUIClass.DisableCanselSelectionButton();
            return;
        }
        Debug.Log("Node MouseUP8");
        if (!buildManager.CanBuild)
            return;
        Debug.Log("Node MouseUP9");
        if (roadNode == true && buildManager.GetTurretToBuild() != shop.roadBomb)
            return;

        Debug.Log("Node MouseUP10");
        nodeUIClass.DisableCanselSelectionButton();
        shop.DisableShopButtons();
       
       
        PreBuildTurret(buildManager.GetTurretToBuild());
        //после строительства турели убирает возможность строить дальше
        buildManager.turretToBuild = null;
        
        buildManager.PreBuildNode(this);


    }

    public void SelectAttackBomb()
    {
        shop.DisableShopButtons();
        nodeUIClass.SetTarget(this);
    }

    GameObject gizmoObject;
    public void PreBuildTurret(TurretBlueprint blueprint)
    {
        nodeUIClass.ui.SetActive(true);
        GameObject _turret = (GameObject)Instantiate(blueprint.samplePrefab, GetBuildPosition(), Quaternion.identity);
        _turret.transform.parent = gameObject.transform;
        turret = _turret;
        if (!nodeUIClass.roadBombEnable && turret.GetComponent<TurretSlow>() == null)
        {
            if (turret.GetComponent<EnergyTurret>() != null)
            {
                turret.GetComponent<EnergyTurret>().nodeToBuild = this;
            }
            else

            turret.GetComponent<Turret>().nodeToBuild = this;
        }
        
        if (turret.GetComponent<TurretSlow>() != null)
        {
            turret.GetComponent<TurretSlow>().SetTurretStatistic();
            turret.GetComponent<TurretSlow>().nodeToBuild = this;
        }
        else if (turret.GetComponent<Turret>() != null)
        {
            turret.GetComponent<Turret>().SetTurretStatistic();
        }
        turretBlueprint = blueprint;
        buildManager.turretToBuild = null;
        nodeUIClass.BuildUIButtonsActivate();
        //confirmBuildUI.confirmBuildPanelUI.SetActive(true);
        
        
       
        ActivateGizmo(true);
        
        Debug.Log("Turret PreBuild!");
        
    }

 void SetRoadBombStat()
    {

        int bombLevel = PlayerPrefs.GetInt("RoadBombSWLevel");
        turretBlueprint = buildManager.GetTurretToBuild();
        GameObject[] prefab = new GameObject[4];
        prefab[0] = turretBlueprint.prefab;
        prefab[1] = turretBlueprint.upgradedPrefab;
        prefab[2] = turretBlueprint.upgradedPrefab2;
        prefab[3] = turretBlueprint.upgradedPrefab3;

        prefab[bombLevel].GetComponent<DetonationBomb>().SetRoadBombStatistic(bombLevel);
    }

    //При нажатии в меню строить турель
    public void BuildTurretButton()
    {

        turretInstaled = true;
        GameObject _turret;
        Destroy(turret);
        
        if (nodeUIClass.roadBombEnable == false)
        {
            _turret = (GameObject)Instantiate(turretBlueprint.prefab, GetBuildPosition(), Quaternion.identity);
            _turret.transform.parent = gameObject.transform;
        }
        
        else
        {

            if (PlayerPrefs.GetInt("RoadBombSWLevel") == 0)
            {
                _turret = (GameObject)Instantiate(turretBlueprint.prefab, GetBuildPosition(), Quaternion.identity);

            }
            else if (PlayerPrefs.GetInt("RoadBombSWLevel") == 1)
            {
                _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
            }
            else if (PlayerPrefs.GetInt("RoadBombSWLevel") == 2)
            {
                Debug.Log("RoadBomb 3");
                _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab2, GetBuildPosition(), Quaternion.identity);
            }
            else if (PlayerPrefs.GetInt("RoadBombSWLevel") == 3)
            {
                _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab3, GetBuildPosition(), Quaternion.identity);
            }
            else
            {
                _turret = null;
            }

            nodeUIClass.roadBombEnable = false;
            Debug.Log("BombInstaled!");
            bombInstaled = true;
            _turret.GetComponent<DetonationBomb>().installedNode = this;
            
        }


        
        buildMarker = BuildMarker.BM;
        buildMarker.BuildRoadNodeLightOff();
        turret = _turret;
        if (!bombInstaled)
        {
            if (turret.GetComponent<TurretSlow>() != null)
            {
                turret.GetComponent<TurretSlow>().nodeToBuild = this;
            }
            else
            turret.GetComponent<Turret>().nodeToBuild = this;
        }
        rend.material.color = startColor;
        //confirmBuildUI.confirmBuildPanelUI.SetActive(false);
        

        //создание эффекта строительства
        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
       // PlayerStats.Money -= turretBlueprint.cost;
        buildManager.selectedNode = null;
        if (buildManager.CanBuild || nodeUIClass.ui.activeSelf) return;
        ActivateGizmo(false);
        shop.DisableTurretsImages();
        /*
        if (turret != null)
        {
            buildManager.SelectNode(this);

            return;
        }
        */
    }

    

    

    public void ReturnOneRoadBomb()
    {
        if (nodeUIClass.roadBombEnable == true)
        {
            SpetialWeaponShop.spetialWeaponShop.swCount[2]++;
            nodeUIClass.roadBombEnable = false;
        }
    }

    //При нажатии в меню отменить строительство
    public void CanselTurretButton()
    {
        ReturnOneRoadBomb();

        bombInstaled = false;
        buildMarker = BuildMarker.BM;
        buildMarker.BuildRoadNodeLightOff();
        Destroy(turret);
        rend.material.color = startColor;
//        confirmBuildUI.confirmBuildPanelUI.SetActive(false);
        shop.DisableTurretsImages();
        ActivateGizmo(false);
    }

    

    public void CancelSelectionTurret()
    {
        ReturnOneRoadBomb();

        bombInstaled = false;
        buildMarker = BuildMarker.BM;
        buildMarker.BuildRoadNodeLightOff();
    }

    //активация гизмо при стоительстве
    public void ActivateGizmo(bool activation)
    {
        GameObject[] Turrets;
        Turrets = GameObject.FindGameObjectsWithTag("Turret");

        for (int i = 0; i < Turrets.Length; i++)
        {
            //ВАЖНО: при изменении положения Gizmo в объекте турели индекс чайлд надо менять
            GameObject child = Turrets[i].transform.GetChild(0).gameObject;
            child.SetActive(activation);
           
        }



    }

    void BuildTurret(TurretBlueprint blueprint)
    {

        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }
       

        PlayerStats.Money -= blueprint.cost;

        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret; 

        turretBlueprint = blueprint;

        //создание эффекта строительства
        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
        
        rend.material.color = startColor;
        Debug.Log("Turret build!");
    }

  
        public void UpgradeTurret()
    {

        if (!isUpgraded)
            {

           

            //PlayerStats.Money -= turretBlueprint.upgradeCost;
            //уничтожает старую турель
            Destroy(turret);

            //строит апгрейд
            GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
            _turret.transform.parent = gameObject.transform;
            turret = _turret;
            if (turret.GetComponent<TurretSlow>() != null)
            {
                turret.GetComponent<TurretSlow>().nodeToBuild = this;
            }
            else
            {
                turret.GetComponent<Turret>().nodeToBuild = this;
            }
            
            turretClass = turret.GetComponent<Turret>();
           
    

            //создание эффекта строительства
            GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
            Destroy(effect, 5f);

            isUpgraded = true;
            Debug.Log("Turret upgraded!");
            if (buildManager.CanBuild || nodeUIClass.ui.activeSelf) return;
            ActivateGizmo(false);
            
            
            return;
        }
        if (!isUpgraded2)
        {

           

            //PlayerStats.Money -= turretBlueprint.upgradeCost2;
            //уничтожает старую турель
            Destroy(turret);

            //строит апгрейд
            GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab2, GetBuildPosition(), Quaternion.identity);
            _turret.transform.parent = gameObject.transform;
            turret = _turret;
            if (turret.GetComponent<TurretSlow>() != null)
            {
                turret.GetComponent<TurretSlow>().nodeToBuild = this;
            }
            else
            {
                turret.GetComponent<Turret>().nodeToBuild = this;
            }
            //создание эффекта строительства
            GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
            Destroy(effect, 5f);

            isUpgraded2 = true;

            

            ActivateGizmo(false);
            Debug.Log("Turret upgraded2!");
            return;
        }
        if (!isUpgraded3)
        {

           

            //PlayerStats.Money -= turretBlueprint.upgradeCost3;
            //уничтожает старую турель
            Destroy(turret);

            //строит апгрейд
            GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab3, GetBuildPosition(), Quaternion.identity);
            _turret.transform.parent = gameObject.transform;
            turret = _turret;
            if (turret.GetComponent<TurretSlow>() != null)
            {
                turret.GetComponent<TurretSlow>().nodeToBuild = this;
            }
            else
            {
                turret.GetComponent<Turret>().nodeToBuild = this;
            }
            //создание эффекта строительства
            GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
            Destroy(effect, 5f);

            isUpgraded3 = true;
            
            ActivateGizmo(false);
            Debug.Log("Turret upgraded3!");
            return;
        }

    }


    



    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void SellTurret()
    {
        if (turret.GetComponent<Enemy>().thisIsEnergyGenerator)
        {
            shop.energyGeneratorAvailableCount++;
        }

        turretInstaled = false;
        PlayerStats.Money += turretBlueprint.GetSellAmount();
        PlaySound(destroyTurret);
        //создание эффекта
        GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;
    }


    
    
    //наведение курсора
    private void OnMouseEnter()
    {
        //создание некликабельной области у иконок башен
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        //если есть деньги
        if (buildManager.HasMoney)
        {
            if (roadNode == true && buildManager.GetTurretToBuild() != shop.roadBomb)
                return;

            rend.material.color = hoverColor;

           
            

        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
    }

    //увод курсора
    private void OnMouseExit()
    {
        if (!buildManager.CanBuild)
            return;
        rend.material.color = startColor;

        

    }
    /*
    //отображение турели при наведении
    void ShowTurretOnMauseEnter(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }

        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        turretBlueprint = blueprint;
    }

    //отмена отображение турели при наведении
    public void DestroyTurretOnMauseExit()
    {
        Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;
    }
    */
}
