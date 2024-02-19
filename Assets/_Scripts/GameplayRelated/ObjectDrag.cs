using _Scripts.ElementRelated;
using _Twisted._Scripts.ControllerRelated;
using _Twisted._Scripts.ElementRelated;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GameplayRelated
{
    public class ObjectDrag : MonoBehaviour
    {
        private Vector3 offset2;
        [SerializeField] private PlaceChecker placeChecker;
        public bool draggable;
        public bool isDragging;
    
        private Vector3 screenPoint;
        private Vector3 offset;
        private float dragSpeed = 20;
        private float distanceFromFinger = 1;
        [SerializeField] private RopeElement _ropeElement;

        [HideInInspector] public float _coolDownTime = 1.5f, _timeCounter = 1.5f;
        private bool _canStartTimer;

        public bool isSnapping;
        [HideInInspector] public Vector3 targetSnapPosition;

        private void Start()
        {
            draggable = true;
            isDragging = false;
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject() || !draggable || _timeCounter < _coolDownTime) return;
            isDragging = true;
            _ropeElement.fingerUp = false;
            _canStartTimer = false;
            placeChecker.GetComponent<Collider>().enabled = true;
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            offset = gameObject.transform.position - mouseWorldPos - (mouseWorldPos - Camera.main.transform.position).normalized * distanceFromFinger;
            SoundsController.instance.PlaySound(SoundsController.instance.pop);
            // Vibration.Vibrate(30);
        }

        private void OnMouseDrag()
        {
            if (isSnapping)
            {
                transform.position = Vector3.Lerp(transform.position, targetSnapPosition, Time.deltaTime * dragSpeed);
                /*if (Vector3.Distance(transform.position, targetSnapPosition) <= 0.1f)
                isSnapping = false;*/
            }
            else
            {
                if(!draggable || EventSystem.current.IsPointerOverGameObject() || _timeCounter < _coolDownTime) return;
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
                transform.position = Vector3.Lerp(transform.position, curPosition, Time.deltaTime * dragSpeed);
            }
        }

        private void OnMouseUp()
        {
            isDragging = false;
            if(!draggable || EventSystem.current.IsPointerOverGameObject() || _timeCounter < _coolDownTime) return;
            placeChecker.PlaceHandle();
            _ropeElement.fingerUp = true;
            SoundsController.instance.PlaySound(SoundsController.instance.pop);
            if(Help.instance) Help.instance.ShowHelp(); 
            _canStartTimer = true;
            _timeCounter = 0;
            isSnapping = false;
        }

        private void Update()
        {
            if (_canStartTimer)
            {
                _timeCounter += Time.deltaTime;
            }
        }
    }
}