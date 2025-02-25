using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform camHolder;
    [SerializeField] private Camera cam;
    [SerializeField] private Vector2 mouseSensitivity = new Vector2(100, 100);
    [SerializeField] bool captureMouse = true;

    [SerializeField] private Transform orientation;

    private Vector2 rotationAxis;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (captureMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        camHolder.transform.position = followTarget.position;

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X") * mouseSensitivity.x, Input.GetAxisRaw("Mouse Y") * mouseSensitivity.y) * Time.deltaTime;

        rotationAxis.y += mouseInput.x;
        rotationAxis.x -= mouseInput.y;

        rotationAxis.x = Mathf.Clamp(rotationAxis.x, -90, 90);

        camHolder.rotation = Quaternion.Euler(rotationAxis.x, rotationAxis.y, 0);
        orientation.rotation = Quaternion.Euler(0, rotationAxis.y, 0);

    }

    public void FOVCam(float fov, float amount = 5f)
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, amount * Time.deltaTime);
    }

    public void TiltCam(float angle, float amount = 8f)
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(transform.localRotation.x,
            transform.localRotation.y,
            angle,
            transform.localRotation.w),
            amount * Time.deltaTime);
    }

    public void ZoomCam(float zoom, float amount = 16f)
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, amount * Time.deltaTime);
    }

    public void setSensitivity(Vector2 sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
}
