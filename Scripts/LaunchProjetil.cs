using UnityEngine;

public class LaunchProjetil : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform launchPoint;
    public Transform target;
    public float launchForce = 20f;

    public void Launch()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 direction = (target.position - launchPoint.position).normalized;
        rb.AddForce(direction * launchForce, ForceMode.Impulse);

        Destroy(projectile, 5f);
    }
}