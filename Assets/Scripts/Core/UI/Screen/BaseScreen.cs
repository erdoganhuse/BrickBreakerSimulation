using System;
using System.Collections;
using System.Collections.Generic;
using Library.CoroutineSystem;
using UnityEngine;

namespace Core.UI.Screen
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseScreen : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup; 
            }
        }

        private Action _onHided;
        
        public void Show()
        {
            gameObject.SetActive(true);
            CanvasGroup.interactable = false;
            CoroutineManager.StartChildCoroutine(AnimateIn(() =>
            {                
                CanvasGroup.interactable = true;
            }));
        }

        public void Hide()
        {
            CanvasGroup.interactable = false;
            CoroutineManager.StartChildCoroutine(AnimateOut(() =>
            {                
                CanvasGroup.interactable = true;
                gameObject.SetActive(false);
                
                _onHided?.Invoke();
                _onHided = null;
            })); 
        }

        public void HideListener(Action onHided = null)
        {
            _onHided += onHided;
        }
        
        private IEnumerator AnimateIn(Action onCompleted = null)
        {
            CanvasGroup.alpha = 0f;
            
            float time = Time.unscaledTime;
            while (Time.unscaledTime - time < 0.5f)
            {
                CanvasGroup.alpha = (Time.unscaledTime - time) / 0.5f;

                yield return new WaitForSecondsRealtime(0.05f);
            }
            
            CanvasGroup.alpha = 1f;
            onCompleted?.Invoke();
        }

        private IEnumerator AnimateOut(Action onCompleted = null)
        {
            float time = Time.unscaledTime;
            while (Time.unscaledTime - time < 0.3f)
            {
                CanvasGroup.alpha = 1f - (Time.unscaledTime - time) / 0.5f;
                
                yield return new WaitForSecondsRealtime(0.05f);
            }

            CanvasGroup.alpha = 0f;            
            onCompleted?.Invoke();
        }
    }
}