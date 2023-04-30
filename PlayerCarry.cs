using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] LayerMask packLayer, packBlockLayer, beltOutLayer, machineOutLayer;
    [SerializeField] float range = 2;

    Pack pack;
    [HideInInspector] public bool carrying = false;

    Rigidbody2D rb;
    PlayerMove pMove;

    [SerializeField] GameObject spot;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pMove = GetComponent<PlayerMove>();
    }

    void TryUnHighlight()
    {
        if (pack != null)
            pack.Highlight(false);
    }

    void Update()
    {
        if (carrying)
        {
            (bool success, Vector2 target) = DropTarget;
            bool showSpot = !pMove.hasMoved && success;
            spot.SetActive(showSpot);
            if (success)
            {
                if (showSpot) spot.transform.position = target;
                TryDrop(target);
            }
        }
        else
        {
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, range, packLayer);
            if (colls.Length == 0)
            {
                TryUnHighlight();
                pack = null;
                return;
            }
            Pack closest = colls.FindClosestCollider2D(transform.position).GetComponent<Pack>();

            if (closest.machining || closest.ending) return;

            if (pack != closest)
            {
                TryUnHighlight();
                closest.Highlight(true);
            }
            pack = closest;

            TryCarry();
        }
    }

    void TryCarry()
    {
        if (Input.GetKeyDown("space"))
        {
            Carry();
        }
    }

    void Carry()
    {
        carrying = true;
        pMove.actualSpeed = pMove.carryingSpeed;
        pack.GetCarried(transform);
    }

    void TryDrop(Vector2 target)
    {
        if (Input.GetKeyDown("space"))
        {
            Drop(target);
        }
    }

    void Drop(Vector2 target)
    {
        carrying = false;
        pMove.actualSpeed = pMove.speed;

        pack.GetDropped(target);
        spot.SetActive(false);
    }

    // Vector2 DropTarget
    // {
    //     get
    //     {
    //         Vector2 p = transform.position;
    //         Vector2 centre = p.SnapToSemi();
    //         Vector2 offset = p - centre;

    //         if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
    //             return centre + Mathf.Sign(offset.x) * Vector2.right;
    //         else
    //             return centre + Mathf.Sign(offset.y) * Vector2.up;
    //     }
    // }



    (bool, Vector2) DropTarget
    {
        get
        {
            Vector2 p = (Vector2)transform.position - .15f * Vector2.up;
            Vector2 target = p + .5f * pMove.lastNonZeroInput.normalized;
            Vector2 v = target.SnapToSemi();

            // Vector2 centre = p.SnapToSemi();
            // Vector2 offset = p - centre;

            // Vector2 v = centre + pMove.lastNonZeroInput.SnapToCardinal();
            if (!v.Check(packBlockLayer))
            {
                Collider2D beltOut = v.CheckGet(beltOutLayer);
                if (beltOut == null || !beltOut.transform.parent.GetComponent<Belt>().belting)
                {
                    Collider2D machineOut = v.CheckGet(machineOutLayer);
                    if (machineOut == null || !machineOut.transform.parent.GetComponent<Machine>().machining)
                    {
                        return (true, v);
                    }
                }
            }
            return (false, Vector2.zero);

            // if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
            // {
            //     foreach (Vector2 v in new Vector2[]{
            //         centre + pMove.lastNonZeroInput.SnapToCardinal()
            //         // centre + Mathf.Sign(offset.x) * Vector2.right,
            //         // centre + Mathf.Sign(offset.y) * Vector2.up,
            //         // centre - Mathf.Sign(offset.y) * Vector2.up,
            //         // centre - Mathf.Sign(offset.x) * Vector2.right
            //         })
            //     {
            //         if (!v.Check(packBlockLayer))
            //         {
            //             Collider2D beltOut = v.CheckGet(beltOutLayer);
            //             if (beltOut == null || !beltOut.transform.parent.GetComponent<Belt>().belting)
            //             {
            //                 return (true, v);
            //             }
            //         }
            //     }
            // }
            // else
            // {
            //     foreach (Vector2 v in new Vector2[]{
            //         centre + pMove.lastNonZeroInput.SnapToCardinal()
            //         // centre + Mathf.Sign(offset.y) * Vector2.up,
            //         // centre + Mathf.Sign(offset.x) * Vector2.right,
            //         // centre - Mathf.Sign(offset.x) * Vector2.right,
            //         // centre - Mathf.Sign(offset.y) * Vector2.up,
            //         })
            //     {
            //         if (!v.Check(packBlockLayer))
            //         {
            //             Collider2D beltOut = v.CheckGet(beltOutLayer);
            //             if (beltOut == null || !beltOut.transform.parent.GetComponent<Belt>().belting)
            //             {
            //                 return (true, v);
            //             }
            //         }
            //     }
            // }
            // return (false, Vector2.zero);
        }
    }
}