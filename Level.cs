using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    [SerializeField] float time1 = 60, time2 = 50, time3 = 40, failMalus = 16;

    float t = 0;

    Image bar;

    [HideInInspector] public int totalPacks;
    int packCount;

    [SerializeField] RectTransform Star2, Star3;
    Tutorial tuto;
    TutoTape tutoTape;
    TutoBomb tutoBomb;
    TutoDest tutoDest;

    public void End(bool success)
    {
        if (!success)
            t += failMalus;
        packCount++;

        if (packCount >= totalPacks)
        {
            GameOver();
            return;
        }
        if (tuto != null)
            StartCoroutine(tuto.Part(packCount));
        if (tutoTape != null)
            StartCoroutine(tutoTape.Part(packCount));
        if (tutoBomb != null)
            StartCoroutine(tutoBomb.Part(packCount));
        if (tutoDest != null)
            StartCoroutine(tutoDest.Part(packCount));
    }

    void Start()
    {
        ActivateGos(false);
        bar = GetComponent<Image>();
        packCount = 0;
        Spawner spawner = FindObjectOfType<Spawner>();
        tuto = FindObjectOfType<Tutorial>();
        tutoTape = FindObjectOfType<TutoTape>();
        tutoBomb = FindObjectOfType<TutoBomb>();
        tutoDest = FindObjectOfType<TutoDest>();
        if (tuto == null && tutoTape == null && tutoBomb == null && tutoDest == null)
            totalPacks = spawner.waveCount * spawner.wavePackCount;

        Star2.anchoredPosition = Mathf.Lerp(-835, 25, time2 / time1) * Vector3.right;
        Star3.anchoredPosition = Mathf.Lerp(-835, 25, time3 / time1) * Vector3.right;

        t = 0;
        UpdateBar();
        // StartCoroutine(Play());
    }

    void UpdateBar()
    {
        bar.fillAmount = 1 - Mathf.Clamp(t, 0, time1) / time1;
    }

    bool gameOver = false;

    public IEnumerator Play()
    {
        t = 0;

        while (t < time1 && !gameOver)
        {
            UpdateBar();
            t += Time.deltaTime;
            yield return null;
        }

        // GameOver();
    }

    [SerializeField] GameObject[] activateOnGameOver;
    void ActivateGos(bool value = true)
    {
        foreach (GameObject go in activateOnGameOver)
        {
            go.SetActive(value);
        }
    }

    [SerializeField] Image leftStar, midStar, rightStar;
    [SerializeField]
    string msg0 = "Better luck next time...", msg1 = "Nice, keep improving!", msg2 = "Great job!", msg3 = "Perfection!";

    [SerializeField] TextMeshProUGUI console;

    int Stars => t <= time3 ? 3 : (t <= time2 ? 2 : (t <= time1 ? 1 : 0));
    void GameOver()
    {
        UpdateBar();

        gameOver = true;
        int stars = Stars;

        ActivateGos();

        switch (stars)
        {
            case 0:
                console.text = msg0;
                leftStar.color = midStar.color = rightStar.color = Color.white;
                break;
            case 1:
                console.text = msg1;
                leftStar.color = rightStar.color = Color.white;
                break;
            case 2:
                console.text = msg2;
                rightStar.color = Color.white;
                break;
            default:
                console.text = msg3;
                break;
        }
    }


}