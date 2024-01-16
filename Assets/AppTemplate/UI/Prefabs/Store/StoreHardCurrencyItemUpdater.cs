// using System;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.UI;

// public class StoreHardCurrencyItemUpdater : MonoBehaviour
// {
//     [SerializeField] Text titleLabel;
//     [SerializeField] Text descriptionLabel;
//     [SerializeField] Text priceLabel;
//     [SerializeField] Text quantityLabel;
//     [SerializeField] Image iconImage;
//     [SerializeField] Image[] levelIndicators;

//     [SerializeField] Button buyButton;
//     [SerializeField] Image buyButtonBg;
//     [SerializeField] Text buyText;
//     [SerializeField] Text upgradeText;
//     [SerializeField] Text fullText;

//     [SerializeField] Color normalColor;
//     [SerializeField] Color fullColor;


//     [SerializeField] StoreHardCurrencyItem itemData;
//     [SerializeField] bool isUpgradable;

//     // [SerializeField] StoreSystem storeSystem;

//     void OnEnable()
//     {
//         itemData.OnQuantityChanged += UpdateUI;
//         SetupWithData();
//     }

//     void OnDisable()
//     {
//         itemData.OnQuantityChanged -= UpdateUI;
//     }

//     [ContextMenu("Setup With Data")]
//     void SetupWithData()
//     {
//         titleLabel.text = itemData.Title.ToUpper();
//         descriptionLabel.text = itemData.Description;
//         iconImage.sprite = itemData.Icon;

//         levelIndicators[0].transform.parent.gameObject.SetActive(isUpgradable);

//         if (isUpgradable)
//         {
//             for (int i = 0; i < levelIndicators.Length; i++)
//             {
//                 levelIndicators[i].gameObject.SetActive(i < itemData.MaxQuantity);
//             }
//         }

//         quantityLabel.transform.parent.gameObject.SetActive(!isUpgradable);

//         UpdateUI();
//     }


//     void UpdateUI()
//     {
//         priceLabel.text = itemData.Price.ToString("N0");

//         if (isUpgradable)
//         {
//             for (int i = 0; i < levelIndicators.Length; i++)
//             {
//                 levelIndicators[i].color = i < itemData.Quantity ? Color.white : Color.black;
//             }
//         }
//         else
//         {
//             quantityLabel.text = "x" + itemData.Quantity.ToString("N0");
//         }

//         buyButtonBg.color = itemData.IsFull() ? fullColor : normalColor;

//         buyText.gameObject.SetActive(!itemData.IsFull() && !isUpgradable);
//         upgradeText.gameObject.SetActive(!itemData.IsFull() && isUpgradable);

//         buyButton.interactable = !itemData.IsFull();
//         priceLabel.transform.parent.gameObject.SetActive(!itemData.IsFull());
//         fullText.gameObject.SetActive(itemData.IsFull());
//     }

//     public void Buy()
//     {
//         Debug.Log("removed BuyWithHardCurrency");
//         // storeSystem.BuyWithHardCurrency(itemData);
//     }
// }
