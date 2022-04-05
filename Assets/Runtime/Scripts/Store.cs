using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace LifeDev.InappPurchase
{
    [System.Serializable]
    public class Store
    {
        public IExtensionProvider StoreExtenstionProvider;
        public IStoreController StoreController;

        /// <summary>
        /// ���⿡ ���� �ִ� ��ǰ���� �־���մϴ�.
        /// </summary>
        public List<LocalProduct> LocalProducts = new List<LocalProduct>();
        public ProductCollection ReceivedProducts; 
    }
}