using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelectScreen : Grimanda.Common.GrimandaWindow
{
    bool isInitialized = false;
    [SerializeField] Transform avatarsRoot;
    [SerializeField] GameObject avatarShopPrefab;
    public override void DoSpecificThingsAtOpen()
    {
        base.DoSpecificThingsAtOpen();
        if(!isInitialized)
        {
            for(int i=0;i<gameController.activeGameConfig.avatars.Length;i++)
            {
                Transform instantiatedAvatar= Instantiate(avatarShopPrefab, avatarsRoot).transform;
                instantiatedAvatar.SetSiblingIndex(i + 1);
                Debug.Log(gameController.activeGameConfig.avatars[i]);
                instantiatedAvatar.GetComponent<ShopAvatar>().Setup(gameController, gameController.activeGameConfig.avatars[i]);
                instantiatedAvatar.GetComponent<ShopAvatar>().avatarSetup.localScale = gameController.activeGameConfig.avatarScale;
            }
            isInitialized = true;
        }
    }

    public override void DoSpecificThingsAtClose()
    {
        base.DoSpecificThingsAtClose();
        gameController.mainMenuModule.ChangeAvatar();
    }
}
