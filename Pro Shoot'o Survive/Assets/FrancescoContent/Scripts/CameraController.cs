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

    [Header("\nCamera variables")]
    [SerializeField] private float cameraMoveSpeedOnRotation;
    [SerializeField] private float lerpCameraPositionSpeed;
    [SerializeField] private float cameraRotationSpeed;
    [SerializeField] private float minCameraOffsetY;
    [SerializeField] private float maxCameraOffsetY;
    [SerializeField] private Vector3 cameraMovePosOffset;
    [SerializeField] private Vector3 cameraAimPosOffset;

    [SerializeField] private Vector3 cameraMoveFixedPosOffsetAsLookRotation;
    [SerializeField] private Vector3 cameraAimFixedPosOffsetAsLookRotation;

    [SerializeField] private float cameraCollisionOffsetMagnitude;
    [SerializeField] private float cameraToPlayerMaxDistanceOnCollision;
    [SerializeField] private float cameraToImpactPointMaxDistance;
    [SerializeField] private LayerMask cameraRaycastLayerMask;

    private bool hasCameraCollided;

    private Vector3 cameraCurrentPosOffsetAsMagnitude;
    private Vector3 cameraCurrentPosOffset;

    private Vector3 cameraCurrentFixedPosOffsetAsLookRotation;


    public float CameraEulerAnglesY
    {
        get
        {
            float cameraEulerAnglesY = transform.eulerAngles.y;

            if (cameraEulerAnglesY > 180)
            {
                cameraEulerAnglesY = 360 - cameraEulerAnglesY;
            }

            return cameraEulerAnglesY;
        }
    }

    public float CameraEulerAnglesY_WithNegativeValue
    {
        get
        {
            float cameraEulerAnglesY = transform.eulerAngles.y;

            if (cameraEulerAnglesY > 180)
            {
                cameraEulerAnglesY -= 360;
            }

            return cameraEulerAnglesY;
        }
    }

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
        cameraCurrentPosOffset = newOffset;
    }

    private Vector3 GetCameraOrbitalMovementOnImpactPoint(Vector3 hitPointOffset)
    {
        float currentRotationY = 0;
        Vector3 newOffset = Vector3.zero;
        Vector2 mouseDeltaDir = PlayerInputsController.GetInputValue<Vector2>("MouseDeltaDir");

        currentRotationY = Mathf.Atan2(hitPointOffset.z, hitPointOffset.x);
        currentRotationY += mouseDeltaDir.x * cameraMoveSpeedOnRotation * 5 * Time.fixedDeltaTime;

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
        currentRotationY += mouseDeltaDir.x * (cameraMoveSpeedOnRotation * 0.5f) * Time.fixedDeltaTime;

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
        float angle = Mathf.Cos(Mathf.Atan(player.transform.position.x + cameraCurrentFixedPosOffsetAsLookRotation.x));
        //float angle = Mathf.Cos(Mathf.Atan2(player.transform.position.x + cameraCurrentFixedPosOffsetAsLookRotation.x, player.transform.position.z + cameraCurrentFixedPosOffsetAsLookRotation.z));
        //float angle = Mathf.Sin(Mathf.Atan(transform.eulerAngles.y));
        Vector3 newOffset = cameraCurrentFixedPosOffsetAsLookRotation * angle;

        newOffset.y = cameraCurrentFixedPosOffsetAsLookRotation.y;
        //newOffset.z = cameraCurrentFixedPosOffsetAsLookRotation.z;

        //cameraCurrentFixedPosOffsetAsLookRotation = newOffset;

        cameraToTarget += cameraCurrentFixedPosOffsetAsLookRotation;
        //cameraToTarget += newOffset;

        //if (PlayerController.IsAiming)
        //{
        //    float currentRotationY = Mathf.Atan2(cameraCurrentPosOffset.z, cameraCurrentPosOffset.x);
        //    Vector3 currentPosOffsetSinAndCos = new Vector3(Mathf.Cos(currentRotationY), 0, Mathf.Sin(currentRotationY));

        //    cameraToTarget += cameraCurrentFixedPosOffsetAsLookRotation * currentRotationY;

        //    //if (CameraEulerAnglesY >= 170 && CameraEulerAnglesY <= 180)
        //    //{
        //    //    cameraToTarget.x *= -1;
        //    //}

        //    //cameraToTarget.x += -currentPosOffsetSinAndCos.x;
        //    //cameraToTarget.x += cameraCurrentFixedPosOffsetAsLookRotation.x + transform.forward.x + transform.right.x;
        //    //cameraToTarget.y += cameraCurrentFixedPosOffsetAsLookRotation.y;


        //    //cameraToTarget.x += currentRotationY * -Math.Sign(transform.forward.z);
        //    //cameraToTarget.x += currentRotationY * -transform.forward.z;
        //}

        //else
        //{
        //    cameraToTarget += cameraCurrentFixedPosOffsetAsLookRotation;
        //}

        // UI_Mngr.Instance.TextSprites["TextInfo"].text = "Camera yaw: " + CameraEulerAnglesY.ToString();
        // UI_Mngr.Instance.TextSprites["TextInfo"].text += "\nCamera yaw with negative value: " + CameraEulerAnglesY_WithNegativeValue.ToString();
        // UI_Mngr.Instance.TextSprites["TextInfo"].text += "\nCamera forward: " + transform.forward.ToString();

        transform.rotation = Quaternion.LookRotation(cameraToTarget.normalized, Vector3.up);

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
        ConcaveCameraMove();
    }

    private void FixedUpdate()
    {
        MoveCameraOrbitally();
    }

    private void LateUpdate()
    {
        MoveCamera();
        RotateCamera();
    }
}
