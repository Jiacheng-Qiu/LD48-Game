using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create new pipes if player reach the end of previous ones, and delete the earlier ones
public class InfinityPipe : MonoBehaviour
{
    public int initAmount = 3;
    public GameObject pipePrefab;
    public GameObject player;

    private ArrayList pipes;
    private float angle;
    private void Start()
    {
        pipes = new ArrayList();
        angle = Mathf.Abs(pipePrefab.transform.rotation.z);
        // Generate three pipes
        for (int i = 0; i < initAmount; i++)
        {
            GenerateNewPipe();
        }
    }
    void FixedUpdate()
    {
        // Delete passed pipes, and create new (check player position)
        if (player.transform.position.x - ((GameObject)pipes[0]).transform.position.x > 17.7f)
        {
            Destroy((GameObject)pipes[0]);
            pipes.RemoveAt(0);
            GenerateNewPipe();
        }
    }

    // Generate a new pipe based on previous position
    private void GenerateNewPipe()
    {
        pipes.Add(Instantiate(pipePrefab));
        int index = pipes.Count - 1;
        if (index == 0)
        {
            ((GameObject)pipes[index]).transform.position = new Vector2(0, 0);
        }
        else
        {
            Vector2 prevPos = ((GameObject)pipes[index - 1]).transform.position;
            ((GameObject)pipes[index]).transform.position = new Vector2(prevPos.x + 17.7f, prevPos.y - 8.255f);
        }
    }
}
