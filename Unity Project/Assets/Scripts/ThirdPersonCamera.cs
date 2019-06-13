using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffSet;
    [SerializeField] float damping;
    Transform cameraLookTarget;
    Player localPlayer;
    public static ThirdPersonCamera Instance;
    Vector3 PosDebug;
    public Vector3 PlayerRotation;
    /* CODE BY ATEF */
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float minimumY = -70F;
    private float maximumY = 10.0F;
    /* */
    // Start is called before the first frame update
    void Awake()
    {
        //transform.localPosition = new Vector3(0.12f, 2.54f, -2.33f);
        //Gm.Instance.OnLocalPlayerJoined += HandleLocalPlayerJoined;
        localPlayer = transform.parent.gameObject.GetComponent<Player>();
        cameraLookTarget = localPlayer.transform.Find("CameraLookTarget");
        if (cameraLookTarget == null)
            cameraLookTarget = localPlayer.transform;
    }

    private void Start() {
        Instance = this;
    }

    /*void HandleLocalPlayerJoined(Player player)
    {
        localPlayer = player;
        cameraLookTarget = localPlayer.transform.Find("CameraLookTarget");
        if (cameraLookTarget == null)
            cameraLookTarget = localPlayer.transform;

    }*/
    // Update is called once per frame
    void Update()
    {
        //transform.position = PosDebug;
        /*Vector3 targetPosition = cameraLookTarget.position + localPlayer.transform.forward * cameraOffSet.z +
            localPlayer.transform.up * cameraOffSet.y +
            localPlayer.transform.right * cameraOffSet.x;*/
        //code khedmou atef
        /*calculY -= 2.0f * Input.GetAxis("Mouse Y");
        calculY = Mathf.Clamp(calculY, -60.0f,90.0f);
        transform.eulerAngles = new Vector3(calculY, 0.0f, 0.0f);*/
        //end code khedmou atef
        //Quaternion targetRotation = Quaternion.LookRotation(cameraLookTarget.position - targetPosition, Vector3.up);

        //transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, damping * Time.deltaTime);
        yaw += damping * Input.GetAxis("Mouse X");
        pitch -= damping * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, minimumY, maximumY);
        transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y, transform.eulerAngles.z);
        localPlayer.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        PlayerRotation = localPlayer.transform.eulerAngles;
    }
}
