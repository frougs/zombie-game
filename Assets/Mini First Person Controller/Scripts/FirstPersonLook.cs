using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;
    private CinemachineVirtualCamera cam;
    //private Alteruna.Avatar _avatar;
    public bool canLook;


    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        //character = GetComponentInParent<FirstPersonMovement>().transform;
        AssignCharacter();
    }

    void Start()
    {
        //_avatar = GetComponentInParent<Alteruna.Avatar>();
        //character = transform.parent;
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
        cam = CameraSingleton.instance.gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if(character == null){
            AssignCharacter();
        }
        if(canLook){
            // Get smooth velocity.
            Vector2 mouseDelta = UserInputs.instance.look.ReadValue<Vector2>();
            //Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            // Rotate camera up-down and controller left-right from velocity.
            cam.transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
            
            character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        }
    }
    public void AssignCharacter(){
        var characters = FindObjectsOfType<FirstPersonMovement>();
        foreach(FirstPersonMovement chara in characters){
            if(chara.GetComponent<Alteruna.Avatar>().IsMe){
                //Debug.Log("Iz me");
                character = chara.transform;
                var spawnPoint = chara.transform.Find("CameraSpawnPoint");
                CameraSingleton.instance.transform.position = spawnPoint.position;
                CameraSingleton.instance.transform.parent = spawnPoint;

            }
        }
        //character = GetComponentInParent<FirstPersonMovement>().transform;
    }
    public void ToggleLook(bool status){
        canLook = status;
    }
}
