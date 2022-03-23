namespace _StrikerDucks._Gameplay._GameplaySteering
{
    public class PursueBall : Pursue
    {
        protected override void Update()
        {
            goal = ball.transform;
            base.Update();
        }
    }
}
