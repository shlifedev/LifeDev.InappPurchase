using UnityEngine;
using UnityEngine.Purchasing;

namespace LifeDev.InappPurchase
{
    /// <summary>
    /// �� ���ο��� �̸� ĳ���س��� ������ ����������
    /// </summary>
    
    [System.Serializable]
    public class LocalProduct
    { 
        public string ProductName; 
        public string IOSProductId; 
        public string AndroidProudctId;  
        public ProductType ProductType;

        public string PlatformProductId
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                    return AndroidProudctId;
                if (Application.platform == RuntimePlatform.IPhonePlayer ) 
                    return IOSProductId;
                if (Application.isEditor)
                    Debug.Log("IAP ������ ����Ƽ �����Ϳ��� �� �� �����ϴ�.");
                throw new System.Exception("IAPManager�� Platform Product ID�� �����ϴ�.");
            }
        }
    }
}