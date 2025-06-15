using UnityEngine;

public class CameraScalar : MonoBehaviour
{
    private Board board;
    public float cameraOffset;
    public float aspectRatio = 0.625f;
    public float padding = 2;
    public float yOffset = 1;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return;
        }
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        if (board != null)
        {
            RepositionCamera(board.width - 1, board.height - 1);
        }
    }

    void RepositionCamera(float width, float height)
    {
        Vector3 tempPosition = new Vector3(width / 2, height / 2 + yOffset, cameraOffset);
        transform.position = tempPosition;

        if (width >= height)
        {
            mainCamera.orthographicSize = (width / 2 + padding) / aspectRatio;
        }
        else
        {
            mainCamera.orthographicSize = (height / 2 + padding);
        }
    }
}
