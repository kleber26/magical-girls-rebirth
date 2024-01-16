using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public int tutorialViewCount;

    public Image overlayButtonImage;
    public GameObject firtSessionMenu;
    public GameObject[] tutorialSteps;
    int step = 0;

    bool inTransition = false;

    Action OnFinish;

    public void Show(Action onFinish)
    {
        OnFinish = onFinish;
        StartTutorial();
    }

    public void SetFirstSession(Action onFinish)
    {
        OnFinish = onFinish;
        firtSessionMenu.gameObject.SetActive(true);
    }

    public void SkipTutorial()
    {
        firtSessionMenu.gameObject.SetActive(false);
        OnFinish.Invoke();
        tutorialViewCount++;
    }

    public void DisableOverlayButton()
    {
        overlayButtonImage.gameObject.SetActive(false);
        inTransition = false;
    }

    public void StartTutorial()
    {
        firtSessionMenu.gameObject.SetActive(false);
        tutorialViewCount++;
        foreach (var tutorialStep in tutorialSteps)
        {
            tutorialStep.SetActive(false);
        }
        step = 0;
        overlayButtonImage.gameObject.SetActive(true);
        var c = overlayButtonImage.color;
        c.a = 1;
        overlayButtonImage.color = c;
        tutorialSteps[step].SetActive(true);
    }

    public void StepTutorial()
    {
        if (step >= tutorialSteps.Length)
        {
            return;
        }

        tutorialSteps[step].SetActive(false);
        step++;

        if (step < tutorialSteps.Length)
        {
            tutorialSteps[step].SetActive(true);
        }
        else if (!inTransition)
        {
            inTransition = true;
            var c = overlayButtonImage.color;
            c.a = 1.0f / 255.0f;
            overlayButtonImage.color = c;
            Invoke(nameof(DisableOverlayButton), 3.0f);
            OnFinish.Invoke();
        }
    }
}
