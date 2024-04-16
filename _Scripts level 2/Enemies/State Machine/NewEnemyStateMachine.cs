
public class NewEnemyStateMachine
{
   public NewEnemyState CurrentEnemyState { get; private set; }

   public void Initialize(NewEnemyState startingState)
   {
      CurrentEnemyState = startingState;
      CurrentEnemyState.EnterState();
   }

   public void ChangeState(NewEnemyState newState)
   {
      CurrentEnemyState.ExitState();
      CurrentEnemyState = newState;
      CurrentEnemyState.EnterState();
   }
}
