using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player references")]
    [SerializeField] private GameObject player;
    public PlayerController PlayerController { get { return player.GetComponent<PlayerController>(); } }
    public InputSysController PlayerInputsController { get { return PlayerController.InputsController; } }

    [Header("\nCamera variables")]
    [SerializeField] private float cameraMoveSpeedOnRotation;
    [SerializeField] private float lerpToPLayerPositionAndCameraPosOffsetSpeed;
    [SerializeField] private float cameraRotationSpeed;
    [SerializeField] private float minCameraOffsetY;
    [SerializeField] private float maxCameraOffsetY;
    [SerializeField] private Vector3 cameraMovePosOffset;
    [SerializeField] private Vector3 cameraAimPosOffset;

    [SerializeField] private Vector3 cameraMoveFixedPosOffsetAsLookRotation;
    [SerializeField] private Vector3 cameraAimFixedPosOffsetAsLookRotation;

    [SerializeField] private float cameraCollisionOffsetMagnitude;

    private Vector3 cameraCurrentPosOffsetAsMagnitude;
    private Vector3 cameraCurrentPosOffset;

    private Vector3 cameraCurrentFixedPosOffsetAsLookRotation;

    [Header("\nCrossHair variables")]
    [SerializeField] private RectTransform crossHairTransform;
    [SerializeField] private float crossHairPositionX_Offset;

    private RaycastHit cameraHitInfo;

    // Start is called before the first frame update
    void Start()
    {
        cameraCurrentPosOffset = cameraCurrentPosOffsetAsMagnitude = cameraMovePosOffset;
        cameraCurrentFixedPosOffsetAsLookRotation = cameraMoveFixedPosOffsetAsLookRotation;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void MoveCamera()
    {
        if (IsCameraColliding())
        {
            transform.position = player.transform.position + cameraCurrentPosOffset;

            return;
        }

        transform.position = Vector3.Lerp(transform.position, player.transform.position + cameraCurrentPosOffset, lerpToPLayerPositionAndCameraPosOffsetSpeed * Time.deltaTime);
    }

    public void MoveCameraOrbitally()
    {
        //this movement will happen when we rotate camera yaw (y axis)

        float currentRotationY = 0;
        float cameraEulerAnglesY = 0;
        Vector3 newOffset = Vector3.zero;
        Vector3 crossHairPosition = Vector3.zero;
        Vector2 mouseDeltaDir = PlayerInputsController.GetInputValue<Vector2>("MouseDeltaDir");

        if (IsCameraColliding())
        {
            Vector3 playerToHitPoint = cameraHitInfo.point - player.transform.position;
            playerToHitPoint = (playerToHitPoint.magnitude + cameraCollisionOffsetMagnitude) * playerToHitPoint.normalized;

            currentRotationY = Mathf.Atan2(playerToHitPoint.z, playerToHitPoint.x);
            currentRotationY += mouseDeltaDir.x * (cameraMoveSpeedOnRotation * 0.5f) * Time.deltaTime;

            newOffset = new Vector3(Mathf.Cos(currentRotationY), 0, Mathf.Sin(currentRotationY));
            newOffset *= new Vector3(playerToHitPoint.x, 0, playerToHitPoint.z).magnitude;

            print(cameraHitInfo.transform.name);
        }

        else
        {
            currentRotationY = Mathf.Atan2(cameraCurrentPosOffset.z, cameraCurrentPosOffset.x);
            currentRotationY += mouseDeltaDir.x * (cameraMoveSpeedOnRotation * 0.5f) * Time.deltaTime;

            newOffset = new Vector3(Mathf.Cos(currentRotationY), 0, Mathf.Sin(currentRotationY));
            newOffset *= new Vector3(cameraCurrentPosOffsetAsMagnitude.x, 0, cameraCurrentPosOffsetAsMagnitude.z).magnitude;
        }

        cameraEulerAnglesY = transform.eulerAngles.y;

        if (cameraEulerAnglesY > 180)
        {
            cameraEulerAnglesY = 360 - cameraEulerAnglesY;
        }

        crossHairPosition.x = cameraEulerAnglesY + crossHairPositionX_Offset;
        crossHairTransform.localPosition = crossHairPosition;

        newOffset.y = cameraCurrentPosOffset.y;
        cameraCurrentPosOffset = newOffset;
    }

    public void ConcaveCameraMove()
    {
        //this movement will happen when we rotate camera pitch (x axis)

        float currentRotationX = Mathf.Atan2(cameraCurrentPosOffset.z, cameraCurrentPosOffset.y);
        Vector3 newOffset = new Vector3(0, Mathf.Cos(currentRotationX), Mathf.Sin(currentRotationX));
        Vector2 mouseDeltaDir = PlayerInputsController.GetInputValue<Vector2>("MouseDeltaDir");

        newOffset *= new Vector3(0, cameraCurrentPosOffset.y, cameraCurrentPosOffset.z).magnitude;

        newOffset.y += mouseDeltaDir.y * cameraMoveSpeedOnRotation * Time.deltaTime;
        newOffset.y = Mathf.Clamp(newOffset.y, minCameraOffsetY, maxCameraOffsetY);

        newOffset.x = cameraCurrentPosOffset.x;

        cameraCurrentPosOffset = newOffset;
    }

    public void RotateCamera()
    {
        Vector3 cameraToTarget = player.transform.position - transform.position;
        Quaternion previousRotation = transform.rotation;
        Quaternion lastRotation = Quaternion.identity;

        cameraToTarget += cameraCurrentFixedPosOffsetAsLookRotation;

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

    public void OnPlayerAim()
    {
        if (PlayerController.IsAiming)
        {
            cameraCurrentPosOffsetAsMagnitude = cameraAimPosOffset;
            cameraCurrentFixedPosOffsetAsLookRotation = cameraAimFixedPosOffsetAsLookRotation;

            return;
        }

        cameraCurrentPosOffsetAsMagnitude = cameraMovePosOffset;
        cameraCurrentFixedPosOffsetAsLookRotation = cameraMoveFixedPosOffsetAsLookRotation;
    }

    // Update is called once per frame
    void Update()
    {
        OnPlayerAim();

        MoveCamera();

        MoveCameraOrbitally();
        ConcaveCameraMove();

        RotateCamera();
    }
}
