using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubScreenManager : MonoBehaviour
{
    [SerializeField] GameObject[] subScreens;
    private int currentSubScreenIndex;

    public void InitializeSubScreens()
    {
        foreach (var screen in subScreens) screen.SetActive(false);
    }

    public void ChangeSubScreen(int index)
    {
        currentSubScreenIndex = index;
        subScreens[currentSubScreenIndex].SetActive(true);
    }

    public void PlayScreenAnimation()
    {
        subScreens[currentSubScreenIndex].GetComponent<Animator>()?.SetTrigger("play");
    }
}