using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelController : MonoBehaviour
{
    public Sprite[] elementSprite;
    [SerializeField] Image[] cellImages;
    [SerializeField] float[] reward;
    [SerializeField] float frameRate = 0.1f;

    Dictionary<string, float> symbolValues;
    List<HashSet<int>> uniqueColumnSymbols = new List<HashSet<int>>();
    float[][] cellValues = new float[4][];
    bool isScalingWinImages = false;

    private void Awake()
    {
        for (int i = 0; i < cellValues.Length; i++)
            cellValues[i] = new float[5];

        symbolValues = new Dictionary<string, float>();
        for (int i = 0; i < elementSprite.Length; i++)
            symbolValues[elementSprite[i].name] = reward[i];
    }

    public IEnumerator SpinReels(float currentAmount)
    {
        isScalingWinImages = false;
        uniqueColumnSymbols.Clear();
        for (int i = 0; i < 5; i++)
            uniqueColumnSymbols.Add(new HashSet<int>());

        for (int i = 0; i < cellImages.Length; i++)
        {
            int row = i / 5;
            int col = i % 5;
            StartCoroutine(PlayAnimation(cellImages[i], row, col));
        }



        // ...rest of your spin logic...

        yield return new WaitForSeconds(4.5f);


        // Now, evaluate and pulse winning cells
        List<int> winIndices;
        float reward = CalculateReward(currentAmount, out winIndices);
        if (winIndices.Count > 0)
        {
            StartCoroutine(PulseWinningCells(winIndices));
        }
    }


    IEnumerator PlayAnimation(Image cellImage, int row, int col)
    {
        int idx = Random.Range(0, 10);
        float timeCompleted = 0f;
        float spinTime = 4f;

        while (timeCompleted <= spinTime)
        {
            idx = Random.Range(0, 10);
            cellImage.sprite = elementSprite[idx];
            yield return new WaitForSeconds(frameRate);
            timeCompleted += frameRate;
        }

        if (col < uniqueColumnSymbols.Count)
        {
            while (uniqueColumnSymbols[col].Contains(idx))
                idx = Random.Range(0, 10);
            uniqueColumnSymbols[col].Add(idx);
        }

        cellValues[row][col] = reward[idx % 10];
        cellImage.sprite = elementSprite[idx];
    }

    public float CalculateReward(float currentAmount, out List<int> winIndices)
    {
        winIndices = new List<int>();
        float totalReward = 0f;

        for (int i = 0; i < 4; i++)
        {
            float val = 0f;
            int longest = 0;

            for (int j = 0; j < 5; j++)
            {
                int k = j + 1;
                while (k < 5 && cellValues[i][k] == cellValues[i][j]) k++;
                int count = k - j;
                if (count >= 3 && count > longest)
                {
                    longest = count;
                    val = cellValues[i][j];
                    for (int col = j; col < k; col++)
                        winIndices.Add((i * 5) + col);
                }
                j = k - 1;
            }

            if (longest >= 3)
            {
                totalReward += val * longest * currentAmount * 0.25f;
            }
        }

        return totalReward;
    }

    IEnumerator PulseWinningCells(List<int> indices)
    {
        Vector3 small = Vector3.one;
        Vector3 big = new Vector3(1.2f, 1.2f, 1f);
        float speed = 3f;
        isScalingWinImages = true;

        while (isScalingWinImages)
        {
            float t = Mathf.PingPong(Time.time * speed, 1f);
            Vector3 targetScale = Vector3.Lerp(small, big, t);

            foreach (int idx in indices)
            {
                if (idx >= 0 && idx < cellImages.Length)
                    cellImages[idx].rectTransform.localScale = targetScale;
            }

            yield return null;
        }

        foreach (int idx in indices)
        {
            if (idx >= 0 && idx < cellImages.Length)
                cellImages[idx].rectTransform.localScale = Vector3.one;
        }
    }
}
 