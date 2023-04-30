using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoBomb : MonoBehaviour
{
    [SerializeField] GameObject general, machine, dangerous, otherOne, safe, unpacked, ua1, ua2;

    PlayerMove pMove;
    PlayerCarry pCarry;
    Spawner1 s1;
    Level level;

    [SerializeField] Machine myMachine, tapeMachine;

    void Awake()
    {
        pMove = FindObjectOfType<PlayerMove>();
        pCarry = FindObjectOfType<PlayerCarry>();
        s1 = FindObjectOfType<Spawner1>();
        level = FindObjectOfType<Level>();

        general.SetActive(false);
        machine.SetActive(false);
        dangerous.SetActive(false);
        otherOne.SetActive(false);
        safe.SetActive(false);
        unpacked.SetActive(false);
        ua1.SetActive(false);
        ua2.SetActive(false);

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
        StartCoroutine(s1.Spawn1Bomb(true));
        Show(machine);
        yield return new WaitUntil(() => myMachine.machining);
        Show(machine, false);
        yield return new WaitUntil(() => !myMachine.machining);
        Show(dangerous);
    }

    public IEnumerator Part(int i)
    {
        if (i == 1)
        {
            Show(dangerous, false);
            Show(otherOne);
            StartCoroutine(s1.Spawn1Bomb(false));
            yield return new WaitUntil(() => myMachine.machining);
            Show(otherOne, false);
            yield return new WaitUntil(() => !myMachine.machining);
            Show(safe);
        }
        else if (i == 2)
        {
            Show(safe, false);
            StartCoroutine(s1.Spawn1Pack(false));
            Show(unpacked);
            Show(ua1);
            yield return new WaitUntil(() => tapeMachine.machining);
            Show(ua1, false);
            yield return new WaitUntil(() => !tapeMachine.machining);
            Show(ua2);
        }
        else if (i == 3)
        {
            Show(unpacked, false);
            Show(ua2, false);
            StartCoroutine(level.Play());
            StartCoroutine(s1.Spawn1());
        }
        yield return null;
    }
}