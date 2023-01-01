using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [SerializeField] private GameObject tank;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (scene.name.Equals("MainMenu")) return;
        tank.transform.position = scene.name switch {
            "LevelOne" => new Vector3(0, 0, 0),
            "LevelTwo" => new Vector3(0, 0, 0),
            "LevelThree" => new Vector3(0, 0, 0),
            "LevelFour" => new Vector3(0, 0, 0),
            "LevelFive" => new Vector3(0, 0, 0),
            _ => tank.transform.position
        };
    }
}