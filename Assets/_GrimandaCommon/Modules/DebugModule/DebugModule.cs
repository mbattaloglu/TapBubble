using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Grimanda.Common
{
    public class DebugModule : MonoBehaviour
    {
        GameController gameController;
        GameObject LogPanel;
        [SerializeField] TextMeshProUGUI TMPDebugLog;
        [SerializeField] TMP_InputField TMPScreenWidth;
        [SerializeField] TMP_InputField TMPScreenHeight;
        [SerializeField] TextMeshProUGUI TMPIngameDebugLog;
        [SerializeField] TextMeshProUGUI TMPShowIngameLog;

        public void ConnectToGameController(GameController gameController)
        {
            this.gameController = gameController;
        }
        public void ShowInGameLogButtonOnClick()
        {
            TMPIngameDebugLog.gameObject.SetActive(!TMPIngameDebugLog.gameObject.activeInHierarchy);

            if (TMPIngameDebugLog.gameObject.activeInHierarchy)
            {
                TMPShowIngameLog.text = "Hide Ingame Log";
            }
            else
            {
                TMPShowIngameLog.text = "Show Ingame Log";
            }
        }

        private void OnEnable()
        {
            TMPScreenWidth.text = Screen.width.ToString();
            TMPScreenHeight.text = Screen.height.ToString();

            if (TMPIngameDebugLog.gameObject.activeInHierarchy)
            {
                TMPShowIngameLog.text = "Hide Ingame Log";
            }
            else
            {
                TMPShowIngameLog.text = "Show Ingame Log";
            }
        }

        public void ResetPlayerData()
        {
            PlayerPrefs.DeleteAll();
            for (int i= 0;i<gameController.playerData.boolValues.Length;i++)
            {
                gameController.playerData.boolValues[i].value = gameController.playerData.boolValues[i].defaultValue;
            }
            for (int i = 0; i < gameController.playerData.intValues.Length; i++)
            {
                gameController.playerData.intValues[i].value = gameController.playerData.intValues[i].defaultValue;
            }
            for (int i = 0; i < gameController.playerData.stringValues.Length; i++)
            {
                gameController.playerData.stringValues[i].value = gameController.playerData.stringValues[i].defaultValue;
            }

            for(int i=0;i<gameController.activeGameConfig.avatars.Length;i++)
            {
                gameController.activeGameConfig.avatars[i].SetCurrentAmount(0);
            }
            gameController.activeGameConfig.avatars[0].SetCurrentAmount(1);
            gameController.playerData.SetActiveAvatar(gameController.activeGameConfig.avatars[0]);

        }

        public void SetScreenResolution()
        {
            int width=800;
            int heigth=1280;
            Debug.Log(TMPScreenWidth.text + "x" + TMPScreenHeight.text);

            try
            {
                int.TryParse(TMPScreenWidth.text, out width);
                    
            }
            catch
            {
                width = 800;
            }

            try
            {
                int.TryParse(TMPScreenHeight.text, out heigth);

            }
            catch
            {
                heigth = 1280;
            }

            Debug.Log(width + "x" + heigth);
            Screen.SetResolution(width, heigth,false);
        }

        public void WriteToDebug(string DebugText, bool Error = false, bool WriteToConsole = false)
        {
            if (WriteToConsole)
            {
                if (Error)
                {
                    Debug.LogError(DebugText);
                }
                else
                {
                    Debug.Log(DebugText);
                }
            }
            TMPDebugLog.text += "\n" + DebugText;
        }

        public void WriteToDebug(string DebugText,int line)
        {
            TMPIngameDebugLog.text += "\n" + line + ". "+DebugText;
        }


        public void ByPassRewardedAdOnClick()
        {
        }

        public void ByPassPurchaseProcessOnClick()
        {
        }


        public void ShowLogOnClick()
        {
            LogPanel.SetActive(true);
        }

        public void init()
        {
#if DEBUG_ENABLED
        gameObject.SetActive(true);
#endif
        }
    }
}
