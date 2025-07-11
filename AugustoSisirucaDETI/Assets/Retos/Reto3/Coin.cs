﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Coin's logic.
/// 
/// @author ShifatKhan
/// </summary>
public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    public TextMeshProUGUI valueText;

    public bool enableGravity = false;
    public MonedaScript monedaScript;

    private bool addForceOnSpawn = false;
    private Vector3 direction;
    private Rigidbody rb;

    private Health health;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = !enableGravity;
    }

    private void Start()
    {
        // Add force on spawn if enabled.
        if (addForceOnSpawn)
        {
            rb.isKinematic = !enableGravity;
            rb.AddForce(direction, ForceMode.Impulse);
        }

        health = GetComponent<Health>();
    }

    private void Update()
    {
        rb.isKinematic = !enableGravity;
    }

    /// <summary>
    /// Used for effects such as Sonic's rings when he gets hit.
    /// </summary>
    /// <param name="direction"></param>
    public void AddForceOnSpawn(Vector3 direction)
    {
        this.direction = direction;
        addForceOnSpawn = true;
        enableGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Increment score.
           monedaScript.CantidadMonedas += value;
           valueText.text = monedaScript.CantidadMonedas.ToString();


            // FX
            health.Damage(99, Vector3.zero);
        }

        // TODO: add sfx for dropping on ground
    }
}
