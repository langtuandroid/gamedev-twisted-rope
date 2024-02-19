using _Scripts.GameplayRelated;
using UnityEngine;

namespace _Scripts.ElementRelated
{
    public class HandleElement : MonoBehaviour
    {
        private ObjectDrag _dragScript;
        [HideInInspector] public bool placePossible;
        [HideInInspector] public Vector3 _lastPos;
    
        private HandleGridElement _handleGridElement;
    
        private void Start()
        {
            _dragScript = GetComponent<ObjectDrag>();
            _lastPos = transform.position;
        }

        public bool PlaceTheHandle(Vector3 gridPos, bool isBusy)
        {
            if(placePossible && !isBusy)
            {
                Vector3 pos = new Vector3(gridPos.x, 0, gridPos.z);
                transform.position = pos;
                _lastPos = pos;
                _dragScript.draggable = true;
                return true;
            }

            _dragScript.draggable = true;
            transform.position = _lastPos;
            return false;
        }
    }
}
