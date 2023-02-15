using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
public class Enemy : MonoBehaviour
{
    public EnemyConfig config;
    public GameObject enemy;
    public float flyingTime;
    public float minLandingTime;
    public float maxLandingTime;
    public float localMoveDistance;
    public Vector3 scale;
    public int index;
    public int loopNumber;
    public bool canFly;
    public bool playAnimation;
    private void Awake()
    {
        enemy = config.enemy;
        canFly = config.canFly;
        playAnimation = config.playAnimation;
        flyingTime = config.flyingTime;
        minLandingTime = config.minLandingTime;
        maxLandingTime = config.maxLandingTime;
        localMoveDistance = config.localMoveDistance;
        loopNumber = config.loopNumber;
        transform.localScale = config.enemyScale;
        scale = transform.localScale;
    }

    private void Start()
    {
        if (canFly) StartCoroutine(WalkArround()); 
    }

    public IEnumerator WalkArround()
    {
        Transform spawnPoint = GameObject.Find("ActiveSector").transform.GetChild(0);
        int j = 0;
        while (j < loopNumber)
        {
            if (maxLandingTime == 0 && minLandingTime == 0)
            {
                for (int i = 0; i < spawnPoint.GetChild(index).childCount; i++)
                {
                    if (i == spawnPoint.GetChild(index).childCount - 1) transform.DOLookAt(spawnPoint.GetChild(index).GetChild(0).position, flyingTime / 5);
                    else transform.DOLookAt(spawnPoint.GetChild(index).GetChild(i + 1).position, flyingTime / 10);
                    //yield return new WaitForSeconds(flyingTime / 5);
                    transform.DOMove(spawnPoint.GetChild(index).GetChild(i).position, flyingTime).SetEase(Ease.Linear);
                    yield return new WaitForSeconds(flyingTime);
                }
            }
            else
            {
                for (int i = 0; i < spawnPoint.GetChild(index).childCount; i++)
                {
                    transform.GetChild(0).DOMoveY(transform.localPosition.y + localMoveDistance, flyingTime / 5);
                    yield return new WaitForSeconds(flyingTime / 5);
                    if (i == spawnPoint.GetChild(index).childCount - 1) transform.DOLookAt(spawnPoint.GetChild(index).GetChild(0).position, flyingTime / 5);
                    else transform.DOLookAt(spawnPoint.GetChild(index).GetChild(i + 1).position, flyingTime / 5);
                    yield return new WaitForSeconds(flyingTime / 5);
                    transform.DOMove(spawnPoint.GetChild(index).GetChild(i).position, flyingTime).SetEase(Ease.Linear);
                    yield return new WaitForSeconds(flyingTime);
                    transform.GetChild(0).DOMoveY(transform.localPosition.y - localMoveDistance, flyingTime / 5);
                    yield return new WaitForSeconds(flyingTime / 5);
                    transform.DOLookAt(transform.position, flyingTime / 5);
                    yield return new WaitForSeconds(flyingTime / 5);
                    if (playAnimation) gameObject.GetComponentInChildren<Animator>().enabled = true;
                    yield return new WaitForSeconds(Random.Range(minLandingTime, maxLandingTime));
                    if (playAnimation) gameObject.GetComponentInChildren<Animator>().enabled = false;
                }
            }
            j++;
        }
    }
}
