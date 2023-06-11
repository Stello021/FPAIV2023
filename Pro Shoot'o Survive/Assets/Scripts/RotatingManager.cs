using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingManager : MonoBehaviour
{
    [SerializeField]
    public List<Transform> rotatingObjects;
    [SerializeField]
    private Vector3 rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject powerUp in powerUps)
        {
            rotatingObjects.Add(powerUp.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rotatingObjects.Count == 0)
        {
            return;
        }
        
        for (int i = 0; i < rotatingObjects.Count; i++)
        {
            if (rotatingObjects[i] != null)
            {
                rotatingObjects[i].Rotate(rotSpeed, Space.World);
            }
            else
            {
                rotatingObjects.Remove(rotatingObjects[i]);
            }
        }
        
    }
}
