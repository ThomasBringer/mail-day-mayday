using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3.5f, carryingSpeed = 1;
    [HideInInspector] public float actualSpeed;
    Rigidbody2D rb;
    Animator anim;
    RealVelocity rv;

    Vector2 prevPos;

    void Awake()
    {
        actualSpeed = speed;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        rv = GetComponent<RealVelocity>();

        prevPos = rb.position;
    }

    [HideInInspector] public Vector2 lastNonZeroInput;
    [HideInInspector] public bool isInputZero = false;

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.sqrMagnitude > 1)
            input.Normalize();

        rb.velocity = actualSpeed * input + BeltSpeed;

        isInputZero = input == Vector2.zero;
        if (!isInputZero) lastNonZeroInput = input;
        // anim.SetBool("Move", rv.HasMoved);
        // transform.Translate(speed * Time.deltaTime * input);
    }

    [HideInInspector] public bool hasMoved = false;

    void FixedUpdate()
    {
        Vector2 eps = rb.position - prevPos - Time.fixedDeltaTime * BeltSpeed;
        hasMoved = eps.sqrMagnitude > .0001f;
        anim.SetBool("Move", hasMoved);
        prevPos = rb.position;
    }


    [SerializeField] LayerMask beltLayer;

    Vector2 BeltSpeed
    {
        get
        {
            Collider2D[] colls = Physics2D.OverlapBoxAll((Vector2)transform.position - .25f * Vector2.up, .25f * Vector2.one, 0, beltLayer);
            if (colls.Length == 0)
                return Vector2.zero;
            Belt closest = colls.FindClosestCollider2D(transform.position).GetComponent<Belt>();
            return closest.speed * closest.transform.right;
        }
    }
}
// ContactFilter2D filter = new ContactFilter2D();
//             filter.layerMask = beltLayer;
//             Collider2D[] colls = new Collider2D[4];
//             int x = Physics2D.OverlapCollider(coll, filter, colls);
//             if (x == 0)
//                 return Vector2.zero;

//             Debug.Log("hu");
//             // Collider2D[] colls = Physics2D.OverlapCo OverlapCircleAll(transform.position, .5f, beltLayer);
//             // if (colls.Length == 0)
//             //     return Vector2.zero;
//             Belt closest = colls.FindClosestCollider2D(transform.position).GetComponent<Belt>();
//             return closest.speed * closest.transform.right;
//         }