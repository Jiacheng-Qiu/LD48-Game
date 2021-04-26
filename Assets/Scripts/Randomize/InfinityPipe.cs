using System.Collections;
using UnityEngine;

// Create new pipes if player reach the end of previous ones, and delete the earlier ones
public class InfinityPipe : MonoBehaviour
{
    public float length;
    public float width;
    public int initAmount = 3;
    private int curPipe;
    public GameObject pipePrefab;
    public GameObject waterPrefab;
    public GameObject rockPrefab;
    public Sprite rockAlternate;
    public GameObject crackPrefab;
    public GameObject enemyPrefab;
    public GameObject player;

    public AudioSource source;

    private ArrayList pipes;
    private float angle;
    private void Start()
    {
        source.loop = true;
        source.Play();
        // Play music on start
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
        GenerateDrops(pipe, curPipe);
        GenerateStones(pipe, curPipe);
        GenerateEnemy(pipe, curPipe);
        curPipe++;
    }

    private void GenerateDrops(GameObject pipe, int index)
    {
        index = 1 + index / 60;
        float len = length / (index + 1);
        for (int i = 0; i < index; i++)
        {
            GameObject drop = Instantiate(waterPrefab);
            drop.transform.position = new Vector3(Random.Range(-length / 2 + len * i, -length / 2 + len * (i + 1)), Random.Range(-width / 3, 0), 0);
            drop.transform.parent = pipe.transform;
        }
    }

    private void GenerateStones(GameObject pipe, int index)
    {
        GameObject rock = Instantiate(rockPrefab);
        if (Random.value > 0.5f)
        {
            rock.GetComponent<SpriteRenderer>().sprite = rockAlternate;
        }
        float scale = Random.Range(1.4f, 2.5f);
        rock.transform.localScale = new Vector3(scale, scale, 1);
        rock.transform.position = new Vector3(Random.Range(-length / 2.2f, length / 2.2f), -width / 2 + rock.GetComponent<Collider2D>().bounds.size.y / 2, 0);
        rock.transform.parent = pipe.transform;
       
    }

    private void GenerateEnemy(GameObject pipe, int j)
    {
        int index = Mathf.Clamp(j / 8, 0, 3);
        float len = length / (index + 1);
        for (int i = 0; i < index; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = new Vector3(Random.Range(-length / 2 + len * i, -length / 2 + len * (i + 1)), width / 6, 0);
            enemy.transform.parent = pipe.transform;
        }
    }
}