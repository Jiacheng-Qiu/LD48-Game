using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create new pipes if player reach the end of previous ones, and delete the earlier ones
public class InfinityPipe : MonoBehaviour
{
    public float length;
    public float width;
    public int initAmount = 3;
    public GameObject pipePrefab;
    public GameObject waterPrefab;
    public GameObject rockPrefab;
    public GameObject crackPrefab;
    public GameObject enemyPrefab;
    public GameObject player;

    private ArrayList pipes;
    private float angle;
    private void Start()
    {
        pipes = new ArrayList();
        // Generate three pipes
        for (int i = 0; i < initAmount; i++)
        {
            GenerateNewPipe();
        }
    }
    void FixedUpdate()
    {
        // Delete passed pipes, and create new (check player position)
        if (player.transform.position.x - ((GameObject)pipes[0]).transform.position.x > length)
        {
            Destroy((GameObject)pipes[0]);
            pipes.RemoveAt(0);
            GenerateNewPipe();
        }
    }

    // Generate a new pipe based on previous position
    private void GenerateNewPipe()
    {
        transform.rotation = new Quaternion();
        pipes.Add(Instantiate(pipePrefab));
        int index = pipes.Count - 1;
        // TODO: Generate enemy and objects in pipes
        GenerateAI((GameObject)pipes[index]);
        if (index == 0)
        {
            ((GameObject)pipes[index]).transform.position = new Vector2(0, 0);
        }
        else
        {
            Vector2 prevPos = ((GameObject)pipes[index - 1]).transform.position;
            ((GameObject)pipes[index]).transform.position = new Vector2(prevPos.x + length, prevPos.y);
        }
        ((GameObject)pipes[index]).transform.parent = transform;
        transform.rotation = Quaternion.AngleAxis(-25, new Vector3(0, 0, 1));
    }

    // Generate objects in pipes
    private void GenerateAI(GameObject pipe)
    {
        // TODO: Make sure they dont overlap
        GenerateDrops().parent = pipe.transform;
        GenerateStones().parent = pipe.transform;
        //GenerateCracks().parent = pipe.transform;
    }

    private Transform GenerateDrops()
    {
        // Generate two types of drops, moving ones and fixed ones
        GameObject drop = Instantiate(waterPrefab);
        drop.transform.position = new Vector3(Random.Range(-length/2, length/2), Random.Range(-width / 2, width / 2), 0);
        return drop.transform;
    }

    private Transform GenerateStones()
    {
        GameObject rock = Instantiate(rockPrefab);
        float scale = Random.Range(1.2f, 2.5f);
        rock.transform.localScale = new Vector3(scale, scale, 1);
        rock.transform.position = new Vector3(Random.Range(-length / 2, length / 2), -width / 2 + rock.GetComponent<Collider2D>().bounds.size.y / 2, 0);
        return rock.transform;
    }

    private Transform GenerateCracks()
    {
        GameObject crack = Instantiate(crackPrefab);
        crack.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), 1, 1);
        crack.transform.position = new Vector3(Random.Range(-length / 2, length / 2), -width / 2, 0);
        return crack.transform;
    }

    private Transform GenerateEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), 1, 1);
        enemy.transform.position = new Vector3(Random.Range(-length / 2, length / 2), width / 2, 0);
        return enemy.transform;
    }
}
