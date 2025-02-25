using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject enemies;
    void Start()
    {
        Application.targetFrameRate = 120;
    }

    private void Update()
    {
        if (enemies.transform.childCount <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
