using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI creditText;
    [SerializeField] TextMeshProUGUI winAmountText;
    [SerializeField] TextMeshProUGUI winLabelText;

    [SerializeField] GameObject winText;
    [SerializeField] GameObject winText_1;
    [SerializeField] GameObject lossText;

    bool isScalingWinText = false;
    bool isScalingLossText = false;

    public void UpdateCreditText(float amount)
    {
        creditText.text = "Credit :- $" + amount.ToString();
    }

    public void UpdateWinAmountText(float amount)
    {
        winAmountText.text = "WinAmount :- $" + amount.ToString();
    }

    public void ResetUI()
    {
        UpdateWinAmountText(0f);
        winText.SetActive(false);
        winText_1.SetActive(false);
        lossText.SetActive(false);
    }

    public void DisplayReward(float reward)
    {
        if (reward > 0f)
        {
            winText.SetActive(true);
            winText_1.SetActive(true);
            winLabelText.text = "$" + reward.ToString();
            StartCoroutine(ScaleWinText());
        }
        else
        {
            Debug.Log("ScaleLossText Coroutine is Called and this is the reward Obtained :- "+reward);
            lossText.SetActive(true);
            StartCoroutine(ScaleLossText());
        }
    }

    IEnumerator ScaleWinText()
    {
        isScalingWinText = true;
        Vector3 big = new Vector3(1.5f, 1.5f, 1f);
        Vector3 small = new Vector3(3f, 3f, 3f);
        float speed = 2f;

        while (isScalingWinText)
        {
            float t = 0f;
            while (t < 1f)
            {
                winText.transform.localScale = Vector3.Lerp(small, big, t);
                winText_1.transform.localScale = Vector3.Lerp(small, big, t);
                t += Time.deltaTime * speed;
                yield return null;
            }
            t = 0f;
            while (t < 1f)
            {
                winText.transform.localScale = Vector3.Lerp(big, small, t);
                winText_1.transform.localScale = Vector3.Lerp(big, small, t);
                t += Time.deltaTime * speed;
                yield return null;
            }
        }
    }

    IEnumerator ScaleLossText()
    {
        isScalingLossText = true;
        Vector3 big = new Vector3(1.5f, 1.5f, 1f);
        Vector3 small = new Vector3(3f, 3f, 3f);
        float speed = 2f;

        while (isScalingLossText)
        {
            float t = 0f;
            while (t < 1f)
            {
                lossText.transform.localScale = Vector3.Lerp(small, big, t);
                t += Time.deltaTime * speed;
                yield return null;
            }
            t = 0f;
            while (t < 1f)
            {
                lossText.transform.localScale = Vector3.Lerp(big, small, t);
                t += Time.deltaTime * speed;
                yield return null;
            }
        }
    }
}
