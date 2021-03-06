﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBossController : MonoBehaviour
{
    public float moveSpeed;
    private Animator anim; 
    private bool isMoving;
    private Transform target;

    // **** spell stuffz ************* 
    public Transform[] spellSpawnPositions;
    public Transform[] spellDirections;
    public float timeBetweenCastSpell;
    public GameObject spellPrefab;
    public float spellSpeed;
    public float targetSpellSpeed;
    private float castSpellCounter;
    private GameObject[] spellz;
    private SoundManager dj;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        dj = SoundManager._instance;
        target = GameObject.FindWithTag("Player").GetComponent<PlayerController>().transform;
        castSpellCounter = timeBetweenCastSpell;
        spellz = new GameObject[4];
    }
	
	// Update is called once per frame
	void Update ()
    {
	   if((Vector2.Distance(transform.position, target.position) < 15.0f))
       {
            isMoving = true;
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (castSpellCounter > 0.0f)
                castSpellCounter -= Time.deltaTime;

            if (castSpellCounter < 0.0f)
            {
                Vector2 spellDir = (target.position - transform.position).normalized * targetSpellSpeed;
                CastSpell(spellDir); // spell counter is less than zero so we can cast the boss spell
                castSpellCounter = timeBetweenCastSpell;
            }
       }
       else
       {
            isMoving = false;
       }

        SetAnimation();
        
    }

    public void CastSpell(Vector2 spellDirection)
    {
        GameObject spell = Instantiate(spellPrefab, transform.position, transform.rotation);
        spell.GetComponent<Rigidbody2D>().velocity = new Vector2(spellDirection.x, spellDirection.y);
        dj.PlaySFX("FireBossSFX");
        Destroy(spell, 3.0f);

        for (int i = 0; i < spellSpawnPositions.Length; i++)
        {
            spellz[i] = (GameObject)Instantiate(spellPrefab, spellSpawnPositions[i].position, Quaternion.identity);
            switch (i)
            {
                default:
                    break;
                case 0: // left position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = (spellDirections[i].position - spellSpawnPositions[i].position) * spellSpeed;
                    break;
                case 1: // right position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = (spellDirections[i].position - spellSpawnPositions[i].position) * spellSpeed;
                    break;
                case 2: // up position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = (spellDirections[i].position - spellSpawnPositions[i].position) * spellSpeed;
                    break;
                case 3: // down position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = (spellDirections[i].position - spellSpawnPositions[i].position) * spellSpeed;
                    break;
            }
            dj.PlaySFX("FireBossSFX");
            Destroy(spellz[i], 3.0f);
        }
    }

    private void SetAnimation()
    {
        //anim.SetFloat("MoveX", target.position.x);
        anim.SetFloat("MoveY", target.position.y);
        anim.SetBool("isMoving", isMoving);
    }
}
