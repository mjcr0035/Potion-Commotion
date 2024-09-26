using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //matthew:
    //stores important player and scene information
    [Header("Levels Completed")]
    public int levelsCompleted;

    [Header("Level High Scores")]
    public float levelOneScore;
    public float levelTwoScore;
    public float levelThreeScore;

    [Header("Endless Unlocked")]
    public bool endlessUnlock;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    
}
