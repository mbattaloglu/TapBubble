using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopAvatar : MonoBehaviour
{
    public Transform avatarSetup;
    [SerializeField] TextMeshProUGUI TMPPrice;
    [SerializeField] Grimanda.Common.Avatar avatar;
    [SerializeField] GameObject buyRoot;
    [SerializeField] GameObject useRoot;

    Grimanda.Common.GameController gameController;

    public void Setup(Grimanda.Common.GameController gameController, Grimanda.Common.Avatar avatar)
    {
        this.avatar = avatar;
        this.gameController = gameController;
        //TMPPrice.text = avatar.price.ToString();
        Transform t = Instantiate(avatar.prefab, avatarSetup).transform;
        t.localPosition = Vector3.zero;
        t.localRotation = avatarSetup.localRotation;
        t.localScale = avatarSetup.localScale;

        if (avatar.GetCurrentAmount() > 0)
        {
            useRoot.SetActive(true);
            buyRoot.SetActive(false);
        }
        else
        {
            useRoot.SetActive(false);
            buyRoot.SetActive(true);
            TMPPrice.text = avatar.neededResources[0].amount.ToString();
        }
    }

    public void UseAvatarOnClick()
    {
        if (avatar.GetCurrentAmount() > 0)
        {
            gameController.playerData.SetActiveAvatar(avatar);
            gameController.avatarSelectScreen.CloseDialog();
            gameController.mainMenuModule.OpenDialog();
        }
        else
        {
            Debug.Log("Should Buy Avatar");
            if (avatar.neededResources[0].amount <= avatar.neededResources[0].resource.GetCurrentAmount())
            {
                avatar.neededResources[0].resource.UpdateResourceAmount(-avatar.neededResources[0].amount);
                avatar.UpdateResourceAmount(1);
                gameController.playerData.SetActiveAvatar(avatar);
                gameController.avatarSelectScreen.CloseDialog();
                gameController.mainMenuModule.OpenDialog();
                Debug.Log("Avatar Bought");
            }
            else
            {
                //todo open shop window
                gameController.buyCoinsPopup.OpenDialog();
            }

        }
    }
}
