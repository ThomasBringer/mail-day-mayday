using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject joe, inPipe, package, table, outPipe;

    PlayerMove pMove;
    PlayerCarry pCarry;
    Spawner1 s1;
    Level level;

    void Awake()
    {
        pMove = FindObjectOfType<PlayerMove>();
        pCarry = FindObjectOfType<PlayerCarry>();
        s1 = FindObjectOfType<Spawner1>();
        level = FindObjectOfType<Level>();

        joe.SetActive(false);
        inPipe.SetActive(false);
        package.SetActive(false);
        table.SetActive(false);
        outPipe.SetActive(false);

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
        Show(joe);
        yield return new WaitUntil(() => pMove.hasMoved);
        yield return new WaitForSeconds(1);
        Show(joe, false);
        Show(inPipe);
        yield return new WaitForSeconds(6);
        Show(inPipe, false);
        StartCoroutine(s1.Spawn1());
        Show(package);
        yield return new WaitUntil(() => pCarry.carrying);
        yield return new WaitForSeconds(1);
        Show(package, false);
        Show(table);
        yield return new WaitUntil(() => !pCarry.carrying);
        yield return new WaitForSeconds(1);
        Show(table, false);
        Show(outPipe);
    }

    public IEnumerator Part(int i)
    {
        if (i == 1)
        {
            StartCoroutine(level.Play());

            Show(outPipe, false);
            StartCoroutine(s1.Spawn1());
            yield return new WaitForSeconds(3);
            StartCoroutine(s1.Spawn1());
        }
    }

    // public IEnumerator SndPart()
    // {
    //     Show(outPipe, false);
    //     StartCoroutine(s1.Spawn1());
    //     yield return new WaitForSeconds(3);
    //     StartCoroutine(s1.Spawn1());
    // }
}