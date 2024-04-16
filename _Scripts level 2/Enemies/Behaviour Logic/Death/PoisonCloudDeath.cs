using System.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Death-Poison Cloud", menuName = "Enemy Logic/Death/Poison Cloud Death")]
public class PoisonCloudDeath : EnemyDeathSOBase
{
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private GameObject _poisonGasParent;
    private MushroomEnemy _mushroomEnemy;
    [SerializeField] private float poisonGasStart = 1f;
    [SerializeField] private float poisonGasStop = 1.5f;
    [SerializeField] private float despawnTime = 1.5f;  
    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
        _mushroomEnemy = Enemy as MushroomEnemy;
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Enemy.AudioSource.Stop();
        Enemy.AudioSource.PlayOneShot(Enemy.deathSfx);
        Enemy.PlayerDetectionCollider.enabled = false;
        Enemy.Animator.SetBool(IsDead, true);
        if (_mushroomEnemy && _mushroomEnemy.poisonGasParent)
        {
            _poisonGasParent = _mushroomEnemy.poisonGasParent;
        }
        Enemy.StartCoroutine(SendPoisonGas());
        
    }

    private IEnumerator SendPoisonGas()
    {
        Enemy.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        Enemy.CapsuleCollider2D.enabled = false;
        yield return new WaitForSeconds(poisonGasStart);
        Enemy.AudioSource.PlayOneShot(_mushroomEnemy.gasSfx);
        _poisonGasParent.SetActive(true);
        Enemy.StartCoroutine(StopPoisonGas());

    }

    private IEnumerator StopPoisonGas()
    {
        yield return new WaitForSeconds(poisonGasStop);
        _poisonGasParent.SetActive(false);
        Enemy.StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(GameObject);
    }
    
    
}
