namespace _StrikerDucks._Gameplay._GameplaySteering
{
    public class FleeBall : Flee
    {
        protected override void Update()
        {
            destTarget.position = ball.transform.position;
            base.Update();
        }
    }
}
