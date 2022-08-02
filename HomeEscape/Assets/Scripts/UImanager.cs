using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public static UImanager instance {get; private set;}
    public Image health;
    public Image magic;

    void Start() {
        instance = this;
    }

    public void UpdateHealth(int curAmount, int maxAmount)
    {
        health.fillAmount = (float)curAmount / (float)maxAmount;
        
    }

    public void UpdateMagic(int curAmount, int maxAmount)
    {
        magic.fillAmount = (float)curAmount / (float)maxAmount;
        
    }
    
}
