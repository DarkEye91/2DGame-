using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    private bool hit;
    private float direction;
    private BoxCollider2D _boxCollider2D;
    private PlayerAttack _playerAttack;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        hit = true;
        _boxCollider2D.enabled = false;
        anim.SetTrigger("explode");
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        _boxCollider2D.enabled = true;

        float LocalScaleX = transform.localScale.x;
        if (Mathf.Sign(LocalScaleX) != direction)
            LocalScaleX = -LocalScaleX;

        transform.localScale = new Vector3(LocalScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
