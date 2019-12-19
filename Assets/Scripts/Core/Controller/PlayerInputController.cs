using Library.ServiceLocatorSystem;
using UnityEngine;

namespace Core.Controller
{
    public class PlayerInputController : MonoBehaviour
    {
        private CameraController _cameraController;
        private BrickSpawner _brickSpawner;
        private BallLauncher _ballLauncher;
        
        private void Start()
        {
            _cameraController = ServiceLocator.Get<CameraController>();
            _brickSpawner = ServiceLocator.Get<BrickSpawner>();
            _ballLauncher = ServiceLocator.Get<BallLauncher>();
        }
        
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 worldPositionOfClick = _cameraController.Camera.ScreenToWorldPoint(Input.mousePosition);

                for (int i = 0; i < _brickSpawner.ActiveBricks.Count; i++)
                {
                    if (_brickSpawner.ActiveBricks[i].IsContainsPoint(worldPositionOfClick))
                    {
                        _brickSpawner.ActiveBricks[i].OnCustomCollisionEnter(_ballLauncher.CurrentBall.Rigidbody.Shape);
                        break;
                    }
                }
            }
        }
    }
}