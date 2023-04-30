using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour
{
    [SerializeField] float initialWait = 3, timeBtwnPack = 1.5f, timeBtwnWaves = 9;
    public int wavePackCount = 3, waveCount = 2;

    Spawner1 s1;

    void Awake()
    {
        s1 = FindObjectOfType<Spawner1>();
    }

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(initialWait);

        StartCoroutine(FindObjectOfType<Level>().Play());

        for (int i = 0; i < waveCount; i++)
        {
            for (int j = 0; j < wavePackCount; j++)
            {
                StartCoroutine(s1.Spawn1());
                yield return new WaitForSeconds(timeBtwnPack);
            }
            yield return new WaitForSeconds(timeBtwnWaves - timeBtwnPack);
        }
    }
}