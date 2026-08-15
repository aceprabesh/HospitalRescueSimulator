using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform doorPanel;

    public float openDistance = 1.8f;
    public float openSpeed = 3f;
    public float triggerDistance = 2.5f;

    private Transform player;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (doorPanel != null)
        {
            closedPosition = doorPanel.localPosition;

            openPosition =
                closedPosition +
                new Vector3(openDistance, 0f, 0f);
        }
    }

    private void Update()
    {
        if (player == null || doorPanel == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        Vector3 targetPosition;

        if (distance <= triggerDistance)
        {
            targetPosition = openPosition;
        }
        else
        {
            targetPosition = closedPosition;
        }

        doorPanel.localPosition =
            Vector3.Lerp(
                doorPanel.localPosition,
                targetPosition,
                Time.deltaTime * openSpeed
            );
    }
}