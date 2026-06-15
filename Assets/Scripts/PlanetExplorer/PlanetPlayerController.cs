using UnityEngine;

public class PlanetPlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float planetRadius = 150f;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 gravityUp = transform.position.normalized;

        transform.rotation =
            Quaternion.FromToRotation(
                transform.up,
                gravityUp
            ) * transform.rotation;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move =
            transform.forward * v +
            transform.right * h;

        controller.Move(
            move.normalized *
            moveSpeed *
            Time.deltaTime);

        transform.position =
            transform.position.normalized *
            planetRadius;
    }
}