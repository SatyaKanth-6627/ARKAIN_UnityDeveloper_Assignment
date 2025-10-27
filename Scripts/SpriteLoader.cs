using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpriteLoader : MonoBehaviour
{
    // UIController.cs
    [SerializeField] TextMeshProUGUI creditText;
    [SerializeField] TextMeshProUGUI winAmountText;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject winText_1;
    [SerializeField] GameObject lossText;
    [SerializeField] TextMeshProUGUI winLabelText;
    bool isScalingWinText = false;
    bool isScalingLossText = false;

    // EventSystem.cs
    [SerializeField] TMP_Dropdown amountDropDown;

    // ReelContorller.cs Elements
    public Sprite[] elementSprite;
    [SerializeField] Image[] cellImages;
    [SerializeField] float[][] cellValues = new float[4][];
    [SerializeField] float[] reward;
    [SerializeField] Dictionary<string, float> dct;
    float frameRate = 0.1f;
    bool isPlaying = false;
    List<HashSet<int>> validIndexs = new List<HashSet<int>>();
    bool isScalingWinImages = false;


    //[SerializeField] GameObject image;


  
    // GameContoller.cs
    [SerializeField] float creditAmount;
    [SerializeField] float winAmount;
    [SerializeField] float currentAmount;
    // used to store the winning indices..
    List<int> winIndices = new List<int>();





    private void Start()
    {
        int idx = amountDropDown.value;
        Debug.Log(idx);
        if(idx == 0)
        {
            currentAmount = 5f;
        }

        winAmount = 0;

        creditText.text = "Credit:- $" + creditAmount.ToString();
        winAmountText.text = "WinAmount:- $" + winAmount.ToString();



        for (int i = 0; i < cellValues.Length; i++)
        {
            cellValues[i] = new float[5];
        }
        dct = new Dictionary<string, float>();
        for (int i = 0; i < elementSprite.Length; i++)
        {
            dct.Add(elementSprite[i].name, reward[i]);
            //Debug.Log(elementSprite[i].name + " :- " +dct[elementSprite[i].name]);
        }
    }

   
    public void StartRoll()
    {
        if (creditAmount >= currentAmount)
        {
            isScalingWinText = false;
            isScalingWinImages = false;
            isScalingLossText = false;
            StopCoroutine(PulseWinningCells(winIndices));
            //StartCoroutine(RestTheScaleRoutine(winIndices));
            winIndices.Clear();
            lossText.SetActive(false);
            winText.SetActive(false);
            winText_1.SetActive(false);
            creditAmount -= currentAmount;
            creditText.text = "Credit :- $" + creditAmount.ToString();
            winAmountText.text = "WinAmount :- $0";

            validIndexs.Clear();
            for (int i = 0; i < 5; i++)
            {
                validIndexs.Add(new HashSet<int>());
            }
            for (int i = 0; i < cellImages.Length; i++)
            {
                StartCoroutine(PlayAnimation(cellImages[i], i/5, i%5));
            }

            StartCoroutine(RewardCalulation());
        }
    }

    IEnumerator PlayAnimation(Image cellImage, int row, int col)
    {
        isPlaying = true;
        int idx = Random.Range(0, 10);
        float timeCompleted = 0f;
        float spinTime = 4f; // each next column spins slightly longer


        while (timeCompleted <= spinTime)
        {
            idx = Random.Range(0, 10);
            cellImage.sprite = elementSprite[idx];
            yield return new WaitForSeconds(frameRate);
            timeCompleted += frameRate;
        }

        // Ensure unique symbol in this column
        if (col < validIndexs.Count)
        {
            while (validIndexs[col].Contains(idx))
            {
                idx = Random.Range(0, 10);
            }
            validIndexs[col].Add(idx);
        }

        // Assign final value safely
        cellValues[row][col] = reward[idx % 10];
        cellImage.sprite = elementSprite[idx];
    }

    IEnumerator RewardCalulation()
    {
        float reward = 0f;
        yield return new WaitForSeconds(4.5f);
        for (int i = 0; i < cellImages.Length; i++)
        {

            int row = (i) / 5;
            int col = (i) % 5;
            cellValues[row][col] = dct[cellImages[i].sprite.name];
        }

        for (int i = 0; i < 4; i++)
        {
            float val = 0f;
            int c = 0, j = 0, k = j + 1;
            for (j = 0; j < 5; j++)
            {
                k = j + 1;
                int tempCount = 1;
                while (k < 5)
                {
                    if (cellValues[i][j] == cellValues[i][k])
                    {
                        k++;
                    }
                    else
                    {
                        break;
                    }
                }
                tempCount = (k - j);
                if (tempCount > c)
                {
                    c = tempCount;
                    val = cellValues[i][j];
                }
                j = k - 1;
            }

            if (c >= 3)
            {
                List<int> tempIndices = new List<int>();

                j = 0;
                while (j < 5)
                {
                    k = j + 1;
                    while (k < 5 && cellValues[i][k] == cellValues[i][j])
                        k++;

                    int count = k - j;
                    if (count >= 3)
                    {
                        for (int col = j; col < k; col++)
                        {
                            int index = (i * 5) + col;
                            tempIndices.Add(index);
                        }
                    }

                    j = k;
                }

                if (tempIndices.Count > 0)
                {
                    // Store globally so you can track them later
                    winIndices.AddRange(tempIndices);

                    //  Start pulsating animation for these indices
                    StartCoroutine(PulseWinningCells(tempIndices, 2f));
                }
            }
            else
            {
                c = 0;
            }
            // calculate maximum value uptill now.using built max value.
            reward += val * c * currentAmount *0.25f;

                string str = "";
            for (j = 0; j < 5; j++)
            {
                str = str + cellValues[i][j] + " ";
            }

            Debug.Log(str);
        }
         RewardAdder(reward);

        yield return null;
    }


    void RewardAdder(float reward)
    {
        Debug.Log("Reward Adder Method is Called");
        StartCoroutine(DisplayText(reward));
        creditAmount += reward;
        winAmount = reward;
        creditText.text = "Credit :- $" + creditAmount.ToString();
        winAmountText.text = "WinAmount :- $" + winAmount.ToString();
    }

    IEnumerator DisplayText(float reward)
    {
        if(reward > 0)
        {
            winText.SetActive(true);
            winText_1.SetActive(true);
            //yield return new WaitForSeconds(3f);
            winLabelText.text = "$"+reward.ToString();
            StartCoroutine (ScaleWinText());
            //winText.SetActive(false);
        }
        else
        {
            lossText.SetActive(true);
            StartCoroutine(ScaleLossText());
            //yield return new WaitForSeconds(3f);

        }
        yield return null;
    }

     public void OnDropdownValueChanged()
    {
        int idx = amountDropDown.value;

        if(idx == 0)
        {
            currentAmount = 5f;
        }else if(idx == 1)
        {
            currentAmount = 10f;
        }else if(idx == 2)
        {
            currentAmount = 50f;
        }else if(idx == 3)
        {
            currentAmount = 100f;
        }else if(idx == 4)
        {
            currentAmount = 120f;
        }else if(idx == 5)
        {
            currentAmount = 150f;
        }else if(idx == 6)
        {
            currentAmount = 200f;
        }else if(idx == 7)
        {
            currentAmount = 250f;
        }else if(idx == 8)
        {
            currentAmount = 300f;
        }
    }

    public void IncreaseAmount()
    {
        int optionsCount = amountDropDown.options.Count;
        int currentOption = amountDropDown.value;

        if(currentOption < optionsCount)
        {
            currentOption++;
        }

        amountDropDown.value = currentOption;
        OnDropdownValueChanged();
        amountDropDown.RefreshShownValue();
    }


    public void DecrementAmount()
    {
        int currentOption = amountDropDown.value;

        if (currentOption > 0)
        {
            currentOption--;
        }

        amountDropDown.value = currentOption;
        OnDropdownValueChanged();
        amountDropDown.RefreshShownValue();
    }

    IEnumerator ScaleWinText()
    {
        Debug.Log("ScaleWinText coroutine called");

        isScalingWinText = true;

        Vector3 big = new Vector3(1.5f, 1.5f, 1f);   // target bigger size
        Vector3 small = new Vector3(3f, 3f, 3f);     // normal size
        float speed = 2f;                            // speed of scaling

        while (isScalingWinText)
        {
            // scale up
            float t = 0f;
            while (t < 1f)
            {
                winText.transform.localScale = Vector3.Lerp(small, big, t);
                winText_1.transform.localScale = Vector3.Lerp(small, big, t);
                t += Time.deltaTime * speed;
                yield return null;
            }

            // scale down
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
        Debug.Log("ScaleWinText coroutine called");

        isScalingLossText = true;

        Vector3 big = new Vector3(1.5f, 1.5f, 1f);   // target bigger size
        Vector3 small = new Vector3(3f, 3f, 3f);     // normal size
        float speed = 2f;                            // speed of scaling

        while (isScalingLossText)
        {
            // scale up
            float t = 0f;
            while (t < 1f)
            {
                lossText.transform.localScale = Vector3.Lerp(small, big, t);
                t += Time.deltaTime * speed;
                yield return null;
            }

            // scale down
            t = 0f;
            while (t < 1f)
            {
                lossText.transform.localScale = Vector3.Lerp(big, small, t);
                t += Time.deltaTime * speed;
                yield return null;
            }
        }
    }

    IEnumerator PulseWinningCells(List<int> winningIndices, float duration = 2f)
    {
        Vector3 small = Vector3.one;
        Vector3 big = new Vector3(1.2f, 1.2f, 1f);
        float speed = 3f;

        float elapsed = 0f;
        isScalingWinImages = true;

        while (isScalingWinImages)
        {
            float t = Mathf.PingPong(Time.time * speed, 1f); // smooth oscillation between 0 and 1
            Vector3 targetScale = Vector3.Lerp(small, big, t);

            foreach (int idx in winningIndices)
            {
                if (idx >= 0 && idx < cellImages.Length)
                    cellImages[idx].rectTransform.localScale = targetScale;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // after pulse, reset all winning images to (1,1,1)
        foreach (int idx in winningIndices)
        {
            if (idx >= 0 && idx < cellImages.Length)
                cellImages[idx].rectTransform.localScale = Vector3.one;
        }
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }


}
