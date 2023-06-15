using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    static private PowerUpManager instance;
    static public PowerUpManager Instance { get { return instance; } }

    [SerializeField] List<PowerUp> powerUpList;

    public void SpawnMedikit(Vector3 position)
    {
        Instantiate(powerUpList[0], position, Quaternion.identity);
    }

    public void SpawnArmor(Vector3 position)
    {
        Instantiate(powerUpList[1], position, Quaternion.identity);
    }

    public void SpawnGrenade(Vector3 position)
    {
        Instantiate(powerUpList[2], position, Quaternion.identity);
    }

    public void SpawnHoming(Vector3 position)
    {
        Instantiate(powerUpList[3], position, Quaternion.identity);
    }

    public void SpawnRandomPowerUp(Vector3 position)
    {
        int randIndex = Random.Range(0, powerUpList.Count);
        Instantiate(powerUpList[randIndex], position, Quaternion.identity);
    }


}
