using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventSystem     : MonoBehaviour
{
    [SerializeField] TMP_Dropdown amountDropDown;

    readonly float[] betAmounts = { 5f, 10f, 50f, 100f, 120f, 150f, 200f, 250f, 300f };

    public float GetSelectedAmount()
    {
        int idx = amountDropDown.value;
        return idx < betAmounts.Length ? betAmounts[idx] : betAmounts[0];
    }

    public void IncreaseAmount()
    {
        if (amountDropDown.value < amountDropDown.options.Count - 1)
            amountDropDown.value++;

        RefreshValue();
    }

    public void DecreaseAmount()
    {
        if (amountDropDown.value > 0)
            amountDropDown.value--;

        RefreshValue();
    }

    void RefreshValue()
    {
        amountDropDown.RefreshShownValue();
    }
}
