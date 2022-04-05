using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;

namespace LifeDev.InappPurchase
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        public static IAPManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<IAPManager>();
                if (_instance == null)
                    return new GameObject().AddComponent<IAPManager>();

                return _instance;
            }
        }
        static IAPManager _instance; 
        public Store Store = new Store(); 
        private IStoreController _storeController; 
        private IExtensionProvider _storeExtenstionProvider;    
        public bool IsInitialized => _storeController != null && _storeExtenstionProvider != null;


        public Store GetStore()
        {
            return Store;
        }
        void Awake()
        {
            if (_instance != null && _instance != this) 
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            Store = new Store();
            Store.LocalProducts.Add(new LocalProduct()
            {
                AndroidProudctId = "consume",
                ProductName = "consume",
                IOSProductId = "consume",
                ProductType = ProductType.Consumable
            });
            Store.LocalProducts.Add(new LocalProduct()
            {
                AndroidProudctId = "nonconsume",
                ProductName = "nonconsume",
                IOSProductId = "nonconsume",
                ProductType = ProductType.NonConsumable
            });
            Store.LocalProducts.Add(new LocalProduct()
            {
                AndroidProudctId = "subscribe",
                ProductName = "subscribe",
                IOSProductId = "subscribe",
                ProductType = ProductType.Subscription
            });

            InitUnityIAP(); 
        }


        public string GetLocalizedPrice(string id)
        {
            return GetProductById(id).metadata.localizedPriceString;
        }

        public string GetLocalizedTitle(string id)
        {
            return GetProductById(id).metadata.localizedTitle;
        }
 
        void InitUnityIAP()
        { 
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance()); 
            foreach (var product in Store.LocalProducts)
            {
                builder.AddProduct(product.ProductName, product.ProductType, new IDs(){
                    { product.IOSProductId, AppleAppStore.Name },
                    { product.AndroidProudctId, GooglePlay.Name },
                }); 
            } 
            UnityPurchasing.Initialize(this, builder);
        }


        /// <summary>
        /// 상점 초기화
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="extensions"></param>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this._storeController = controller;
            this._storeExtenstionProvider = extensions; 
            this.Store.StoreController = controller;
            this.Store.StoreExtenstionProvider = extensions;
            this.Store.ReceivedProducts = controller.products;   
        }
 
        /// <summary>
        /// 초기화 실패 오류처리
        /// </summary>
        /// <param name="error"></param>
        public void OnInitializeFailed(InitializationFailureReason error)
        { 
        } 
        
        /// 구매 실패 오류처리        
        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
        {
     
        } 


        /// <summary>
        /// 구매 프로세스 진행
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {  
            var id = args.purchasedProduct.definition.id; 
            return PurchaseProcessingResult.Complete;
        }


        public Product GetProductById (string id)
        {
            var product = _storeController.products.WithID(id);
            return product;
        }

        /// <summary>
        /// 구매
        /// </summary>
        /// <param name="id"></param>
        public void Purchase(string id)
        {
            if (!IsInitialized) return;

            var product = GetProductById(id);
            if(product != null)
            {
                Debug.Log("���� �õ� - " + product.definition.id);
                _storeController.InitiatePurchase(product);
            }
            else
            {
               
            }
        }
        public bool Purchase(LocalProduct product) => HadPurchase(product.PlatformProductId);
        /// <summary>
        /// 결제복구
        /// </summary>
        public void RestorePurchase()
        {
            if (!IsInitialized) return;
            var platform = Application.platform;


            if (platform == RuntimePlatform.IPhonePlayer ||
                platform == RuntimePlatform.OSXPlayer)
            {
                var extenstion = _storeExtenstionProvider.GetExtension<IAppleExtensions>();
                extenstion.RestoreTransactions((success) => {
                    if (success == false)
                    {
                     
                    }
                    else
                    {
                         
                    }
                });
            }

            if (platform == RuntimePlatform.Android)
            {
                var extenstion = _storeExtenstionProvider.GetExtension<IGooglePlayStoreExtensions>();
                extenstion.RestoreTransactions((success) => {
                    if (success == false)
                    {
                        Debug.Log($"���� ���� �õ� ����");
                    }
                    else
                    {
                        Debug.Log($"���� ���� ����");
                    }
                });
            }
        }


        /// <summary>
        /// ������ ��ǰ���� Ȯ��
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool HadPurchase(string productId)
        {
            if (!IsInitialized) return false;
            var product = _storeController.products.WithID(productId);

            if (product != null)
                return product.hasReceipt;

            return false;
        }

        public bool HadPurchase(LocalProduct product) => HadPurchase(product.PlatformProductId);
    }

}