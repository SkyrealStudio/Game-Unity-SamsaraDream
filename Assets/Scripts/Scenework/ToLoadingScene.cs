using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ToLoadingScene : MonoBehaviour
{
    public Text text;

    [Tooltip("Name of the next aScene")]
    public string nextSceneName;

    public void OnPointerClick()
    {
        text.text = "Loading...";
        SceneManager.LoadScene(nextSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
