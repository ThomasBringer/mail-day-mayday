using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class End : MonoBehaviour
{
    [SerializeField] Transform packInput, stop;

    [SerializeField] LayerMask packLayer;

    [SerializeField] float speed;

    [SerializeField] int destination;
    float delay;

    Level level;

    void Awake()
    {
        delay = (stop.position - packInput.position).magnitude / speed;
        level = FindObjectOfType<Level>();
    }

    void Update()
    {
        Collider2D coll = ((Vector2)packInput.position).CheckGet(packLayer);
        if (coll != null)
        {
            Pack pack = coll.GetComponent<Pack>();
            if (!pack.carried && !pack.belting && !pack.ending)
            {
                StartCoroutine(Work(pack));
            }
        }
    }

    [SerializeField] GameObject explosionPrefab;

    IEnumerator Work(Pack pack)
    {
        pack.ending = true;
        pack.SetSortOrder(-1);
        pack.transform.DOMove(stop.position, delay).SetEase(Ease.Linear);

        bool success = (destination == -1 && pack.bomb) || (pack.destination == destination && !pack.bomb);
        level.End(success);

        Debug.Log("success?" + success + "; delay: " + delay);
        if (success)
            Destroy(pack.gameObject, delay);
        else
        {
            yield return new WaitForSeconds(delay);
            pack.transform.DOMove(packInput.position, delay * .5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(delay * .5f);
            Destroy(pack.gameObject, delay);
            Instantiate(explosionPrefab, packInput.position, Quaternion.identity);
        }
    }
}