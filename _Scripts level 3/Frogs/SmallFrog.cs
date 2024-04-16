using UnityEngine;

public class SmallFrog : BaseFrog
{
   [SerializeField] private float frogShowTime = 6f;
   [SerializeField] private float minHiddenTime = 1f;
   [SerializeField] private float maxHiddenTime = 4f;
   protected override void Initialize()
   {
      base.Initialize();
      FrogShowTime = frogShowTime;
      OriginalFrogShowTime = frogShowTime;
      TimeToResurfaceMin = minHiddenTime;
      TimeToResurfaceMax = maxHiddenTime;

   }
   
   private void Update()
   {
      FrogShowHide();
   }
}
