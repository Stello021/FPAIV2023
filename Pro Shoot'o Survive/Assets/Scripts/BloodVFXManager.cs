using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodVFXManager : MonoBehaviour
{
    static private BloodVFXManager instance;
    public static BloodVFXManager Instance { get { return instance; } }

    [SerializeField] Queue<GameObject> bloodVFXPool;
    [SerializeField] GameObject bloodPrefab;
    [SerializeField] int VFXamount;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        bloodVFXPool = new Queue<GameObject>();
        GameObject inst;

        for (int i = 0; i < VFXamount; i++)
        {
            inst = Instantiate(bloodPrefab);
            inst.SetActive(false);
            bloodVFXPool.Enqueue(inst);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetBloodVFX(Vector3 position)
    {
        GameObject bloodVFX = bloodVFXPool.Dequeue();
        bloodVFX.transform.position = position;
        bloodVFX.SetActive(true);
        bloodVFX.GetComponent<ParticleSystem>().Play();
        bloodVFXPool.Enqueue(bloodVFX);
    }
}
