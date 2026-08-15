using UnityEngine;

public class DoubleSlidingDoor : MonoBehaviour
{
    [Header("Door Panels")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Settings")]
    public float openDistance = 2.2f;
    public float openSpeed = 3f;
    public float triggerDistance = 4f;

    private Vector3 leftClosedPosition;
    private Vector3 rightClosedPosition;

    private Vector3 leftOpenPosition;
    private Vector3 rightOpenPosition;

    private Transform player;

    void Start()
    {
        leftClosedPosition = leftDoor.localPosition;
        rightClosedPosition = rightDoor.localPosition;

        leftOpenPosition =
            leftClosedPosition + Vector3.left * openDistance;

        rightOpenPosition =
            rightClosedPosition + Vector3.right * openDistance;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        bool shouldOpen = distance <= triggerDistance;

        Vector3 leftTarget =
            shouldOpen ? leftOpenPosition : leftClosedPosition;

        Vector3 rightTarget =
            shouldOpen ? rightOpenPosition : rightClosedPosition;

        leftDoor.localPosition = Vector3.MoveTowards(
            leftDoor.localPosition,
            leftTarget,
            openSpeed * Time.deltaTime
        );

        rightDoor.localPosition = Vector3.MoveTowards(
            rightDoor.localPosition,
            rightTarget,
            openSpeed * Time.deltaTime
        );
    }
}