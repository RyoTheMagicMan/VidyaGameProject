﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBossController : MonoBehaviour
{ 
    // ******* movement stuffz ************
    public float moveSpeed;
    public float timeBetweenMove;
    public float timeToMove;
    public Transform[] movementPositions;
    private bool isMoving;
    private float timeToMoveCounter;
    private Vector3 moveDir;
    private Transform moveTarget;
    private float timeBetweenMoveCounter;
    public bool inFight;

    // **** spell stuffz ************* 
    public Transform[] spellSpawnPositions;
    public float timeBetweenCastSpell;
    public GameObject spellPrefab;
    public float spellSpeed;
    public float targetSpellSpeed;
    private float castSpellCounter;
    private GameObject[] spellz;
    private Transform target;
    private SoundManager dj;
   

    // **** animator object **************
    private Animator anim;
    
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        timeBetweenMoveCounter = timeBetweenMove;
        timeToMoveCounter = timeToMove;
        castSpellCounter = timeBetweenCastSpell;
        target = GameObject.FindWithTag("Player").GetComponent<PlayerController>().transform;
        dj = SoundManager._instance;
        spellz = new GameObject[8];
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            timeToMoveCounter -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, moveTarget.position, moveSpeed * Time.deltaTime);
            
            if (timeToMoveCounter < 0.0f)
            {
                isMoving = false;
                timeBetweenMoveCounter = timeBetweenMove;
            }
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            if (timeBetweenMoveCounter < 0.0f)
            {
                isMoving = true;
                timeToMoveCounter = timeToMove;
                moveTarget = movementPositions[Random.Range(0, movementPositions.Length)];
                moveDir = moveTarget.position;
            }
        }

        if ((Vector2.Distance(transform.position, target.position) < 15.0f))
        {
            inFight = true;
            if (castSpellCounter > 0.0f)
                castSpellCounter -= Time.deltaTime;

            if (castSpellCounter < 0.0f)
            {
                Vector2 spellDir = (target.position - transform.position).normalized * targetSpellSpeed;
                CastSpell(spellDir); // spell counter is less than zero so we can cast the boss spell
                castSpellCounter = timeBetweenCastSpell;
            }
        }
    
        SetAnimation(); // this sets the npc's animation object to the correct movement
    }


    private void CastSpell(Vector2 spellDirection)
    {
        GameObject spell = Instantiate(spellPrefab, transform.position, transform.rotation);
        spell.GetComponent<Rigidbody2D>().velocity = new Vector2(spellDirection.x, spellDirection.y);
        dj.PlaySFX("WaterBossSFX");
        Destroy(spell, 3.0f);

        for (int i = 0; i < spellSpawnPositions.Length; i++)
        {
            spellz[i] = (GameObject)Instantiate(spellPrefab, spellSpawnPositions[i].position, Quaternion.identity);
            switch (i)
            {
                default:
                    break;
                case 0: // left position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = Vector2.left * spellSpeed;
                    break;
                case 1: // right position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = Vector2.right * spellSpeed;
                    break;
                case 2: // up position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = Vector2.up * spellSpeed;
                    break;
                case 3: // down position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = Vector2.down * spellSpeed;
                    break;
                case 4: // down left position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = (spellSpawnPositions[i].position - transform.position) * spellSpeed;
                    break;
                case 5: // down right position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = (spellSpawnPositions[i].position - transform.position) * spellSpeed;
                    break;
                case 6: // up right position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = (spellSpawnPositions[i].position - transform.position) * spellSpeed;
                    break;
                case 7: // up left position
                    spellz[i].GetComponent<Rigidbody2D>().velocity = (spellSpawnPositions[i].position - transform.position) * spellSpeed;
                    break;
            }
            dj.PlaySFX("WaterBossSFX");
            Destroy(spellz[i], 2.0f);
        }
            
    }

    private void SetAnimation()
    {
        anim.SetFloat("MoveX", moveDir.x);
        anim.SetFloat("MoveY", moveDir.y);
        anim.SetBool("isMoving", isMoving);
    }

}
