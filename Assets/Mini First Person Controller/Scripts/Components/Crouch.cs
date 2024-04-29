using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Crouch : MonoBehaviour
{
    //public KeyCode key = KeyCode.LeftControl;
    [Header("Crouch Speed")]
    public float crouchSpeed;
    [Header("Slow Movement")]
    [Tooltip("Movement to slow down when crouched.")]
    public FirstPersonMovement movement;
    [Tooltip("Movement speed when crouched.")]
    public float movementSpeed = 2;

    [Header("Low Head")]
    [Tooltip("Head to lower when crouched.")]
    public Transform headToLower;
    [HideInInspector]
    public float? defaultHeadYLocalPosition;
    public float crouchYHeadPosition = 1;
    
    [Tooltip("Collider to lower when crouched.")]
    public CapsuleCollider colliderToLower;
    [HideInInspector]
    public float? defaultColliderHeight;

    public bool IsCrouched { get; private set; }
    public event System.Action CrouchStart, CrouchEnd;
    private bool startHeadReset;
    private Alteruna.Avatar _avatar;

    private void Start(){
        _avatar = GetComponent<Alteruna.Avatar>();
        if(!_avatar.IsMe){
            return;
        }
        var cam = CameraSingleton.instance.gameObject.GetComponent<CinemachineVirtualCamera>();
        headToLower = cam.transform;
    }
    void Reset()
    {
        if(!_avatar.IsMe){
            return;
        }
        // Try to get components.
        movement = GetComponentInParent<FirstPersonMovement>();
        headToLower = movement.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.transform;
        colliderToLower = movement.GetComponentInChildren<CapsuleCollider>();
    }
    void LateUpdate()
    {
        if(!_avatar.IsMe){
            return;
        }
        if (UserInputs.instance.crouch.IsPressed())
        {
            startHeadReset = false;
            // Enforce a low head.
            if (headToLower)
            {
                // If we don't have the defaultHeadYLocalPosition, get it now.
                if (!defaultHeadYLocalPosition.HasValue)
                {
                    defaultHeadYLocalPosition = headToLower.localPosition.y;
                }

                // Lower the head.
                Vector3 headEndPOS = new Vector3(headToLower.localPosition.x, crouchYHeadPosition, headToLower.localPosition.z);
                headToLower.localPosition = Vector3.MoveTowards(headToLower.localPosition, headEndPOS, crouchSpeed * Time.deltaTime);
            }

            // Enforce a low colliderToLower.
            if (colliderToLower)
            {
                // If we don't have the defaultColliderHeight, get it now.
                if (!defaultColliderHeight.HasValue)
                {
                    defaultColliderHeight = colliderToLower.height;
                }

                // Get lowering amount.
                float loweringAmount;
                if(defaultHeadYLocalPosition.HasValue)
                {
                    loweringAmount = defaultHeadYLocalPosition.Value - crouchYHeadPosition;
                }
                else
                {
                    loweringAmount = defaultColliderHeight.Value * .5f;
                }

                // Lower the colliderToLower.
                colliderToLower.height = Mathf.Max(defaultColliderHeight.Value - loweringAmount, 0);
                colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
            }

            // Set IsCrouched state.
            if (!IsCrouched)
            {
                IsCrouched = true;
                SetSpeedOverrideActive(true);
                CrouchStart?.Invoke();
            }
        }
        else
        {
            if (IsCrouched)
            {
                startHeadReset = true;
            }
        }
        if(startHeadReset){
            // Rise the head back up.
                if (headToLower)
                {
                    Vector3 headEndPOS = new Vector3(headToLower.localPosition.x, defaultHeadYLocalPosition.Value, headToLower.localPosition.z);
                    headToLower.localPosition = Vector3.MoveTowards(headToLower.localPosition, headEndPOS, crouchSpeed * Time.deltaTime);
                    if(headEndPOS == headToLower.localPosition){
                        IsCrouched = false;
                        SetSpeedOverrideActive(false);
                        CrouchEnd?.Invoke();
                        startHeadReset = false;
                    }
                }

                // Reset the colliderToLower's height.
                if (colliderToLower)
                {
                    colliderToLower.height = defaultColliderHeight.Value;
                    colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
                }

        }
    }


    #region Speed override.
    void SetSpeedOverrideActive(bool state)
    {
        if(!_avatar.IsMe){
            return;
        }
        // Stop if there is no movement component.
        if(!movement)
        {
            return;
        }

        // Update SpeedOverride.
        if (state)
        {
            // Try to add the SpeedOverride to the movement component.
            if (!movement.speedOverrides.Contains(SpeedOverride))
            {
                movement.speedOverrides.Add(SpeedOverride);
            }
        }
        else
        {
            // Try to remove the SpeedOverride from the movement component.
            if (movement.speedOverrides.Contains(SpeedOverride))
            {
                movement.speedOverrides.Remove(SpeedOverride);
            }
        }
    }

    float SpeedOverride() => movementSpeed;
    #endregion
}
