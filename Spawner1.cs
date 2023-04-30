using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner1 : MonoBehaviour
{
    [SerializeField] float unpackProb = .5f;
    bool RandomUnpack => Random.Range(0f, 1f) < unpackProb;

    [SerializeField] float bombProb = .5f;
    bool RandomBomb => Random.Range(0f, 1f) < bombProb;

    [SerializeField] int destinationCount = 3;
    int RandomDestination => Random.Range(0, destinationCount);

    [SerializeField] GameObject packPrefab;

    [SerializeField] Transform spawnPoint, spawnOut;

    [SerializeField] float speed;
    float delay;

    void Awake()
    {
        delay = (spawnPoint.position - spawnOut.position).magnitude / speed;
    }

    public IEnumerator Spawn1()
    {
        yield return StartCoroutine(Spawn1(!RandomUnpack, RandomBomb, RandomDestination));
    }

    public IEnumerator Spawn1Pack(bool myPacked)
    {
        yield return StartCoroutine(Spawn1(myPacked, RandomBomb, RandomDestination));
    }
    public IEnumerator Spawn1Bomb(bool myBomb)
    {
        yield return StartCoroutine(Spawn1(!RandomUnpack, myBomb, RandomDestination));
    }
    public IEnumerator Spawn1Destination(int myDestination)
    {
        yield return StartCoroutine(Spawn1(!RandomUnpack, RandomBomb, myDestination));
    }

    public IEnumerator Spawn1(bool myPacked, bool myBomb, int myDestination)
    {
        Pack pack = Instantiate(packPrefab, spawnPoint.position, Quaternion.identity).GetComponent<Pack>();
        pack.SetSortOrder(-1);
        pack.transform.DOMove(spawnOut.position, delay).SetEase(Ease.Linear);

        pack.Packed = myPacked;
        if (myPacked && myBomb)
            pack.bomb = true;

        pack.destination = myDestination;
        yield return new WaitForSeconds(delay);
        pack.SetSortOrder(0);
    }
}
