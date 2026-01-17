using UnityEngine;
public class ArmAiming : MonoBehaviour
{
    [Header("Calibration")]
    public float rotationOffset = 0f;
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float fireRate = 0.15f;
    private float nextFireTime;
    void LateUpdate()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -Camera.main.transform.position.z;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float facingDirection = transform.root.localScale.x;
        float finalAngle = angle * facingDirection;
        transform.localRotation = Quaternion.Euler(0, 0, finalAngle + rotationOffset);
        Vector3 localScale = Vector3.one;
        float checkAngle = finalAngle;
        if (checkAngle > 180) checkAngle -= 360;
        if (checkAngle < -180) checkAngle += 360;

        if (checkAngle > 90 || checkAngle < -90)
        {
            localScale.y = -1f;
            localScale.x = -1f;
        }
        else
            localScale.y = 1f;

        transform.localScale = localScale;

        // --- SHOOTING LOGIC ---
        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
    }
}
