using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum Menu { None, MainMenu, Items, Config, Tutorial }

public class MenuController : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject items;
    public GameObject config;
    public Tutorial tutorial;
    public GameObject notEnoughCurrencyPopUp;
    public int tutorialViewCount;
    // public AsyncLoaderSystem asyncLoaderSystem;

    // [SerializeField] ScriptableEvent notEnoughCurrencyEvent;
    [SerializeField] FadeController fadeController;
    [SerializeField] string gameScene;

    // [SerializeField] RewardPopupController rewardPopup;
    // [SerializeField] ChestRewardReceivedEvent chestOpenEvent;

    // void OnEnable()
    // {
    //     chestOpenEvent.OnRaise += rewardPopup.Show;
    // }

    // void OnDisable()
    // {
    //     chestOpenEvent.OnRaise -= rewardPopup.Show;
    // }
    
    public void OnPlayClicked()
    {
        Debug.Log("removed asyncLoaderSystem");
        // asyncLoaderSystem.LoadSceneAsync(gameScene, 2.0f);
    }

    public void FadeIn(bool showMessage = false, bool setMessage = false)
    {
        if (setMessage)
        {
            fadeController.SetRandomMessage();
        }

        fadeController.FadeIn(showMessage);
    }

    public void FadeOut(bool showMessage = false, bool setMessage = false)
    {
        if (setMessage)
        {
            fadeController.SetRandomMessage();
        }

        fadeController.FadeOut(showMessage);
    }

    void Awake() {

        // if (notEnoughCurrencyEvent != null)
        // {
        //     notEnoughCurrencyEvent.OnRaise += ShowNotEnoughCurrencyPopUp;
        // }

        if (tutorial == null || tutorialViewCount == null || tutorialViewCount > 0)
        {
            SetMenu(Menu.MainMenu);
        }
        else
        {
            tutorial.SetFirstSession(() =>
            {
                SetMenu(Menu.MainMenu);
                FadeIn();
            });
        }
    }

    void ShowNotEnoughCurrencyPopUp()
    {
        notEnoughCurrencyPopUp?.SetActive(true);
    }

    public void ShowMainMenu() {
        SetMenu(Menu.MainMenu);
    }

    public void ShowSettingsMenu() {
        SetMenu(Menu.Config);
    }

    public void ShowStoreMenu() {
        SetMenu(Menu.Items);
    }

    public void ShowTutorial() {
        SetMenu(tutorial != null ? Menu.Tutorial : Menu.MainMenu);
        tutorial?.Show(() =>
        {
            SetMenu(Menu.MainMenu);
            FadeIn();
        });
    }

    public void SetMenu(Menu m) {
        mainMenu.SetActive(m == Menu.MainMenu);
        items.SetActive(m == Menu.Items);
        config.SetActive(m == Menu.Config);
    }
}
