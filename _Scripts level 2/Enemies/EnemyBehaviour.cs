using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
   private IDeathBehaviour _deathBehaviour;
 
   private Animator _animator;
   private static readonly int IsDead = Animator.StringToHash("IsDead");


   private void Awake()
   {
      _animator = GetComponent<Animator>();
      _deathBehaviour = GetComponent<IDeathBehaviour>();
   }
   public void Die()
   {
      if (_deathBehaviour == null) return;
         
      _animator.SetBool(IsDead, true);
      _deathBehaviour.StartDeath();
   }
   
}
