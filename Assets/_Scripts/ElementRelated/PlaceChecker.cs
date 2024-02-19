using System;
using _Scripts.GameplayRelated;
using UnityEngine;

namespace _Scripts.ElementRelated
{
    public class PlaceChecker : MonoBehaviour
    {
        [SerializeField] private HandleGridElement _handleGridElement;
        [SerializeField] private HandleElement handleElement;
        [SerializeField] private ObjectDrag _dragScript;

        private void Start()
        {
            var colliders = Physics.OverlapSphere(transform.position, 0.2f);
            foreach (var cd in colliders)
            {
                if (cd.TryGetComponent<HandleGridElement>(out var hde))
                {
                    hde.ChangeVisual(true);
                    hde.SetIsBusy(true);
                }
            }
        }

        private void OnTriggerEnter(Collider other) => SetPlaceable(other.gameObject);

        private void OnTriggerStay(Collider other) => SetPlaceable(other.gameObject);

        private void SetPlaceable(GameObject other)
        {
            if (other.TryGetComponent<HandleGridElement>(out var hde))
            {
                _handleGridElement = hde;
                _handleGridElement.ChangeVisual(true);
                handleElement.placePossible = true;
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<HandleGridElement>(out var hde))
            {
                HandleGridElement ballPlaceElement = hde;
                ballPlaceElement.ChangeVisual(false);
                ballPlaceElement.SetIsBusy(false);
                handleElement.placePossible = false;
            }
            if (other.CompareTag("floor"))
            {
                _dragScript.targetSnapPosition = handleElement._lastPos;
                _dragScript.isSnapping = true;
            }
        }
        public void PlaceHandle()
        {
            _handleGridElement.SetIsBusy(handleElement.PlaceTheHandle(_handleGridElement.transform.position, _handleGridElement.IsBusy));
        }
    }
}