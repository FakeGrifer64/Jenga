using System.Collections;
using UnityEngine;

public class AnimationToagdoll : MonoBehaviour
{
    [SerializeField] Collider myCollider;
   // [SerializeField] float respawnTime = 30f;
    [SerializeField] private LaunchProjetil projectileLauncher;
    Rigidbody[] rigidbodies;
    Animator animator;
    bool bIsRagdoll = false;

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        ToggleRagdoll(true);
        StartCoroutine(LaunchInitialProjectile());
    }

    IEnumerator LaunchInitialProjectile()
    {
        yield return new WaitForSeconds(2f);
        projectileLauncher.Launch();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !bIsRagdoll)
        {
            Debug.Log("Colidiu com projétil");
            ToggleRagdoll(false);
            StartCoroutine(GetBackUp());
        }
    }

    private IEnumerator GetBackUp()
    {
        yield return new WaitForSeconds(5f);

        ToggleRagdoll(true);
        animator.SetTrigger("GetUp");

        yield return new WaitForSeconds(6f);
        animator.SetTrigger("Dance");

        yield return new WaitForSeconds(3f);
        animator.SetTrigger("Run");

        yield return new WaitForSeconds(3f);

        projectileLauncher.Launch();
    }

    private void ToggleRagdoll(bool bIsAnimating)
    {
        bIsRagdoll = !bIsAnimating;
        myCollider.enabled = bIsAnimating;

        foreach (Rigidbody ragdollBone in rigidbodies)
        {
            ragdollBone.isKinematic = bIsAnimating;
        }

        animator.enabled = bIsAnimating;

        if (bIsAnimating)
        {
            RandomAnimation();
        }
    }

    void RandomAnimation()
    {
        int randomNum = Random.Range(0, 2);
        if (randomNum == 0)
            animator.SetTrigger("Walk");
        else
            animator.SetTrigger("Idle");
    }
}
