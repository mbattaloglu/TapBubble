using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnim : MonoBehaviour
{
    public FlySlapCoreGame coreGame;

    Vector3 hitterPos;
    Vector3 handlePos;
    private void Awake()
    {
        hitterPos = new Vector3(0, 0, -10);
        handlePos = new Vector3(0, 0, 0);
    }
    public void OnWeaponHit()
    {
        transform.position = new Vector3(transform.parent.position.x, 0, -13);
        coreGame.weaponFinishedHit = true;
        transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        transform.GetChild(0).GetChild(0).GetChild(0).localPosition = hitterPos;
        transform.GetChild(0).GetChild(0).GetChild(1).localPosition = handlePos;
    }

}
