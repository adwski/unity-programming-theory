using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] GameObject tree;
    [SerializeField] float treeProbability;
    [SerializeField] GameObject diamond;
    [SerializeField] float diamondProbability;

    GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SpawnObjects(transform);
    }

    // SpawnObjects spawns objects at the island
    void SpawnObjects(Transform tran)
    {
        float step = 1;
        float scaleX = tran.localScale.x;
        float scaleZ = tran.localScale.x;
        float startX = tran.position.x - scaleX / 2;
        float startZ = tran.position.z - scaleZ / 2;
        float endX = startX + scaleX;
        float endZ = startZ + scaleZ;
        float y = tran.position.y;

        int diamondSpawned = 0;

        for (float x = startX + 1; x < endX; x += step)
        {
            for (float z = startZ + 1; z < endZ; z += step)
            {
                if (Random.Range(0f, 1f) < treeProbability)
                {
                    SpawnTree(new Vector3(x, y, z));
                }
                else if (Random.Range(0f, 1f) < diamondProbability)
                {
                    SpawnDiamond(new Vector3(x, y, z));
                    diamondSpawned++;
                }
            }
        }

        gameManager.SetGemsSpawned(diamondSpawned);
    }

    void SpawnTree(Vector3 pos)
    {
        //Debug.Log("Instatiating tree at " + pos);
        Instantiate(tree, pos, Quaternion.identity);
    }

    void SpawnDiamond(Vector3 pos)
    {
        //Debug.Log("Instatiating diamond at " + pos);
        Instantiate(diamond, pos + Vector3.up * 1.5f, Quaternion.Euler(new Vector3(90,0,0)));
    }
}
