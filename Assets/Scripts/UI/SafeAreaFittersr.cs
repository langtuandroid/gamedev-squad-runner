using UnityEngine;

namespace UI
{
    public class SafeAreaFittersr : MonoBehaviour
    {
        public bool DontSafeBottosrm;
        private RectTransform _fittedRectTransformsr;
        private Rect _safeRectComponentsr;
        private Vector2 _minAnchorVectorsr;
        private Vector2 _maxAnchorVectorsr;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            _fittedRectTransformsr = GetComponent<RectTransform>();
            _safeRectComponentsr = Screen.safeArea;
            _minAnchorVectorsr = _safeRectComponentsr.position;
            _maxAnchorVectorsr = _minAnchorVectorsr + _safeRectComponentsr.size;
        
            _minAnchorVectorsr.x /= Screen.width;
            _minAnchorVectorsr.y = DontSafeBottosrm ? _minAnchorVectorsr.y = 0 : _minAnchorVectorsr.y /= Screen.height;
            _maxAnchorVectorsr.x /= Screen.width;
            _maxAnchorVectorsr.y /= Screen.height;
        
            _fittedRectTransformsr.anchorMin = _minAnchorVectorsr;
            _fittedRectTransformsr.anchorMax = _maxAnchorVectorsr;
        }
    }
}
