using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class FlySlapCoreGame : Grimanda.Common.CoreGame
{
    public GameConfig gameConfig;

    public GameObject targetCircle;
    GameObject initialTargetCircle;
    public GameObject initialSector;
    public GameObject[] allyPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] sectors;

    Vector3 spawnPosition;
    Vector3 mousePosition;
    Vector3 scaleMagnifierVector;
    Vector3 weaponScaleChanger;
    Vector3 weaponPositionChanger;
    
    [Header("Weapon Settings")]
    public GameObject weapon;
    public Animator weaponAnimator;
    public Vector3 weaponStartingScale;
    public Vector3 weaponStartingPosition;
    public Transform cameraAndWeapon;
    public Transform enemiesSpawned;
    public Transform sectorsSpawned;
    public Transform circleCreated;


    [Header("Enemy Spawn Points")]
    public Transform[] spawnPoints;

    float weaponTargetDistance;
    float touchOffset;
    float cameraNormalSpeed;
    float cameraTouchingSpeed;
    float weaponHitDelay;
    float weaponDelayCounter;
    [Header(" ")]
    public float circleScaleMagnifierConstant;
    public float maxCircleScale;
    public float spawnerFrequency;

    bool isCircleCreated;
    bool canWeaponHit;
    bool weaponHit;

    public int score;
    public int health;

    public float gameTime;

    public bool weaponFinishedHit;
    public bool isPlayerFailed;
    public bool isGameStarted;

    public TextMeshProUGUI TMPScoreText;
    public TextMeshProUGUI TMPTimeText;
    private bool useTime;

    private void Start()
    {
        score = 0;
        weaponFinishedHit = true;
        canWeaponHit = true;
        weaponDelayCounter = 0;

        touchOffset = gameConfig.touchOffset;
        circleScaleMagnifierConstant = gameConfig.circleScaleMagnifierConstant;
        maxCircleScale = gameConfig.maxCircleScale;
        spawnerFrequency = gameConfig.spawnerFrequency;
        cameraNormalSpeed = gameConfig.cameraNormalSpeed;
        cameraTouchingSpeed = gameConfig.cameraTouchingSpeed;
        weaponHitDelay = gameConfig.weaponHitDelay;
        gameTime = gameConfig.gameTime;
        useTime = gameConfig.useTimeAndFail;

        weaponStartingScale = weapon.transform.GetChild(0).GetChild(0).localScale;
        weaponStartingPosition = weapon.transform.position;
        weapon.SetActive(true);

        spawnPosition = GetMousePosition();
        scaleMagnifierVector = new Vector3(circleScaleMagnifierConstant, 0, circleScaleMagnifierConstant);
        
        spawnPoints = new Transform[3];
        spawnPoints[0] = initialSector.transform.GetChild(0).transform;
    }

    private void Update()
    {
        //Sector Management
        if (initialSector.transform.GetChild(4).position.x - cameraAndWeapon.position.x  <= 20)
        {
            int spawnIndex = Random.Range(0, sectors.Length);
            initialSector.gameObject.name = "OldSector";
            initialSector = Instantiate(sectors[spawnIndex], initialSector.transform.GetChild(4).position, Quaternion.identity);
            initialSector.gameObject.name = "ActiveSector";
            initialSector.transform.parent = sectorsSpawned;
            spawnPoints[0] = initialSector.transform.GetChild(0).transform;
            StartCoroutine(SpawnEnemies(spawnerFrequency));
        }
    
        if(!isGamePaused && !isPlayerFailed)
        {
            //Score Text
//            TMPScoreText.text = score.ToString();

            //Time Management
            gameTime -= Time.deltaTime;
            //todo sonra acılacak
/*            TMPTimeText.text = ((int)gameTime).ToString();
            if (gameTime <= 3)
            {
                TMPTimeText.color = Color.red;
            }*/
            if (gameTime <=0 && useTime)
            {
                isPlayerFailed = true;
                PlayerFailed();
            }

            //Weapon Management
            if (weaponDelayCounter >= weaponHitDelay)
            {
                canWeaponHit = true;
                weaponHit = false;
                weaponDelayCounter = 0;
            }
            
            if (canWeaponHit)
            {
                //Getting Mouse Input (Touch) For Creating Circle
                if (Input.GetMouseButtonDown(0))
                {
                    weaponFinishedHit = false;
                    spawnPosition = GetMousePosition();
                    //Destroy the last circle if exists
                    if (initialTargetCircle != null)
                        Destroy(initialTargetCircle);
                    //Give an offset for touch
                    spawnPosition.z += touchOffset;
                    //Create a circle and hold it
                    initialTargetCircle = Instantiate(targetCircle, spawnPosition, Quaternion.identity);
                    initialTargetCircle.transform.parent = circleCreated;
                }


                //Check If We Hold The Mouse Button(Touching Continuously) 
                if (Input.GetMouseButton(0) && initialTargetCircle != null)
                {
                    if (initialTargetCircle.transform.localScale.x < maxCircleScale)
                        initialTargetCircle.transform.localScale += scaleMagnifierVector;

                    mousePosition = GetMousePosition();
                    mousePosition.y = 0.5f;
                    mousePosition.z += touchOffset;
                    initialTargetCircle.transform.position = mousePosition;
                }

                if (Input.GetMouseButtonUp(0) && initialTargetCircle != null)
                {
                    canWeaponHit = false;
                    weaponHit = true;
                    isCircleCreated = true;
                }
            }

            if(weaponHit) weaponDelayCounter += Time.deltaTime;

            if (isCircleCreated)
            {
                //Calculating distance between hitting point and weapon and adding to reach target
                weaponTargetDistance = initialTargetCircle.transform.position.z;

                weaponScaleChanger = new Vector3(initialTargetCircle.transform.localScale.x, 1f, initialTargetCircle.transform.localScale.z);
                weaponPositionChanger = new Vector3(initialTargetCircle.transform.position.x, weapon.transform.position.y, weapon.transform.position.z + weaponTargetDistance + 3);

                weapon.transform.GetChild(0).GetChild(0).localScale = weaponScaleChanger;
                //Close the distance between handle and hitter 
                weapon.transform.GetChild(0).GetChild(0).GetChild(1).localPosition -= new Vector3(0, 0, weaponStartingScale.z - weaponScaleChanger.z + 0.5f);
                weapon.transform.position = weaponPositionChanger;
                //Enable shadow while hitting
                weapon.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                weapon.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                weaponAnimator.SetTrigger("Hit");

                //TODO : Solve this bug
                //Checking the circle if it has any enemy on it or not.
                //The reason why we check if it is greater than two is we collide with weapon and game area.
                //Collider[] enemies = Physics.OverlapSphere(initialTargetCircle.transform.position, initialTargetCircle.transform.localScale.x);
                //if (enemies.Length <= 2)
                //{
                //    Debug.Log("failed: boşluk");
                //}

                Destroy(initialTargetCircle);
                isCircleCreated = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isGamePaused && !isPlayerFailed)
        {
            //Camera Rotation
            if (!Input.GetMouseButton(0) && weaponFinishedHit)
            {
                cameraAndWeapon.position += new Vector3(0.01f * cameraNormalSpeed, 0, 0);
            }

            else if(Input.GetMouseButton(0))
            {
                cameraAndWeapon.position += new Vector3(0.01f * cameraTouchingSpeed, 0, 0);
            }
        }
    }

    /// <summary>
    /// Gets the mouse position by sending a Ray.
    /// </summary>
    /// <returns>Vector3 clickPosition</returns>
    Vector3 GetMousePosition()
    {
        Vector3 clickPosition = -Vector3.one;
        //Check if the ray which is created my mouse click(touch) is hit somewhere
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            clickPosition = hit.point;
        }
        return clickPosition;
    }

    public IEnumerator SpawnEnemies(float spawnerFrequency)
    {
        int index = 0;
        int childNumber = spawnPoints[0].childCount;
        while (index < childNumber)
        {
            Vector3 spawnPoint = spawnPoints[0].GetChild(index).GetChild(0).position;
            int randomIndex = Random.Range(1, spawnPoints.Length);
            GameObject enemy = Instantiate(enemyPrefabs[0], spawnPoint, new Quaternion(0,0,0,0));
            enemy.GetComponent<Enemy>().index = index;
            enemy.transform.parent = enemiesSpawned;
            index++;
            yield return new WaitForSeconds(spawnerFrequency);
        }
    }

    public override void StartPlayingGame()
    {
        base.StartPlayingGame();
        score = 0;
        weaponFinishedHit = true;
        canWeaponHit = true;
        weaponDelayCounter = 0;
        isPlayerFailed = false;

        StartCoroutine(SpawnEnemies(spawnerFrequency));

        spawnPoints = new Transform[3];
        spawnPoints[0] = initialSector.transform.GetChild(0).transform;
        isGamePaused = false;
        //todo weapon ayarları yapılacak
    }

}


