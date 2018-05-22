using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour {

    public void LoadLevel(string levelToLoad) {
        Application.LoadLevel(levelToLoad);
    }
}
