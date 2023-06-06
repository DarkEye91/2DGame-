using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;
    [SerializeField] private AudioClip fireballsound;
    private Animator anim;
    private PlayerMovement _playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && cooldownTimer > attackCooldown && _playerMovement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }
    private void Attack()
    {
        SoundManager.instance.PlaySound(fireballsound );
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireBalls[0].transform.position = firePoint.position;
        fireBalls[0].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        
    }
}
