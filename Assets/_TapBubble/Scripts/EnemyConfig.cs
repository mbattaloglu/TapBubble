using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "Enemy Config File")]
public class EnemyConfig : ScriptableObject
{
    public bool playAnimation;
    public bool canFly;
    public int loopNumber;
    public float flyingTime;
    public float minLandingTime;
    public float maxLandingTime;
    public float localMoveDistance;
    public Vector3 enemyScale;
    public GameObject enemy;

}
