using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class Player : MonoBehaviour
{


    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;
    }

    [SerializeField] float speed;
    [SerializeField] MouseInput MouseControl;

    private Crosshair m_crosshair;
        private Crosshair Crosshair
    {
        get
        {
            if (m_crosshair != null)
                m_crosshair = GetComponentInChildren<Crosshair>();
            return m_crosshair;
        }
    }


    private MoveController m_MoveController;
    public MoveController MoveController
    {
        get
        {
            if (m_MoveController == null)
            {
                m_MoveController = gameObject.GetComponent<MoveController>();
            }
            return m_MoveController;
        }
    }
    InputController PlayerInput;
    Vector2 mouseInput;
    Rigidbody rigid;
    public float JumpForce = 300;
    bool jumped = false;
    public float rejump_time = 1.5f;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerInput = Gm.Instance.InputController;
        m_crosshair = GetComponentInChildren<Crosshair>();
        Gm.Instance.LocalPlayer = this;
    }

    private void Start() {
        rigid = transform.GetComponent<Rigidbody>();
        Network.Instance.Player = this.gameObject;
    }


    IEnumerator jumpAgain() {
        yield return new WaitForSeconds(rejump_time);
        jumped = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(PlayerInput.Vertical * speed, PlayerInput.Horizontal * speed);
        MoveController.Move(direction);

        /*mouseInput.x = Mathf.Lerp(mouseInput.x, PlayerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, PlayerInput.MouseInput.y, 1f / MouseControl.Damping.y);
        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);*/
        Crosshair.LookHeight(mouseInput.y * MouseControl.Sensitivity.y);
        if (Input.GetKeyDown(KeyCode.Space) && !jumped) {
            rigid.AddForce(0, JumpForce, 0);
            jumped = true;
            StartCoroutine(jumpAgain());
        } else if (jumped) { 
            NetworkControl.Instance.UpdateMovement(transform.position);
        }
    }

}
