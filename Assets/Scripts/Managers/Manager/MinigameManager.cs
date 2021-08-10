using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneReferenceInEditor;
using BikeRidingMinigame;

[System.Serializable]
public class Minigame
{
    [HideInInspector]
    public string Name;
    public string path;
    public SceneReference scene;
    public MinigameBehaviour minigameBehaviour;

    public Minigame(SceneReference sceneReference)
    {
        scene = sceneReference;
    }

    public void SetNameAndPath(string assetPath)
    {
        SetName(assetPath);
        SetPath(assetPath);
    }

    public void SetName(string assetPath)
    {
        if (scene.ScenePath.Contains(assetPath))
        {
            Name = scene.ScenePath.Replace(assetPath, string.Empty);
        }
        Name = Name.Replace(".unity", string.Empty);
    }

    public void SetPath(string assetPath)
    {
       path = assetPath + Name;

        if (path.Contains("Assets/"))
        {
            path = path.Replace("Assets/", string.Empty);
        }
    }
}

public class MinigameManager : MonoBehaviour {

    public static MinigameManager instance = null;

    public string MinigamesAssetPath = "Assets/Scenes/Minigames/";
    

    [Header("Minigame Parameters")]
    public ShopParameter shopParameter;

    [HideInInspector]
    public SceneReference nullScene;

    public Minigame currentMinigame;

    public List<Minigame> minigames;

    #region MonoBehaviour

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	

	// Update is called once per frame
	void Update () {
    }

    #endregion

    #region PublicMethod

    public void UpdateCurrentMinigame(SceneReference scene)
    {
        currentMinigame = new Minigame(scene);
        currentMinigame.SetNameAndPath(MinigamesAssetPath);
        GameManager.instance.gameState = GameState.Minigame;
    }

    public void EndMinigame(bool won)
    {

        if (won)
        {

        }
        else
        {

        }

        

        GameManager.instance.gameState = GameState.Navigation;
        currentMinigame.scene = nullScene;
    }

    #endregion
}
