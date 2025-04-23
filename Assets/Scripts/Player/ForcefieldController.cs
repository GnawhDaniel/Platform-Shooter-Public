using UnityEngine;

public class ForcefieldController : MonoBehaviour
{
    [SerializeField] private Forcefield forcefield; // Reference to the forcefield object
    private bool forcefieldKeyHold = false; // Tracks if forcefield is active
    private bool buttonDown = false; // Tracks if the button is pressed

    // Update is called once per frame
    void Update()
    {
        bool rightMouseButton = Input.GetMouseButton(1);

        // User let go of the right mouse button
        if (forcefield.IsAvailable() && forcefieldKeyHold == true && rightMouseButton == false)
        {
            forcefield.SetLastUsed(Time.time);
        }

        // Handle Forcefield
        if (rightMouseButton)
        {
            forcefieldKeyHold = true;
        }
        else
        {
            forcefieldKeyHold = false;
        }

        // Update forcefield position
        forcefield.transform.position = transform.position;
    }

    private void FixedUpdate()
    {
        // Handle forcefield
        if (forcefieldKeyHold && forcefield.IsAvailable())
        {
            forcefield.EnableForcefield();
            if (buttonDown == false)
            {
                // Deduct set amount of energy
                forcefield.DeductEnergy(15);
            }
            buttonDown = true;
        }
        else
        {
            forcefield.DisableForcefield();
            buttonDown = false;
        }
    }
}
