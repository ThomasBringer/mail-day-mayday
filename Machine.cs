using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    [SerializeField] Transform packInput, packOutput;

    [SerializeField] LayerMask packLayer;

    [SerializeField] Image bar;

    void Update()
    {
        Collider2D collOut = ((Vector2)packOutput.position).CheckGet(packLayer);
        if (collOut != null)
        {
            Pack pack = collOut.GetComponent<Pack>();
            if (!pack.carried)
            {
                return;
            }
        }

        if (machining) return;

        Collider2D coll = ((Vector2)packInput.position).CheckGet(packLayer);
        if (coll != null)
        {
            Pack pack = coll.GetComponent<Pack>();
            if (!pack.carried && !pack.belting)
            {
                StartCoroutine(Work(pack));
            }
        }
    }

    [SerializeField] float slideTime = 1, workTime = 3;

    [HideInInspector] public bool machining = false;

    [SerializeField] int machineType;

    IEnumerator Work(Pack pack)
    {
        machining = true;
        pack.machining = true;
        pack.transform.DOMove(transform.position, slideTime).SetEase(Ease.Linear);
        pack.SetSortOrder(-1);

        yield return new WaitForSeconds(slideTime);

        switch (machineType)
        {
            case 0: pack.ShowBadge(); break;
            case 1: pack.Packed = true; break;
            default: pack.MarkBomb(); break;
        }

        float elapsedTime = 0f;
        while (elapsedTime < workTime)
        {
            bar.fillAmount = elapsedTime / workTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        bar.fillAmount = 0;
        pack.transform.DOMove(packOutput.position, slideTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(slideTime);
        pack.SetSortOrder(0);
        machining = false;
        pack.machining = false;
    }
}