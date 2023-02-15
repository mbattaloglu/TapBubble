using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    FlySlapCoreGame coreGame;

    private void Awake()
    {
        coreGame = FindObjectOfType<FlySlapCoreGame>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            coreGame.score++;
        }
        else if (collider.gameObject.tag == "Ally")
        {
            coreGame.isPlayerFailed = true;
            coreGame.PlayerFailed();
        }
        else if (collider.gameObject.tag == "Food")
        {
            coreGame.isPlayerFailed = true;
            coreGame.PlayerFailed();
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            GameObject hittedEnemy = collider.transform.parent.parent.parent.gameObject;
            hittedEnemy.GetComponent<Enemy>().StopAllCoroutines();
            collider.gameObject.SetActive(false);
            hittedEnemy.transform.rotation = Quaternion.Euler(90, 0, 45);
            hittedEnemy.transform.DOMove(new Vector3(hittedEnemy.transform.position.x, hittedEnemy.transform.position.y, -20), 1.5f);
            Destroy(hittedEnemy, 1.48f);
            coreGame.gameTime = coreGame.gameConfig.gameTime;
        }
        else if(collider.gameObject.tag == "Ally")
        {
            Destroy(collider.transform.parent.parent.gameObject);
        }
    }
}
