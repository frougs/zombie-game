using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;
    private Alteruna.Avatar _avatar;
    private Animator anim;
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
            anim.SetBool("Jumping", true);
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
        else{
            anim.SetBool("Jumping", false);
        }
    }
}
