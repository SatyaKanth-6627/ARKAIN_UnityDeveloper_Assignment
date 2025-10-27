using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] ReelController reelController;
    [SerializeField] EventSystem eventSystemHandler;

    [SerializeField] float creditAmount;
    [SerializeField] float winAmount;
    [SerializeField] float currentAmount;

    List<int> winIndices = new List<int>();

    private void Start()
    {
        uiController.UpdateCreditText(creditAmount);
        uiController.UpdateWinAmountText(0);
        OnDropdownValueChanged();
    }

    public void StartRoll()
    {
        if (creditAmount < currentAmount) return;

        creditAmount -= currentAmount;
        uiController.UpdateCreditText(creditAmount);
        uiController.ResetUI();

        StartCoroutine(HandleReelSpin());
    }

    IEnumerator HandleReelSpin()
    {
        yield return StartCoroutine(reelController.SpinReels(currentAmount));
        float reward = reelController.CalculateReward(currentAmount, out winIndices);

        AddReward(reward);
    }

    void AddReward(float reward)
    {
        winAmount = reward;
        creditAmount += reward;

        uiController.UpdateCreditText(creditAmount);
        uiController.UpdateWinAmountText(winAmount);
        uiController.DisplayReward(reward);
    }

    public void OnDropdownValueChanged()
    {
        currentAmount = eventSystemHandler.GetSelectedAmount();
    }

    public float GetCurrentAmount() => currentAmount;

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
