using UnityEngine;
using UnityEngine.InputSystem;

public class RagdollDragger : MonoBehaviour
{
    [Header("Settings")]
    public float power = 10f;
    public LayerMask ragdollLayer;

    private Rigidbody2D selectedLimb;
    private bool isDragging = false;

    void Update()
    {
        // 1. Check for Mouse Press (New Input System)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryGrabLimb();
        }

        // 2. Check for Release
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
            selectedLimb = null;
        }
    }

    void FixedUpdate()
    {
        if (isDragging && selectedLimb != null)
        {
            // Convert Mouse Position to World Space
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            // Replicating your GDScript logic with the updated property
            Vector2 direction = mouseWorldPos - selectedLimb.position;
            selectedLimb.linearVelocity = direction * power;
        }
    }

    void TryGrabLimb()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // Check for a collider at the mouse position
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, ragdollLayer);

        if (hit != null && hit.attachedRigidbody != null)
        {
            // Make sure the limb is tagged "Player"
            if (hit.CompareTag("Player"))
            {
                selectedLimb = hit.attachedRigidbody;
                isDragging = true;
            }
        }
    }
}