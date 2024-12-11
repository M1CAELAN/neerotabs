using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
     private static Image hpBar;
    [SerializeField] Player health;


    private void Start()
    {
        hpBar = GetComponent<Image>();
    }
    private void Awake()
    {
        health.HealthChanged += SetHphBarValue;
    }

    public static void SetHphBarValue(float value)
    {
        hpBar.fillAmount = value;
    }

    private void OnDestroy()
    {
        health.HealthChanged -= SetHphBarValue;
    }

}
