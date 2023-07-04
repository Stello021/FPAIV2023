using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player references")]
    [SerializeField] private GameObject player;
    public PlayerController PlayerController { get { return player.GetComponent<PlayerController>(); } }
    public InputSysController PlayerInputsController { get { return PlayerController.InputsController; } }

    [Header("\nCamera speed variables")]
    [SerializeField] private float cameraMoveSpeedOnAim;
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private float lerpCameraPositionSpeed;
    [SerializeField] private float cameraRotationSpeed;
    private float currentCameraMoveSpeed;

    [Header("\nCamera position offset variables")]
    [SerializeField] private float minCameraOffsetY;
    [SerializeField] private float maxCameraOffsetY;
    [SerializeField] private Vector3 cameraMovePosOffsetAsMagnitude;
    [SerializeField] private Vector3 cameraAimPosOffsetAsMagnitude;
    private Vector3 cameraCurrentPosOffsetAsMagnitude;
    private Vector3 cameraCurrentPosOffset;

    [Header("\nCamera look rotation variables")]
    [SerializeField] private Vector3 cameraMoveFixedPosOffsetAsLookRotation;
    [SerializeField] private Vector3 cameraAimFixedPosOffsetAsLookRotation;
    [SerializeField] private float lookRotationAngle;
    private Vector3 cameraCurrentFixedPosOffsetAsLookRotation;

    [Header("\nCamera collision variables")]
    [SerializeField] private float cameraCollisionOffsetMagnitude;
    [SerializeField] private float cameraToPlayerMaxDistanceOnCollision;
    [SerializeField] private float cameraToImpactPointMaxDistance;
    [SerializeField] private LayerMask cameraRaycastLayerMask;
    private bool hasCameraCollided;
    private RaycastHit cameraHitInfo;


    // Start is called before the first frame update
    void Start()
    {
        cameraCurrentPosOffset = cameraCurrentPosOffsetAsMagnitude = cameraMovePosOffsetAsMagnitude;
        cameraCurrentFixedPosOffsetAsLookRotation = cameraMoveFixedPosOffsetAsLookRotation;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (PlayerPrefs.GetFloat("Sensitivity") != 0)
        {
            cameraMoveSpeed = PlayerPrefs.GetFloat("Sensitivity");
        }

        currentCameraMoveSpeed = cameraMoveSpeed;
        cameraMoveSpeedOnAim = cameraMoveSpeed * 0.5f;
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + cameraCurrentPosOffset, lerpCameraPositionSpeed * Time.deltaTime);
    }

    public void MoveCameraOrbitally()
    {
        //this movement will happen when we rotate camera yaw (y axis)

        Vector3 cameraToPlayer = player.transform.position - transform.position;
        Vector3 newOffset = Vector3.zero;
        Vector3 playerToHitPoint = cameraHitInfo.point - player.transform.position;

        playerToHitPoint = (playerToHitPoint.magnitude + cameraCollisionOffsetMagnitude) * playerToHitPoint.normalized;

        if (IsCameraColliding() && cameraToPlayer.sqrMagnitude <= cameraToPlayerMaxDistanceOnCollision)
        {
            newOffset = GetCameraOrbitalMovementOnImpactPoint(playerToHitPoint);
            hasCameraCollided = true;
        }

        else
        {
            float cameraPosOffsetToHitPointDist = (playerToHitPoint - cameraCurrentPosOffset).magnitude;

            if (cameraPosOffsetToHitPointDist <= cameraToImpactPointMaxDistance && hasCameraCollided && cameraToPlayer.sqrMagnitude <= cameraToPlayerMaxDistanceOnCollision)
            {
                newOffset = GetCameraOrbitalMovementOnImpactPoint(playerToHitPoint);

                Vector3 currentPosOffset = cameraCurrentPosOffset;
                Vector3 newPosOffset = newOffset;
                Vector2 mouseDeltaDir = PlayerInputsController.GetInputValue<Vector2>("MouseDeltaDir");

                currentPosOffset.y = 0;
                newPosOffset.y = 0;

                float posOffsetDeltaDist = Vector3.Distance(currentPosOffset, newPosOffset);

                if (posOffsetDeltaDist <= 4 && mouseDeltaDir.x != 0)
                {
                    newOffset = GetCameraOrbitalMovementOnCurrentPosOffset();
                    hasCameraCollided = false;
                }
            }

            else
            {
                newOffset = GetCameraOrbitalMovementOnCurrentPosOffset();
                hasCameraCollided = false;
            }
        }

        newOffset.y = cameraCurrentPosOffset.y;
        cameraCurrentPosOffset = Vector3.Lerp(cameraCurrentPosOffset, newOffset, lerpCameraPositionSpeed * Time.fixedDeltaTime);
    }

    private Vector3 GetCameraOrbitalMovementOnImpactPoint(Vector3 hitPointOffset)
    {
        float currentRotationY = 0;
        Vector3 newOffset = Vector3.zero;
        Vector2 mouseDeltaDir = PlayerInputsController.GetInputValue<Vector2>("MouseDeltaDir");

        currentRotationY = Mathf.Atan2(hitPointOffset.z, hitPointOffset.x);
        currentRotationY += mouseDeltaDir.x * currentCameraMoveSpeed * Time.fixedDeltaTime;

        newOffset = new Vector3(Mathf.Cos(currentRotationY), 0, Mathf.Sin(currentRotationY));
        newOffset *= new Vector3(hitPointOffset.x, 0, hitPointOffset.z).magnitude;

        //print("Is colliding");

        return newOffset;
    }

    private Vector3 GetCameraOrbitalMovementOnCurrentPosOffset()
    {
        float currentRotationY = 0;
        Vector3 newOffset = Vector3.zero;
        Vector2 mouseDeltaDir = PlayerInputsController.GetInputValue<Vector2>("MouseDeltaDir");

        currentRotationY = Mathf.Atan2(cameraCurrentPosOffset.z, cameraCurrentPosOffset.x);
        currentRotationY += mouseDeltaDir.x * currentCameraMoveSpeed * Time.fixedDeltaTime;

        newOffset = new Vector3(Mathf.Cos(currentRotationY), 0, Mathf.Sin(currentRotationY));
        newOffset *= new Vector3(cameraCurrentPosOffsetAsMagnitude.x, 0, cameraCurrentPosOffsetAsMagnitude.z).magnitude;

        //print("Is not colliding");

        return newOffset;
    }

    public void ConcaveCameraMove()
    {
        //this movement will happen when we rotate camera pitch (x axis)

        float currentRotationX = Mathf.Atan2(cameraCurrentPosOffset.z, cameraCurrentPosOffset.y);
        Vector3 newOffset = new Vector3(0, Mathf.Cos(currentRotationX), Mathf.Sin(currentRotationX));
        Vector2 mouseDeltaDir = PlayerInputsController.GetInputValue<Vector2>("MouseDeltaDir");

        newOffset *= new Vector3(0, cameraCurrentPosOffset.y, cameraCurrentPosOffset.z).magnitude;

        newOffset.y += mouseDeltaDir.y * (currentCameraMoveSpeed * 2.5f) * Time.fixedDeltaTime;
        newOffset.y = Mathf.Clamp(newOffset.y, minCameraOffsetY, maxCameraOffsetY);

        newOffset.x = cameraCurrentPosOffset.x;

        cameraCurrentPosOffset = Vector3.Lerp(cameraCurrentPosOffset, newOffset, lerpCameraPositionSpeed * Time.fixedDeltaTime);
    }

    public void RotateCamera()
    {
        Vector3 cameraToTarget = player.transform.position - transform.position;
        Vector3 newFixedPosOffset = cameraCurrentFixedPosOffsetAsLookRotation;

        Quaternion previousRotation = transform.rotation;
        Quaternion lastRotation = Quaternion.identity;
        float currentRotationY = Mathf.Atan2(cameraCurrentPosOffset.z, cameraCurrentPosOffset.x) + (lookRotationAngle * Mathf.Deg2Rad);
        Vector3 currentPosOffsetSinAndCos = new Vector3(Mathf.Cos(currentRotationY), 0, Mathf.Sin(currentRotationY));

        newFixedPosOffset.x *= currentPosOffsetSinAndCos.x;
        newFixedPosOffset.z *= currentPosOffsetSinAndCos.z;

        cameraToTarget += newFixedPosOffset;

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
        if (Physics.Raycast(player.transform.position, playerToCamera, out hitInfo, playerToCamera.magnitude, cameraRaycastLayerMask))
        {
            cameraHitInfo = hitInfo;

            return true;
        }

        return false;
    }

    public void ToggleCameraOffsets()
    {
        if (PlayerController.IsAiming)
        {
            cameraCurrentPosOffsetAsMagnitude = cameraAimPosOffsetAsMagnitude;
            cameraCurrentFixedPosOffsetAsLookRotation = cameraAimFixedPosOffsetAsLookRotation;
            currentCameraMoveSpeed = cameraMoveSpeedOnAim;

            return;
        }

        cameraCurrentPosOffsetAsMagnitude = cameraMovePosOffsetAsMagnitude;
        cameraCurrentFixedPosOffsetAsLookRotation = cameraMoveFixedPosOffsetAsLookRotation;
        currentCameraMoveSpeed = cameraMoveSpeed;
    }

    private void FixedUpdate()
    {
        try
        {
            MoveCameraOrbitally();
            ConcaveCameraMove();
        }

        catch
        {
        }
    }

    private void LateUpdate()
    {
        try
        {
            MoveCamera();
            RotateCamera();
        }

        catch
        {
        }
    }
}
