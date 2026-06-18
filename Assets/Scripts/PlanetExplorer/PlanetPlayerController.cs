using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlanetPlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotateSpeed = 8f;
    
    public WorldLookup worldLookup;

    private CharacterController controller;
    private Camera mainCam;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        mainCam = Camera.main;

        if (mainCam == null)
            Debug.LogError("Main Camera not found! Make sure ExplorerCamera is tagged MainCamera.");

        if (worldLookup == null)
            Debug.LogError("WorldLookup is not assigned!");
    }

    void Update()
    {
        Vector3 gravityUp = transform.position.normalized;

        // Keep player upright
        Quaternion surfaceRotation =
            Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            surfaceRotation,
            rotateSpeed * Time.deltaTime);

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward =
            Vector3.ProjectOnPlane(mainCam.transform.forward, gravityUp).normalized;
        Vector3 camRight =
            Vector3.ProjectOnPlane(mainCam.transform.right, gravityUp).normalized;
        Vector3 moveDir =
            camForward * v +
            camRight * h;
        if (moveDir.sqrMagnitude > 0.01f)
        {
            moveDir.Normalize();

            controller.Move(
                moveDir *
                moveSpeed *
                Time.deltaTime);

            if (moveDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(moveDir, gravityUp);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotateSpeed * Time.deltaTime * 2f);
            }
        }

        // Stick to planet
        Coordinate coord =
            GeoMaths.PointToCoordinate(transform.position.normalized);

        TerrainInfo terrain =
            worldLookup.GetTerrainInfoImmediate(coord);

        float playerHeight =
            controller.height * 0.5f;

        transform.position =
            transform.position.normalized *
            (terrain.height + playerHeight + 0.1f);
    }
}