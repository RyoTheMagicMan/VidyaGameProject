﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 target;
    public float speed;
    public int damageAmount;
	// Use this for initialization
	void Start ()
    {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
        Destroy(gameObject, 2.0f);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyHealthManager>().DamageEnemy(damageAmount);
            Destroy(gameObject);
        }
    }

}
