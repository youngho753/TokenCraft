using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _mouseTransform;

    Vector2 _center;
    Vector2 _mapSize;
    
    float zIndex = -7f;

    public float Height { get; set; } = 0;
    public float Width { get; set; } = 0;

    void Start()
    {
        SetCameraSize();
    }
    void SetCameraSize()
    {
        Camera.main.orthographicSize = 8f;
        Height = Camera.main.orthographicSize;
        Width = Height * Screen.width / Screen.height;
        OnDrawGizmos();
    }
    void LateUpdate()
    {
        if(_mouseTransform != null)
            transform.position = new Vector3(_mouseTransform.position.x, _mouseTransform.position.y, -zIndex);
    }

    void LimitCameraArea()
    {
        // transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -10f);
        // //
        // // float limitX = Managers.Game.MapSize.x * 0.5f - Width;
        // // float clampX = Mathf.Clamp(transform.position.x, -limitX, limitX);
        // float clampX = Mathf.Clamp(transform.position.x, -0, 0);
        //
        // // float limitY = Managers.Game.CurrentMap.MapSize.y * 0.5f - Height;
        // // float clampY = Mathf.Clamp(transform.position.y, -limitY, limitY);
        // float clampY = Mathf.Clamp(transform.position.y, -0, -0);
        //
        // transform.position = new Vector3(clampX, clampY, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(_center, _mapSize.x);
        //Gizmos.DrawWireCube(_center, _mapSize * 2);
    }
}