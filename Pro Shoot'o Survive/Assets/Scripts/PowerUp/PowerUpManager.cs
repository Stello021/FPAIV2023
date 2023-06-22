using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    static private PowerUpManager instance;
    static public PowerUpManager Instance { get { return instance; } }

    [SerializeField] List<GameObject> powerUpList;

    private void Start()
    {
        instance = this;
    }

    public void SpawnMedikit(Vector3 position)
    {
        GameObject powerUp = Instantiate(powerUpList[0], position, Quaternion.identity);
        RotatingManager.Instance.AddRotatingObject(powerUp.transform);
    }

    public void SpawnArmor(Vector3 position)
    {
        GameObject powerUp = Instantiate(powerUpList[1], position, Quaternion.identity);
        RotatingManager.Instance.AddRotatingObject(powerUp.transform);
    }

    public void SpawnGrenade(Vector3 position)
    {
        GameObject powerUp = Instantiate(powerUpList[2], position, Quaternion.identity);
        RotatingManager.Instance.AddRotatingObject(powerUp.transform);
    }

    public void SpawnHoming(Vector3 position)
    {
        GameObject powerUp = Instantiate(powerUpList[3], position, Quaternion.identity);
        RotatingManager.Instance.AddRotatingObject(powerUp.transform);
    }

    public void SpawnRandomPowerUp(Vector3 position)
    {
        int randIndex = Random.Range(0, powerUpList.Count);
        GameObject powerUp = Instantiate(powerUpList[randIndex], position, Quaternion.identity);
        RotatingManager.Instance.AddRotatingObject(powerUp.transform);
    }


}
