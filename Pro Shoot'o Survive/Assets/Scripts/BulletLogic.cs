using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float raycastDistance;

    public bool IsHoming;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsHoming) HomingMovement();
        else StandardMovement();
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward,out hit, raycastDistance))
        {
            if(hit.collider.tag == "Enemy")
            {
                //enemy damaged
            }
            Destroy(gameObject);
        }
    }

    private void StandardMovement()
    {
        transform.position += new Vector3(0, 0, bulletSpeed) * Time.deltaTime;
    }

    private void HomingMovement() { }
}
