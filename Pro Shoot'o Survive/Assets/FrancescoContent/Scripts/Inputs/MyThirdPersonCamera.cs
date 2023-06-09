using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyThirdPersonCamera : MonoBehaviour
{
    public Transform parentTransform;   //the transform of the player or object you want the camera to focus on
    public float rotSpeed;              //how fast the camera rotates around the focussed object
    public int tolerance;
    Vector3 targToCamNoY;               //vector from the camera to the object of focus with y=0
    float yAngle;
    public float yAngleTol;             //the angle in degrees that the focus object to camera vector can be from the flat plane running throw the focus object's origin
    float yCompOrg;
    bool tooHigh;
    bool tooLow;
    Vector3 playerToCamera;

    // Use this for initialization
    void Start()
    {
        tooHigh = false;
        tooLow = false;
    }

    // Update is called once per frame
    void Update()
    {
        ////left right input/movement =======================================================================================
        //if (Input.mousePosition.x >= (mouseOrgX + tolerance)) //move right
        //{
        //    gameObject.transform.Translate(Vector3.right * Time.deltaTime * rotSpeed, Space.Self);
        //}
        //else if (Input.mousePosition.x <= (mouseOrgX - tolerance)) //move left
        //{
        //    gameObject.transform.Translate(Vector3.right * Time.deltaTime * -rotSpeed, Space.Self);
        //}
        ////========================================================================================================

        ////check to see if too high or low =============================================================
        //targToCamNoY = parentTransform.position - gameObject.transform.position;
        //yCompOrg = targToCamNoY.y;
        //targToCamNoY.y = 0;

        //yAngle = Vector3.Angle(targToCamNoY, parentTransform.position - gameObject.transform.position);
        ////Debug.Log("yAngle = " + yAngle + "yCompOrg = " + yCompOrg + " tooHigh = " + tooHigh + " tooLow = " + tooLow);
        //if (yAngle > yAngleTol && yCompOrg < 0) //too far up
        //{
        //    tooHigh = true;
        //}
        //else if (yAngle > yAngleTol && yCompOrg > 0) //too far down
        //{
        //    tooLow = true;
        //}
        ////=============================================================================================

        ////only listen to curtain input based on prior analysis==================================================
        //if (tooHigh) //if too high only listen to downward input
        //{
        //    if ((Input.mousePosition.y <= (mouseOrgY - tolerance))) //down
        //    {
        //        gameObject.transform.Translate(Vector3.up * Time.deltaTime * -rotSpeed, Space.Self);
        //    }
        //}
        //else if (tooLow) // only listen to upward input
        //{
        //    if ((Input.mousePosition.y >= (mouseOrgY + tolerance))) //up
        //    {
        //        gameObject.transform.Translate(Vector3.up * Time.deltaTime * rotSpeed, Space.Self);
        //    }
        //}
        //else //camera is not too high or too low so listen to both up and down input
        //{
        //    if ((Input.mousePosition.y >= (mouseOrgY + tolerance))) //up
        //    {
        //        gameObject.transform.Translate(Vector3.up * Time.deltaTime * rotSpeed, Space.Self);
        //    }
        //    else if ((Input.mousePosition.y <= (mouseOrgY - tolerance))) //down
        //    {
        //        gameObject.transform.Translate(Vector3.up * Time.deltaTime * -rotSpeed, Space.Self);
        //    }
        //}
        ////========================================================================================================




        ////check to see if camera is no longer too high or too low =======================================
        //targToCamNoY = parentTransform.position - gameObject.transform.position;
        //yCompOrg = targToCamNoY.y;
        //targToCamNoY.y = 0;

        //yAngle = Vector3.Angle(targToCamNoY, parentTransform.position - gameObject.transform.position);
        //if (yAngle <= yAngleTol - 4) //4 is arbitrary
        //{
        //    tooHigh = false;
        //    tooLow = false;
        //}
        ////===============================================================================================

        //gameObject.transform.rotation = Quaternion.LookRotation(parentTransform.position - gameObject.transform.position, Vector3.up);

    } //end update

    void FixedUpdate()
    {
        RaycastHit hitInfo;
        playerToCamera = gameObject.transform.position - parentTransform.position;

        //if a collider is blocking the line of sight of the camera snap the camera in front of the collider ...
        if (Physics.Raycast(parentTransform.position, playerToCamera, out hitInfo, playerToCamera.magnitude))
        {
            gameObject.transform.position = hitInfo.point;
        }
    }
}

