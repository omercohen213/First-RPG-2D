using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingEntity
{

    // XP
    [SerializeField] private int xpValue = 10;
    [SerializeField] private Player player;

    // Logic
    public float triggerLength = 1;
    public float chaseLength = 5;
    public float chaseSpeed = 1.5f;
    public float returnSpeed = 3.0f;
    private bool chasing=false;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPos;


    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameObject.Find("Player").transform;
        //playerTransform = GameManager.instance.player.transform
        startingPos = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {      
        // Is player in chasing range?
        if (Vector3.Distance(startingPos, playerTransform.position) < chaseLength)
        {
            if (!chasing)
                UpdateMotor(startingPos - transform.position, returnSpeed);

            // Is player in trigger range?
            if (Vector3.Distance(startingPos, playerTransform.position) < triggerLength)
                chasing = true;

            if (chasing)
                if (!collidingWithPlayer)
                    UpdateMotor((playerTransform.position - transform.position).normalized, chaseSpeed); // Go to player
                else UpdateMotor(startingPos - transform.position, returnSpeed); // Go back to the starting position
        }
        else
        {
            // Player is not in range anymore, go back to the starting position                   
            UpdateMotor(startingPos - transform.position, returnSpeed);
            chasing = false;

        }

        // Check for overlaps
        collidingWithPlayer = false;

        // Collision work
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            if (hits[i].tag=="Fighter" && hits[i].name == "Player")
                collidingWithPlayer=true;

            // Array is not cleaned up so we do it by ourselves
            hits[i] = null;

        }      
    }

    protected override void Death()
    {
        Destroy(gameObject);
        player.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 20, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
        
    }

}
