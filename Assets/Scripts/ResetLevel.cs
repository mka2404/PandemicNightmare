using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public void LevelReset(){
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Level Select");
    }
}
