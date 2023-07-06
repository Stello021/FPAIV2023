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

    public void SpawnRandomPowerUp(Vector3 position)
    {
        int randIndex = Random.Range(0, powerUpList.Count);
        GameObject powerUp = Instantiate(powerUpList[randIndex], position, Quaternion.identity);
        RotatingManager.Instance.AddRotatingObject(powerUp.transform);
    }


}
