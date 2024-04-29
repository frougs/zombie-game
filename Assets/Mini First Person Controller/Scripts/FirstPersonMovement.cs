using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public float runTransitionSpeed;
    private float internalRunFOV;
    public float defaultFOV;
    private CinemachineVirtualCamera camera;
    private Alteruna.Avatar _avatar;
    private Animator anim;


    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();


    private void Start(){
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Idle", true);
        _avatar = GetComponent<Alteruna.Avatar>();
        if(!_avatar.IsMe){
            return;
        }
        camera = CameraSingleton.instance.gameObject.GetComponent<CinemachineVirtualCamera>();
        defaultFOV = camera.m_Lens.FieldOfView;
    }
    void Awake()
    {

        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        if(!_avatar.IsMe){
            return;
        }
        // Update IsRunning from input.
        IsRunning = canRun && UserInputs.instance.sprint.IsPressed();

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 moveInput = UserInputs.instance.move.ReadValue<Vector2>();
        Vector2 targetVelocity =new Vector2( moveInput.x * targetMovingSpeed, moveInput.y * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
        if(rigidbody.velocity != Vector3.zero){
            anim.SetBool("Walking", true);
            anim.SetBool("Idle", false);
            var speedChange = camera.m_Lens.FieldOfView + rigidbody.velocity.magnitude;
            speedChange = Mathf.Clamp(speedChange, defaultFOV, defaultFOV + rigidbody.velocity.magnitude);

            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, speedChange, (runTransitionSpeed + rigidbody.velocity.magnitude) * Time.deltaTime);
        }
        else{
            anim.SetBool("Walking", false);
            anim.SetBool("Idle", true);
            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, defaultFOV, runTransitionSpeed * Time.deltaTime);
        }
        if(IsRunning){
            anim.SetBool("Running", true);
            anim.SetBool("Idle", false);
        }
        else{
            anim.SetBool("Running", false);
        }
    }
}