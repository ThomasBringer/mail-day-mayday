using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class Pack : MonoBehaviour
{
    [SerializeField] SpriteRenderer regular, highlight;

    Collider2D coll;
    [HideInInspector] public bool carried = false;

    [SerializeField] LayerMask tableLayer;

    [HideInInspector] public int destination;
    [HideInInspector] public bool bomb = false;
    bool packed = true;
    public bool Packed { get { return packed; } set { packed = value; DoPack(value); } }

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    public void Highlight(bool active = true)
    {
        regular.enabled = !active;
        highlight.enabled = active;
    }

    public void GetCarried(Transform carrier)
    {
        if (belting)
        {
            belting = false;
            beltTweener.Kill();
        }
        carried = true;
        transform.SetParent(carrier);
        transform.DOLocalMove(Vector2.zero, .1f);
        Highlight(false);
        coll.enabled = false;
        SetSortOrder();
    }

    // Vector2 Target => ((Vector2)transform.position).SnapToSemi();

    // Vector2 Target
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

    SortingGroup sg;

    void OnEnable()
    {
        sg = GetComponent<SortingGroup>();
        CheckTable();
    }

    void CheckTable()
    {
        bool table = ((Vector2)transform.position).Check(tableLayer);// Physics2D.OverlapCircle(transform.position, .1f, tableLayer);
        SetSortOrder(table ? 5 : 0);
    }

    public void SetSortOrder(int i = 0)
    {
        sg.sortingOrder = i;
        // regular.sortingOrder = i;
        // highlight.sortingOrder = i;
    }

    public void GetDropped(Vector2 target)
    {
        transform.SetParent(null);
        transform.DOMove(target, .1f);
        StartCoroutine(GetDroppedEnd());
    }

    IEnumerator GetDroppedEnd()
    {
        carried = false;
        yield return new WaitForSeconds(.1f);
        coll.enabled = true;
        CheckTable();
    }

    [HideInInspector] public bool belting = false;
    [HideInInspector] public Tweener beltTweener;

    [HideInInspector] public bool machining = false;

    [HideInInspector] public bool ending = false;

    [SerializeField] SpriteRenderer badge;
    [SerializeField] SpriteRenderer cross;
    [SerializeField] Color[] badgeColors;

    public void ShowBadge()
    {
        badge.color = badgeColors[destination];
        badge.enabled = true;
    }

    [SerializeField] Sprite packedSprite;
    [SerializeField] Sprite unpackedSprite;

    void DoPack(bool value)
    {
        highlight.sprite = regular.sprite = value ? packedSprite : unpackedSprite;
    }

    public void MarkBomb()
    {
        if (bomb)
            cross.enabled = true;
    }
}