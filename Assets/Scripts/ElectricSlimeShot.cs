﻿using UnityEngine;
using System.Collections;

public class ElectricSlimeShot : SlimeShot {

    [SerializeField]
    private float eletricSlimeRadius;
    private YellowSlimePlayer yellowSlime;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        yellowSlime = GameObject.Find("YellowSlime(Clone)").GetComponent<YellowSlimePlayer>();
        damage = 0;
    }

    // Update is called once per frame
    protected override void Update()
    {
        shotDirection = (slimeShotTarget - transform.position).normalized;

        rBody.AddForce(shotDirection * shotSpeed); //Add forces to shoot the slime shot

        if (Mathf.Abs(slimeShotTarget.x - rBody.position.x) / 7 <= 0.05f && Mathf.Abs(slimeShotTarget.y - rBody.position.y) / 7 <= 0.05f)
        {
            if (!coroutineRunning)
            {
                Destroy(gameObject);
            }
        }
    }

    protected override void explode()
    {
        if (Mathf.Abs(slimeShotTarget.x - rBody.position.x) / 7 <= 0.05f && Mathf.Abs(slimeShotTarget.y - rBody.position.y) / 7 <= 0.05f)
        {
            // get all colliders
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, eletricSlimeRadius);

            // check each collider to see if its an enemy
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].transform.tag == "Enemy")
                {

                    StartCoroutine(colls[i].transform.GetComponent<Enemy>().EletrocuteEnemy(this.gameObject, yellowSlime.SlimeAttack2Damage));
                    coroutineRunning = true;
                }
            }
            if (!coroutineRunning)
            {
                Destroy(gameObject);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy") //If the slime is colliding with the enemy
        {
            rBody.constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;

            Enemy enemySlime = coll.gameObject.GetComponent<Enemy>();
            if (enemySlime.SlimeType != slimePlayerScript.SlimerType)
            {
                // get all colliders
                Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, eletricSlimeRadius);

                // check each collider to see if its an enemy
                for (int i = 0; i < colls.Length; i++)
                {
                    if (colls[i].transform.tag == "Enemy")
                    {

                        StartCoroutine(colls[i].transform.GetComponent<Enemy>().EletrocuteEnemy(this.gameObject, yellowSlime.SlimeAttack2Damage));
                        coroutineRunning = true;
                    }
                }
                if (!coroutineRunning)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if(coll.gameObject == yellowSlime.gameObject || coll.tag == "Player Detector")
        {
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
