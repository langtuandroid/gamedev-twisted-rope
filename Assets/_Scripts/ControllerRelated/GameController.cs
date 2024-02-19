using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Twisted._Scripts.ControllerRelated
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;
        public GameObject confetti;

        // Lion Analytics Counter;
        //public static int AttemptsCounter;
        [SerializeField] private LayerMask clickPreventMask;
        public PhysicMaterial ropePhysicsMat;

        [SerializeField] private List<Material> handleMaterials;

        private void Awake() => instance = this;

        private void Start()
        {
            Camera.main.eventMask = clickPreventMask;
            Time.timeScale = 1;
        }

        public Material GetHandleMaterial(Material material)
        {
            Material target = null;
            foreach (var mat in handleMaterials)
            {
                if (material.name.Contains(mat.name))
                {
                    target = mat;
                    break;
                }
            }
            return target;
        }

        private void OnEnable()
        {
            MainController.GameStateChanged += GameManager_GameStateChanged;
        }

        private void OnDisable()
        {
            MainController.GameStateChanged -= GameManager_GameStateChanged;
        }

        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Levelwin)
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    // Vibration.Vibrate(27);
                    confetti.SetActive(true);
                    SoundsController.instance.PlaySound(SoundsController.instance.confetti);
                });
            }
        }
        
        public void On_NextButtonClicked()
        {
            if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(UnityEngine.Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
                PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
                if (PlayerPrefs.GetInt("lastLevelPlayed") < SceneManager.GetActiveScene().buildIndex)
                {
                    PlayerPrefs.SetInt("lastLevelPlayed", SceneManager.GetActiveScene().buildIndex);
                }
            }

            PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
        }

        public void On_ResetButtonPressed()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public IEnumerator DumpUnUsed(GameObject obj)
        {
            yield return new WaitForSeconds(1.5f);
            obj.SetActive(false);
        }
    }
}