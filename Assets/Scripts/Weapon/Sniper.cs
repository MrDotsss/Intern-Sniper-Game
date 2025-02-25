using UnityEngine;
using UnityEngine.InputSystem;

public class Sniper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask hurtbox;
    [SerializeField] private Transform orientation;
    [SerializeField] private PlayerInput input;
    [SerializeField] private FPSCamera cam;
    [SerializeField] private GameObject sphereHit;
    [Header("Properties")]
    [SerializeField] private float range = 100f;

    private RaycastHit bulletHit;

    private void Update()
    {
        if (input.actions["fire"].WasPressedThisFrame())
        {
            Shoot();
        }
        else if (input.actions["aim"].IsPressed())
        {
            cam.ZoomCam(10, 32);
            DoSlowMo(0.1f);
        }
        else if (!input.actions["aim"].IsPressed())
        {
            cam.ZoomCam(70, 32);
            DoSlowMo(1f);
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(orientation.transform.position, orientation.transform.forward, out bulletHit, range, hurtbox))
        {
            if (bulletHit.collider.TryGetComponent(out Hurtbox hurt))
            {
                if (hurt.owner.TryGetComponent(out IDamageable damage))
                {
                    damage.TakeDamage(10);
                }
            }
            ShowHitMark();
        }
    }

    private void DoSlowMo(float scale, float amount = 8f)
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, scale, amount * Time.deltaTime);
    }

    private void ShowHitMark()
    {
        GameObject mark = Instantiate(sphereHit);
        mark.transform.position = bulletHit.point;
        mark.transform.rotation = Quaternion.identity;
        Destroy(mark, 5f);
    }
}
