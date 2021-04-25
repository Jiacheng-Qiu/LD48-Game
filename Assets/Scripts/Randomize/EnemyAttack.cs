using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;
    public float attackCD = 3f;
    private float nextAttack;
    private float prevAttack;
    private bool restore;

    void Start()
    {
        restore = true;
        nextAttack = Time.time + attackCD * Random.Range(0.6f, 1.4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!restore && Time.time - prevAttack > 0.2f)
        {
            restore = true;
            transform.localPosition += new Vector3(0, 0.18f, 0);
        }
        if (Time.time - prevAttack > 0.1f)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        } 
        else if (Time.time > prevAttack)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
            animator.SetBool("attack", false);
        if (Time.time >= nextAttack)
        {
            restore = false;
            Debug.Log("Fire");
            nextAttack = Time.time + attackCD * Random.Range(0.6f, 1.4f);
            animator.SetBool("attack", true);
            prevAttack = Time.time + 0.15f;
            transform.localPosition -= new Vector3(0, 0.18f, 0);
        }
    }
}
