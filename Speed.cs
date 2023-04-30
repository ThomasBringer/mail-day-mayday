using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Speed : MonoBehaviour
{
    [SerializeField] Vector2 start, end;
    public float speed;
    float delay;

    void Start()
    {
        delay = (end - start).magnitude / speed;
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        while (true)
        {
            transform.localPosition = start;
            transform.DOLocalMove(end, delay).SetEase(Ease.Linear);
            yield return new WaitForSeconds(delay);
        }
    }
}