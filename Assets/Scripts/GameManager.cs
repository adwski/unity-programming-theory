using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Platform> platforms;
    [SerializeField] Platform startingPlatform;

    int _gems;

    List<int> gemWaves = new List<int>();

    // ENCAPSULATION
    public int gems
    {
        get { return _gems; }
    }

    private void Awake()
    {
        startingPlatform.Activate();
    }

    public void CollectGem()
    {
        _gems++;
        Debug.Log("Gems: " + _gems);

        checkAndUnlockPlatforms();
    }

    public void SetGemsSpawned(int num)
    {
        if (num < 0) {
            num = 0;
        }
        gemWaves.Add(num);
    }

    void checkAndUnlockPlatforms()
    {
        float gemsForUnlock = _gems;
        for (int i = 0; i < gemWaves.Count; i++)
        {
            if (gemWaves[i] <= gemsForUnlock)
            {
                if (!platforms[i].active) {
                    platforms[i].Activate();
                }
            }
            gemsForUnlock -= gemWaves[i];
        }
    }
}
