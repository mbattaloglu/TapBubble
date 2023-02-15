using System;
using UnityEngine;

public class PlayerDataBasic : MonoBehaviour
{
    // Start is called before the first frame update
    void OnPlayerRegisteredStart()
    {
        PlayerPrefs.SetInt("isPlayerRegistered", Convert.ToInt32(true));
    }

    void OnPlayerEarnedCoin(int totalCoinCount)
    {
        PlayerPrefs.SetInt("PlayerCoinCount", totalCoinCount);
    }
}
