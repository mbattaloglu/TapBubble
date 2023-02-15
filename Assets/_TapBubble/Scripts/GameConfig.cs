using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameConfig", menuName = "Game Config File")]

public class GameConfig : Grimanda.Common.CommonGameConfig
{
    [Header("Game Specific Settings")]
    public float circleScaleMagnifierConstant;
    public float maxCircleScale;
    public float spawnerFrequency;
    public float cameraNormalSpeed;
    public float cameraTouchingSpeed;
    public float weaponHitDelay;
    public float gameTime;
    public float touchOffset;
    public bool useTimeAndFail;
}
