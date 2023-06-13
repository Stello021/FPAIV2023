using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player references")]
    [SerializeField] private GameObject player;

    [Header("\nCamera variables")]
    [SerializeField] private float cameraMoveSpeedOnRotation;
    [SerializeField] private float lerpToPLayerPositionAndCameraPosOffsetSpeed;
    [SerializeField] private float cameraRotationSpeed;
    [SerializeField] private float minCameraOffsetY;
    [SerializeField] private float maxCameraOffsetY;
    [SerializeField] private Vector3 cameraStartPosOffset;
    [SerializeField] private Vector3 cameraFixedPosOffset;
    [SerializeField] private Vector3 cameraPosOffset;
    [SerializeField] private float cameraCollisionOffsetMagnitude;

    private Vector3 playerVelocity;
    private RaycastHit cameraHitInfo;


    // Start is called before the first frame update
    void Start()
    {
        //Vector3 newOffset = new Vector3(cameraPosOffset.x, 0, cameraPosOffset.z).magnitude * -player.transform.forward;
        //newOffset.y = cameraPosOffset.y;

        //cameraPosOffset = newOffset;
        cameraPosOffset = cameraStartPosOffset;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + cameraPosOffset, lerpToPLayerPositionAndCameraPosOffsetSpeed * Time.deltaTime);
    }

    public void MoveCameraOrbitally()
    {
        //this movement will happen when we rotate camera yaw (y axis)

        float currentRotationY = 0;
        Vector3 newOffset = Vector3.zero;

        if (IsCameraColliding())
        {
            Vector3 playerToHitPoint = cameraHitInfo.point - player.transform.position;
            playerToHitPoint = (playerToHitPoint.magnitude + cameraCollisionOffsetMagnitude) * playerToHitPoint.normalized;

            currentRotationY = Mathf.Atan2(playerToHitPoint.z, playerToHitPoint.x);
            currentRotationY += InputSysController.Instance.MouseDeltaDir.x * (cameraMoveSpeedOnRotation * 0.5f) * Time.deltaTime;

            newOffset = new Vector3(Mathf.Cos(currentRotationY), 0, Mathf.Sin(currentRotationY));
            newOffset *= new Vector3(playerToHitPoint.x, 0, playerToHitPoint.z).magnitude;

            print(cameraHitInfo.transform.name);
        }

        else
        {
            currentRotationY = Mathf.Atan2(cameraPosOffset.z, cameraPosOffset.x);
            currentRotationY += InputSysController.Instance.MouseDeltaDir.x * (cameraMoveSpeedOnRotation * 0.5f) * Time.deltaTime;

            newOffset = new Vector3(Mathf.Cos(currentRotationY), 0, Mathf.Sin(currentRotationY));
            newOffset *= new Vector3(cameraStartPosOffset.x, 0, cameraStartPosOffset.z).magnitude;
        }

        newOffset.y = cameraPosOffset.y;

        cameraPosOffset = newOffset;
    }

    public void ConcaveCameraMove()
    {
        //this movement will happen when we rotate camera pitch (x axis)

        float currentRotationX = Mathf.Atan2(cameraPosOffset.z, cameraPosOffset.y);
        Vector3 newOffset = new Vector3(0, Mathf.Cos(currentRotationX), Mathf.Sin(currentRotationX));

        newOffset *= new Vector3(0, cameraPosOffset.y, cameraPosOffset.z).magnitude;

        newOffset.y += InputSysController.Instance.MouseDeltaDir.y * cameraMoveSpeedOnRotation * Time.deltaTime;
        newOffset.y = Mathf.Clamp(newOffset.y, minCameraOffsetY, maxCameraOffsetY);

        newOffset.x = cameraPosOffset.x;

        cameraPosOffset = newOffset;
    }

    public void RotateCamera()
    {
        Vector3 cameraToTarget = player.transform.position - transform.position;
        Quaternion previousRotation = transform.rotation;
        Quaternion lastRotation = Quaternion.identity;

        cameraToTarget += cameraFixedPosOffset;

        transform.rotation = Quaternion.LookRotation(cameraToTarget, Vector3.up);

        lastRotation = transform.rotation;
        transform.rotation = previousRotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, lastRotation, cameraRotationSpeed * Time.deltaTime);
    }

    public bool IsCameraColliding()
    {
        RaycastHit hitInfo;
        Vector3 playerToCamera = transform.position - player.transform.position;

        //if a collider is blocking the line of sight of the camera snap the camera in front of the collider ...
        if (Physics.Raycast(player.transform.position, playerToCamera, out hitInfo, playerToCamera.magnitude))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                return false;
            }

            cameraHitInfo = hitInfo;

            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();

        MoveCameraOrbitally();
        ConcaveCameraMove();

        RotateCamera();
    }
}
