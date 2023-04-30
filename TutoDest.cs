using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoDest : MonoBehaviour
{
    [SerializeField] GameObject general, machine, goesHere;

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
        goesHere.SetActive(false);

        FindObjectOfType<Level>().totalPacks = 3;
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
        StartCoroutine(s1.Spawn1Destination(2));
        Show(machine);
        yield return new WaitUntil(() => myMachine.machining);
        Show(machine, false);
        yield return new WaitUntil(() => !myMachine.machining);
        Show(goesHere);
    }

    public IEnumerator Part(int i)
    {
        if (i == 1)
        {
            Show(goesHere, false);
            StartCoroutine(level.Play());
            StartCoroutine(s1.Spawn1());
            yield return new WaitForSeconds(3);
            StartCoroutine(s1.Spawn1());
        }
        yield return null;
    }
}