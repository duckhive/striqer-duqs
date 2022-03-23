using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float initialVelocity;
    [SerializeField] private float angle;

    private Player player;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float _angle = angle * Mathf.Deg2Rad;
            StopAllCoroutines();
            StartCoroutine(CoMovement(initialVelocity, angle));
        }
    }

    private IEnumerator CoMovement(float v0, float angle)
    {
        float t = 0;
        while (t < 100)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = new Vector3(x, y, 0);
            
            t -= Time.deltaTime;
            yield return null;
        }
    }
}
