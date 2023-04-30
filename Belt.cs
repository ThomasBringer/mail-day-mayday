using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Belt : MonoBehaviour
{
    [SerializeField] Transform packOutput;

    [SerializeField] LayerMask packLayer;

    void Update()
    {
        Collider2D collOut = ((Vector2)packOutput.position).CheckGet(packLayer);
        if (collOut != null)
        {
            Pack pack = collOut.GetComponent<Pack>();
            if (!pack.carried && !pack.belting)
            {
                return;
            }
        }

        Collider2D coll = ((Vector2)transform.position).CheckGet(packLayer);
        if (coll != null)
        {
            Pack pack = coll.GetComponent<Pack>();
            if (!pack.carried && !pack.belting)
            {
                StartCoroutine(Work(pack));
            }
        }
    }

    public float speed = 1;
    [SerializeField] float slideTime = 1;

    void Awake()
    {
        Speed s = GetComponentInChildren<Speed>();
        s.speed = speed;
    }

    [HideInInspector] public bool belting = false;

    IEnumerator Work(Pack pack)
    {
        pack.belting = true;
        belting = true;

        pack.beltTweener = pack.transform.DOMove(packOutput.position, slideTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(slideTime);

        pack.belting = false;
        belting = false;
        // pack.SetSortOrder(-1);


        // float elapsedTime = 0f;
        // while (elapsedTime < workTime)
        // {
        //     bar.fillAmount = elapsedTime / workTime;
        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }
        // bar.fillAmount = 0;
        // pack.transform.DOMove(packOutput.position, slideTime).SetEase(Ease.Linear);
        // yield return new WaitForSeconds(slideTime);
        // pack.SetSortOrder(0);
    }
}
