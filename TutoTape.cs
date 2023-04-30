using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoTape : MonoBehaviour
{
    [SerializeField] GameObject general, machine, alreadyPacked;

    PlayerMove pMove;
    PlayerCarry pCarry;
    Spawner1 s1;
    Level level;

    [SerializeField] Machine myMachine;

    void Awake()
    {
        pMove = FindObjectOfType<PlayerMove>();
        pCarry = FindObjectOfType<PlayerCarry>();
        s1 = FindObjectOfType<Spawner1>();
        level = FindObjectOfType<Level>();

        general.SetActive(false);
        machine.SetActive(false);
        alreadyPacked.SetActive(false);

        FindObjectOfType<Level>().totalPacks = 4;
    }

    void Start()
    {
        StartCoroutine(Tuto());
    }

    void Show(GameObject go, bool show = true)
    {
        go.FadeComponents(this, show);
        if (show) go.SetActive(true);
    }

    IEnumerator Tuto()
    {
        Show(general);
        yield return new WaitForSeconds(3);
        Show(general, false);
        StartCoroutine(s1.Spawn1Pack(false));
        Show(machine);
        yield return new WaitUntil(() => myMachine.machining);
        Show(machine, false);
    }

    public IEnumerator Part(int i)
    {
        if (i == 1)
        {
            Show(alreadyPacked, true);
            StartCoroutine(s1.Spawn1Pack(true));
        }
        else if (i == 2)
        {
            Show(alreadyPacked, false);
            StartCoroutine(level.Play());
            StartCoroutine(s1.Spawn1());
            yield return new WaitForSeconds(3);
            StartCoroutine(s1.Spawn1());
        }
        yield return null;
    }
}