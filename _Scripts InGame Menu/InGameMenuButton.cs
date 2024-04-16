using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuButton : MonoBehaviour
{
    public void OnClickMenu()
    {
        SceneManager.LoadScene(0);
    }
}
