using System;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections;

namespace Grimanda.Common
{
    public class InAppPurchaseModule : MonoBehaviour, IStoreListener
    {
        GameController gameController;


        private static IStoreController m_StoreController;
        private static IExtensionProvider m_StoreExtensionProvider;

        public InAppProduct[] inAppProducts;
        public string AppId;

        public bool isProcessingAPurchase=false;

        public PriceText[] priceTexts;

        // Google Play Store-specific product identifier subscription product.
        private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

        bool isInitialized = false;
        int initTrialCount;

        [SerializeField] GameObject errorScreen;
        [SerializeField] GameObject IAPOverlay;
        [SerializeField] int IAPOverlayDuration;
        [SerializeField] TMPro.TextMeshProUGUI TMPOverleyCountDown;
        int IAPOverlayTime;



        public void ConnectToGameController(GameController gameController)
        {
            this.gameController = gameController;

            initTrialCount = 0;
            isInitialized = false;
            if (m_StoreController == null)
            {
                isProcessingAPurchase = false;
                InitializePurchasing();
            }
        }


        void InitializePurchasing()
        {

            initTrialCount++;
            if (IsInitialized())
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            ConsumableIAPItem consumable = new ConsumableIAPItem();
            for (int i = 0; i < inAppProducts.Length; i++)
            {
                inAppProducts[i].ConnectToGameController(gameController);
                if (inAppProducts[i].GetType() == consumable.GetType())
                {
                    builder.AddProduct("com.grimanda." + AppId + "_" + inAppProducts[i].productId, ProductType.Consumable);
                }
                else
                {
                    builder.AddProduct("com.grimanda." + AppId + "_" + inAppProducts[i].productId, ProductType.NonConsumable);
                }
            }
            UnityPurchasing.Initialize(this, builder);
        }



        private bool IsInitialized()
        {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }


        public void BuyProduct(InAppProduct inAppProduct)
        {
/*#if DEBUG_ENABLED
        if (!PlayerDataUnity.GetBool("BypassPurchaseProcess",true))
        {
            Debug.Log("Bypasing purchase(Debug):" + inAppProduct.productId);
            for (int i = 0; i < inAppProducts.Length; i++)
            {
                Debug.Log(inAppProduct.productId + " ---- " + inAppProducts[i].productId);
                if (String.Equals("com." + AppId + "_" + inAppProduct.productId, "com." + AppId + "_" + inAppProducts[i].productId, StringComparison.Ordinal))
                {
                        inAppProducts[i].OnPlayerPurchasedThis();
                        Debug.Log("Bypasing purchase(Debug):" + inAppProduct.productId);
                }
            }
            return;
        }
#endif*/
            if (isProcessingAPurchase)
            {
                return;
            }
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(inAppProduct.productId);
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log("Buying:" + inAppProduct.productId);
                    Debug.LogError(product);
                    IAPOverlay.SetActive(true);
                    isProcessingAPurchase = true;
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product " + inAppProduct.productId + ", its either is not found or is not available for purchase");
                    errorScreen.SetActive(true);
                    gameController.debugModule.WriteToDebug("BuyProductID: FAIL. Not purchasing product " + inAppProduct.productId + ", its either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("Store Not Initialized");

                gameController.debugModule.WriteToDebug("BuyProductID FAIL. Not initialized.");
            }
        }


        public void RestorePurchases()
        {
#if (UNITY_ANDROID || UNITY_IPHONE)

            if (!IsInitialized())
            {
                gameController.debugModule.WriteToDebug("Restoring Purchases.......Error : Not Initialized");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                apple.RestoreTransactions((result) =>
                {
                // TODO : restore purchases
            });
            }
            else
            {
            }
#endif

        }


        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.LogError("IAP init successfull");

            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;

            for (int i = 0; i < inAppProducts.Length; i++)
            {
                inAppProducts[i].productId="com.grimanda." + AppId + "_" + inAppProducts[i].productId;
                inAppProducts[i].price= m_StoreController.products.WithID(inAppProducts[i].productId).metadata.localizedPriceString;
            }

            for(int i=0;i<priceTexts.Length;i++)
            {
                priceTexts[i].UpdatePriceText();
            }
            isInitialized = true;

        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error);
            gameController.debugModule.WriteToDebug("OnInitializeFailed InitializationFailureReason:" + error);
            gameController.debugModule.WriteToDebug("OnInitializeFailed Trying again:");

            isInitialized = false;
            if (initTrialCount < 5)
            {
                InitializePurchasing();
            }
            else
            {
                gameController.debugModule.WriteToDebug("OnInitializeFailed Tried 5 times");
            }
        }

        IEnumerator StartCoundDownForIAPOverlay()
        {
            IAPOverlayTime = IAPOverlayDuration;
            for (int i = 0; i < IAPOverlayDuration; i++)
            {
                yield return new WaitForSeconds(1);
                IAPOverlayTime--;
                TMPOverleyCountDown.text = IAPOverlayTime.ToString();
            }
            IAPOverlay.SetActive(false);

        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Debug.LogError("Purchase Successfull");
            for (int i = 0; i < inAppProducts.Length; i++)
            {
                if (String.Equals(args.purchasedProduct.definition.id, inAppProducts[i].productId, StringComparison.Ordinal))
                {
                    inAppProducts[i].OnPlayerPurchasedThis();
                    isProcessingAPurchase = false;
                }
            }
            IAPOverlay.SetActive(false);
            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {

            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            Debug.LogError(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
            isProcessingAPurchase = false;
            gameController.debugModule.WriteToDebug(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
            errorScreen.SetActive(true);
            IAPOverlay.SetActive(true);


        }


    }
}
