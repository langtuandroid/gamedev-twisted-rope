using System;
using System.Collections;
using _Scripts.ElementRelated;
using _Scripts.GameplayRelated;
using _Twisted._Scripts.ControllerRelated;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace _Twisted._Scripts.ElementRelated
{
    public class RopeElement : MonoBehaviour
    {
        public Transform handle1, handle2;
        
        [HideInInspector] public bool fingerUp, rolledUp;
        private bool intouch;
        private float timer;
        public float starttime;

        public RopeNode collidedRopeNode;
        private float _initialDistFromMid;
        private bool _checkStress;

        private void OnEnable()
        {
            AddMissingComponentsToNodes();
        }

        void Start()
        {
            starttime = 1f;
            intouch = false;
            timer = starttime;
            fingerUp = true;
            ChangeHandleColor();
            Decider.instance.RopeAdded();
        }

        void ChangeHandleColor()
        {
            handle1.GetComponent<Renderer>().material = GameController.instance.GetHandleMaterial(GetComponent<Renderer>().material);
            handle2.GetComponent<Renderer>().material = GameController.instance.GetHandleMaterial(GetComponent<Renderer>().material);
        }
        public void InContact() => intouch = true;

        private void NoContact() => intouch = false;
        
        void Update()
        {
            if (!intouch && fingerUp)
            {
                timer -= Time.deltaTime;
                if (timer <= 0 && !rolledUp)
                {
                    rolledUp = true;
                    StartCoroutine(RollUpRope());
                }
            }
            else
            {
                timer = starttime;
            }

            NoContact();
            
            if (!_checkStress || !collidedRopeNode) return;
            Vector3 midPoint = (handle1.position + handle2.position) / 2;
            float dist = Vector3.Distance(collidedRopeNode.transform.position, midPoint);
            if(dist >= _initialDistFromMid + 1f)
            {
                SnapHandles();
                collidedRopeNode = null;
            }
        }

        private void SnapHandles()
        {
            ObjectDrag dragScript1 = handle1.GetComponent<ObjectDrag>();
            dragScript1.targetSnapPosition = handle1.GetComponent<HandleElement>()._lastPos;
            dragScript1.isSnapping = true;
            ObjectDrag dragScript2 = handle2.GetComponent<ObjectDrag>();
            dragScript2.targetSnapPosition = handle2.GetComponent<HandleElement>()._lastPos;
            dragScript2.isSnapping = true;
        }
        IEnumerator RollUpRope()
        {
            Decider.instance.RopeCollapsed();
            ChangeLayersOfNodes();
            handle1.GetComponent<Collider>().enabled = false;
            handle1.DORotate(Vector3.right * 180, 0.5f);
            
            Vector3 movePos = new Vector3(handle2.position.x, 0.35f, handle2.position.z);
            handle1.DOMove(movePos, 0.5f).SetEase(Ease.OutBack);
            handle2.DOMoveY(handle2.position.y - 15, 0.5f);
            SoundsController.instance.PlaySound(SoundsController.instance.cupUp);
            // Vibration.Vibrate(30);
            
            yield return new  WaitForSeconds(0.5f);
            Transform fx = Instantiate(FxController.instance.ropeSortFx, movePos, quaternion.identity).transform;
            GameController.instance.StartCoroutine(GameController.instance.DumpUnUsed(fx.gameObject));
            
            handle1.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
            handle2.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.35f);
            handle1.gameObject.SetActive(false);
            handle2.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        void AddMissingComponentsToNodes()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.tag = "otherrope";
                RopeNode ropeNode = child.GetComponent<RopeNode>();
                if (ropeNode == null)
                    child.AddComponent<RopeNode>();
                child.gameObject.layer = 9;
            }
        }

        void ChangeLayersOfNodes()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.layer = 6;
            }
        }
    }
}