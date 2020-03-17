using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static IAPManager PurchaseManager;
    public StoreScript StoreReference;
    public string revives_1 = "revives_1";
    public string revives_2 = "revives_2";
    public string revives_3 = "revives_3";
    public string revives_4 = "revives_4";
    public string revives_5 = "revives_5";
    public string revives_6 = "revives_6";
    public string revives_7 = "revives_7";
    public string revives_8 = "revives_8";
    public string revives_9 = "revives_9";
    public string revives_10 = "revives_10";

    // Apple App Store-specific product identifier for the subscription product.
    private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

    // Google Play Store-specific product identifier subscription product.
    private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    void Awake()
    {
        PurchaseManager = this;
    }

    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(revives_1, ProductType.Consumable);
        builder.AddProduct(revives_2, ProductType.Consumable);
        builder.AddProduct(revives_3, ProductType.Consumable);
        builder.AddProduct(revives_4, ProductType.Consumable);
        builder.AddProduct(revives_5, ProductType.Consumable);
        builder.AddProduct(revives_6, ProductType.Consumable);
        builder.AddProduct(revives_7, ProductType.Consumable);
        builder.AddProduct(revives_8, ProductType.Consumable);
        builder.AddProduct(revives_9, ProductType.Consumable);
        builder.AddProduct(revives_10, ProductType.Consumable);


        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        //builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
        // Continue adding the non-consumable product.
        //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here. 
        //builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
        //{ kProductNameAppleSubscription, AppleAppStore.Name },
        //{ kProductNameGooglePlaySubscription, GooglePlay.Name },
        //});

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyRevives(int Count)
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.

        switch (Count)
        {
            case 0:
                break;
            case 1:
                BuyProductID(revives_1);
                break;
            case 2:
                BuyProductID(revives_2);
                break;
            case 3:
                BuyProductID(revives_3);
                break;
            case 4:
                BuyProductID(revives_4);
                break;
            case 5:
                BuyProductID(revives_5);
                break;
            case 6:
                BuyProductID(revives_6);
                break;
            case 7:
                BuyProductID(revives_7);
                break;
            case 8:
                BuyProductID(revives_8);
                break;
            case 9:
                BuyProductID(revives_9);
                break;
            case 10:
                BuyProductID(revives_10);
                break;
        }

    }


    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                //Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                //Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            //Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            //Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            //Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                //Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            //Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        //Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        //Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, revives_1, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(10);
            StoreReference.UpdateRITText();
        }

        else if (String.Equals(args.purchasedProduct.definition.id, revives_2, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(20);
            StoreReference.UpdateRITText();
        }

        else if (String.Equals(args.purchasedProduct.definition.id, revives_3, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(30);
            StoreReference.UpdateRITText();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, revives_4, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(45);
            StoreReference.UpdateRITText();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, revives_5, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(65);
            StoreReference.UpdateRITText();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, revives_6, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(85);
            StoreReference.UpdateRITText();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, revives_7, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(105);
            StoreReference.UpdateRITText();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, revives_8, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(125);
            StoreReference.UpdateRITText();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, revives_9, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(150);
            StoreReference.UpdateRITText();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, revives_10, StringComparison.Ordinal))
        {
            GameController.GameControl.RevivesAdded(180);
            StoreReference.UpdateRITText();
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        //Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}