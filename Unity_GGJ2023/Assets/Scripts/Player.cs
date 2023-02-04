namespace DefaultNamespace
{
    public class Player : Rotatable
    {
        private int movementSpeed = 10;
        public void MoveClockwise()
        {
            AddRotation(movementSpeed);
        }

        public void MoveCounterClockwise()
        {
            AddRotation(movementSpeed);
        }
    }
}