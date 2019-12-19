using Library.Extensions;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Core.Controller
{
    public class CameraController : MonoBehaviour
    {
        private const float PositionMultiplierByY = 0.1f;
        private const float SizeMultiplierForWidth = 0.46f;
        private const float SizeMultiplierForHeight = 0.6f;

        public Camera Camera => _camera;
        
        [SerializeField] private Camera _camera;

        private void Start()
        {
            SignalBus.Subscribe<GameAreaCreatedSignal>(OnGameAreaCreated);
        }

        private void OnDestroy()
        {
            SignalBus.Unsubscribe<GameAreaCreatedSignal>(OnGameAreaCreated);
        }

        private float GetOrthographicSize(float width, float height)
        {
            return Mathf.Max(width * SizeMultiplierForWidth, height * SizeMultiplierForHeight);
        }

        #region Signal Listeners

        private void OnGameAreaCreated(GameAreaCreatedSignal gameAreaCreatedSignal)
        {
            _camera.orthographicSize = GetOrthographicSize(gameAreaCreatedSignal.GameArea.Size.x,
                gameAreaCreatedSignal.GameArea.Size.y);
            
            _camera.transform.SetPosY(_camera.orthographicSize * PositionMultiplierByY);
        }

        #endregion        
    }
}