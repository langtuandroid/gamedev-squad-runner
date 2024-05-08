using UnityEngine;
using UnityEngine.UI;


namespace JetSystems
{
    public class ShopButton : MonoBehaviour
    {
        [Header(" Settings ")]
        public Image containerImage;
        public Image itemImage;
        public Image contour;
        [SerializeField]
        private int price;
        [SerializeField]
        private TypeItem _typeItem;
        private Button thisButton;

        public TypeItem ItemType => _typeItem;

        public int Price => price;


        private void Awake()
        {
            thisButton = GetComponent<Button>();
        }
        
        private void Start()
        {
            if(thisButton == null)
                thisButton = GetComponent<Button>();
        }
        
        public void Lock()
        {
            itemImage.gameObject.SetActive(false);
            thisButton.interactable = false;
        }

        public void Unlock()
        {
            itemImage.gameObject.SetActive(true);
            thisButton.interactable = true;
        }

        public void Configure(Sprite sprite)
        {
            itemImage.sprite = sprite;
        }

        public void SetSelected(bool state)
        {
            if (state)
                contour.gameObject.SetActive(true);
            else
                contour.gameObject.SetActive(false);
        }

        public void SetContainerSprite(Sprite containerSprite)
        {
            containerImage.sprite = containerSprite;
        }
    }

    public enum TypeItem
    {
        ForCoins,
        ForDiamonds,
        ForAds
    }
}