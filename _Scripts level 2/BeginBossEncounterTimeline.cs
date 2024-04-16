using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BeginBossEncounterTimeline : MonoBehaviour
{
    [SerializeField] private FloatVariable playerHealthPoints;
    [SerializeField] private PlayableDirector bossTimeline;
    private Vector3 _playerEndPos;
    private HandleDamage _handleDamage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _handleDamage = other.GetComponent<HandleDamage>();
            bossTimeline.Play();

            StartCoroutine(IncreaseHealthOverTime());
        }
    }

    private IEnumerator IncreaseHealthOverTime()
    {
        var targetHealth = 100f;
        var incrementAmount = 1f;
        var delayBetweenIncrement = 0.1f;

        while (playerHealthPoints.Value < targetHealth)
        {
            playerHealthPoints.Value += incrementAmount;
            _handleDamage.UpdateSliderValue();

            yield return new WaitForSeconds(delayBetweenIncrement);
        }
    }
}
