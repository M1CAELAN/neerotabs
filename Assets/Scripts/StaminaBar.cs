using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private static Image staminaBar;
    [SerializeField] Player Stamina;


    private void Start()
    {
        staminaBar = GetComponent<Image>();
    }
    private void Awake()
    {
        Stamina.StaminaChanged += SetStaminaValue;
    }

    public static void SetStaminaValue(float value)
    {
        staminaBar.fillAmount = value;
    }

    private void OnDestroy()
    {
        Stamina.StaminaChanged -= SetStaminaValue;
    }
}
