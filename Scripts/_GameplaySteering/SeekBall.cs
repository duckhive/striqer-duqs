namespace _StrikerDucks._Gameplay._GameplaySteering
{
    public class SeekBall : Seek
    {
        protected override void Update()
        {
            goal = ball.transform;
            base.Update();
        }
    }
}
