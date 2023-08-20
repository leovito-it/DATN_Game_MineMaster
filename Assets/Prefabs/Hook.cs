using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Hook : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D box;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CreateVFX();
        RopeController.Instance.RegisterObject(collision.gameObject);
    }

    void CreateVFX()
    {

    }


}
