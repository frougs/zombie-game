using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Jump : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;
    public UnityEvent OnJump;
    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;
    private Alteruna.Avatar _avatar;
    private Animator anim;
    public bool jumping;
    private void Start(){
        anim = GetComponentInChildren<Animator>();
        _avatar = GetComponent<Alteruna.Avatar>();
    }
    void Reset()
    {
                if(!_avatar.IsMe){
            return;
        }
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rigidbody.
        rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
                if(!_avatar.IsMe){
            return;
        }
        // Jump when the Jump button is pressed and we are on the ground.
        if (UserInputs.instance.jump.triggered && (!groundCheck || groundCheck.isGrounded))
        {
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
            OnJump?.Invoke();
        }
        if(groundCheck.isGrounded){
            jumping = false;
            anim.SetBool("Jumping", false);
        }
        else{
            anim.SetBool("Jumping", true);
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
        }
    }
    public void JumpAnim(){
        anim.SetBool("Jumping", true);
    }
}
