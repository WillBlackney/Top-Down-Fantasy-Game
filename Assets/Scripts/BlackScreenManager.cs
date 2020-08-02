﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenManager : Singleton<BlackScreenManager>
{
    [Header("Component References")]
    public GameObject visualParent;
    public CanvasGroup canvasGroup;
    public Canvas canvas;

    [Header("Properties")]
    public int currentSortingLayer;
    public int aboveEverythingExceptUI;
    public int aboveEverything;
    public int behindEverything;
    public bool fadingOut;
    public bool fadingIn; 

    // Visibility + View Logic
    #region
    public void SetSortingLayer(int newLayer)
    {
        canvas.sortingOrder = newLayer;
        currentSortingLayer = newLayer;
    }
    public void SetActive(bool onOrOff)
    {
        if (onOrOff == true)
        {
            visualParent.SetActive(true);
        }
        else
        {
            visualParent.SetActive(false);
        }
    }
    #endregion

    // Fade In / Out Logic
    #region
    public Action FadeIn(int sortingLayer, int speed = 2, float alphaTarget = 0, bool setActiveOnComplete = true)
    {
        Action action = new Action();
        StartCoroutine(FadeInCoroutine(sortingLayer, speed, alphaTarget, setActiveOnComplete, action));
        return action;
    }
    private IEnumerator FadeInCoroutine(int sortingLayer, int speed, float alphaTarget, bool setActiveOnComplete, Action action)
    {
        fadingIn = false;
        fadingOut = false;
        fadingIn = true;

        SetActive(true);
        SetSortingLayer(sortingLayer);

        Debug.Log("FadeInCoroutine() started...");

        while (canvasGroup.alpha > alphaTarget && fadingIn)
        {
            canvasGroup.alpha -= 0.25f * speed * Time.deltaTime;
            if (canvasGroup.alpha == alphaTarget)
            {
                SetSortingLayer(behindEverything);
                SetActive(setActiveOnComplete);
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("FadeInCoroutine() finished...");
        action.MarkAsComplete();

    }
    public Action FadeOut(int sortingLayer, int speed = 2, float alphaTarget = 1, bool setActiveOnComplete = false)
    {
        Action action = new Action();
        StartCoroutine(FadeOutCoroutine(sortingLayer, speed, alphaTarget, setActiveOnComplete, action));
        return action;
    }
    private IEnumerator FadeOutCoroutine(int sortingLayer, int speed, float alphaTarget, bool setActiveOnComplete, Action action)
    {
        fadingIn = false;
        fadingOut = false;
        fadingOut = true;

        SetActive(true);
        SetSortingLayer(sortingLayer);

        while (canvasGroup.alpha < alphaTarget && fadingOut == true)
        {
            canvasGroup.alpha += 0.25f * speed * Time.deltaTime;
            if (canvasGroup.alpha == alphaTarget)
            {
                SetActive(setActiveOnComplete);
            }
            yield return new WaitForEndOfFrame();
        }
        action.MarkAsComplete();
    }
    public void SetTotalBlackOutState()
    {
        visualParent.SetActive(true);
        canvasGroup.alpha = 1;
        SetSortingLayer(aboveEverything);
    }
    #endregion
}
