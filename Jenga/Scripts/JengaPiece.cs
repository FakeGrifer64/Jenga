using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class JengaPiece : MonoBehaviour
{
    [Header("Drag Settings")]
    public float maxDragDistance = 2f;

    [Header("Push Settings")]
    public float pushForce = 5f;
    public float doubleClickForceMultiplier = 2f;
    public float doubleClickThreshold = 0.3f;

    [Header("Scroll Settings")]
    public float scrollMoveSpeed = 3f;
    public float maxScrollDistance = 3f;

    [Header("Game Over Settings")]
    public static bool gamePaused = false;
    private static int piecesOnGround = 0;
    private bool hasCounted = false;

    private Rigidbody rb;
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 startDragPosition;
    private float lastClickTime = -1f;
    private float currentScrollDistance = 0f;

    // Referência para o canvas de game over
    private GameObject gameOverCanvas;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;


    }

    private void OnMouseDown()
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        if (timeSinceLastClick <= doubleClickThreshold)
        {
            ApplyPush(pushForce * doubleClickForceMultiplier);
            return;
        }

        ApplyPush(pushForce);

        isDragging = true;
        rb.isKinematic = true;

        Vector3 mousePos = GetMouseWorldPosition();
        offset = transform.position - mousePos;
        startDragPosition = transform.position;
        currentScrollDistance = 0f;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        // Movimento normal do mouse (X/Y)
        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 targetPos = mousePos + offset;

        Vector3 dragVector = targetPos - startDragPosition;
        if (dragVector.magnitude > maxDragDistance)
        {
            dragVector = dragVector.normalized * maxDragDistance;
        }

        // Movimento do scroll (Z)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentScrollDistance += scroll * scrollMoveSpeed;
        currentScrollDistance = Mathf.Clamp(currentScrollDistance, -maxScrollDistance, maxScrollDistance);

        // Combina ambos movimentos
        transform.position = startDragPosition + dragVector + mainCamera.transform.forward * currentScrollDistance;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = false;
    }

    private void ApplyPush(float force)
    {
        Vector3 direction = (transform.position - mainCamera.transform.position).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCamera.WorldToScreenPoint(startDragPosition).z
        );
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se colidiu com o chão (Plane)
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!hasCounted)
            {
                piecesOnGround++;
                hasCounted = true;
                CheckGameOver();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Se a peça sair do chão, diminui a contagem
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (hasCounted)
            {
                piecesOnGround--;
                hasCounted = false;
            }
        }
    }

    private void CheckGameOver()
    {
        if (piecesOnGround >= 2 && !gamePaused)
        {
            GameManager.GameOver();
            gamePaused = true;
        }
    }




}
