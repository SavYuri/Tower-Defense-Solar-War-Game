
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    //количество живых врагов
    public static int EnemiesAlive = 0;

    public Wave[] waves;
    

    public Transform spawnPoint;
   


    public Transform spawnFlyPoint;
    public Transform spawnPointDropShip;

    public float timeBetweenWaves = 5f;
    private float countdown = 18f;
    public Text waveCountdownText;

    public GameManager gameManager;

    public GameObject SpawnShip;

    private int waveIndex = 0;

    public Text enemyWavesText;

    public Text flyEnemyAlertText;
    public GameObject cameraController;

    public static WaveSpawner waveSpawner;

    GameObject [] newShip;
    

    AudioSource audioS;
    public AudioClip spawnDropShipClip;

    public GameObject flyEnemyWay;

    bool closeFlyEnemyWayEnable;

    public TouchMoveCamera touchMoveCamera;

    public bool removableNodesLevel;
    public NodePoints nodePoints;
    public AudioClip changeNodePosition;

    //public GameObject navigationPanel;



    private void Start()
    {

      

        flyEnemyWay.SetActive(false);

        if (waveSpawner != null) return;
        else waveSpawner = this;

        newShip = new GameObject[waves.Length];

       
        
        EnemiesAlive = 0;

        audioS = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
      

        enemyWavesText.text = PlayerStats.Rounds.ToString() + " / " + waves.Length.ToString(); 

        //если есть враги, код дальше не идёт
        if (EnemiesAlive > 0 || GameManager.GameIsOver == true)
        {
            return;
        }

        //конец волн
        if (waveIndex == waves.Length && GameManager.GameIsOver == false) //waves.Length
        {
           
            gameManager.WinLevel();
            this.enabled = false;
            
        }

        if (countdown <= 0f)
        {
            
            StartCoroutine (SpawnWave());
            //if (navigationPanel.activeSelf) return; else navigationPanel.SetActive(true);
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        //задаёт формат отображения таймера
       // waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    public void ShowFlyEnemyWay()
    {
        if (flyEnemyWay.activeSelf)
        {
            flyEnemyWay.SetActive(false);
            flyEnemyAlertText.gameObject.SetActive(false);
        }
        else
        {
            flyEnemyWay.SetActive(true);
            flyEnemyAlertText.gameObject.SetActive(true);
        }

        if (closeFlyEnemyWayEnable) return;
        else
        {
            closeFlyEnemyWayEnable = true;
            Invoke("CloseFlyEnemyWay", 8);
        }

        
    }

    void CloseFlyEnemyWay()
    {
        flyEnemyWay.SetActive(false);
        flyEnemyAlertText.gameObject.SetActive(false);
        closeFlyEnemyWayEnable = false;
    }


    IEnumerator SpawnShipC()
    {
        newShip[waveIndex] = Instantiate(SpawnShip, spawnPointDropShip.position, spawnPointDropShip.rotation);
        if (!newShip[waveIndex].gameObject.GetComponent<Animator>().enabled) newShip[waveIndex].gameObject.GetComponent<Animator>().enabled = true;
       else
        {
            newShip[waveIndex].gameObject.GetComponent<Animator>().SetTrigger("In");
        }
        
        yield break;
    }

   

    IEnumerator SpawnWave()
    {

        if (removableNodesLevel)
        {
            nodePoints.ToMoveAllNodes();
            audioS.clip = changeNodePosition;
            audioS.Play();
        }

        audioS.clip = spawnDropShipClip;
        audioS.Play();

        CheckFlyEnemyInWaves();
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];
       


        EnemiesAlive ++; //костыль

        StartCoroutine(SpawnShipC());

        yield return new WaitForSeconds(8);

        cameraController.SetActive(true);


        touchMoveCamera.cameraObject.GetComponent<Animator>().enabled = false;


        for (int i = 0; i < wave.count; i++)
        {
            //наземные враги
            SpawnEnemy(wave.enemy);
            //летающие
            SpawnEnemy(wave.flyEnemy);
           
            
            yield return new WaitForSeconds(1f/ wave.rate); // в скобках время между появлением каждого врага
            if (i == wave.count - 1)
            {
                newShip[waveIndex].gameObject.GetComponent<Animator>().SetTrigger("Out");
                Destroy(newShip[waveIndex], 30);
            }
        }

        
        waveIndex++;
        Debug.Log("Волна№: " + waveIndex);
        EnemiesAlive--; //костыль
    }

    

    void SpawnEnemy(GameObject enemy)
    {
        //если нет врага
        if (enemy == null)
        {
            return;
        }
        //если наземный
        if (enemy == waves[waveIndex].enemy)
        {
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        }
        //если летающий
        if (enemy == waves[waveIndex].flyEnemy)
        {
            Instantiate(enemy, spawnFlyPoint.position, spawnFlyPoint.rotation);
        }
    }

  

    void EnableAlertText()
    {
        flyEnemyAlertText.gameObject.SetActive(true);
    }
    void DisnableAlertText()
    {
        flyEnemyAlertText.gameObject.SetActive(false);
    }

    void CheckFlyEnemyInWaves()
    {
        for (int i = PlayerStats.Rounds; i<waves.Length; i++)
        {
            if (waves[i].flyEnemy != null)
            {
                if (i - PlayerStats.Rounds > 0)
                {
                    EnableAlertText();
                    flyEnemyAlertText.text = "FLY ENEMIES COMES IN " + (i - PlayerStats.Rounds).ToString() + " WAVES";
                    Invoke("DisnableAlertText", 7);
                    return;
                }
                else
                {
                    EnableAlertText();
                    flyEnemyAlertText.text = "FLY ENEMIES COMES RIGHT NOW!";
                    Invoke("DisnableAlertText", 7);
                    return;
                }
            }
        }
    }
}
