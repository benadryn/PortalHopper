using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public void OnClickLevel(int sceneNumber)
    {
        LevelManager.Instance.LoadSpecificLevel(sceneNumber);
    }
}
