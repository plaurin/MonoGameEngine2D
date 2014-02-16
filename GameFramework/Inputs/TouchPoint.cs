namespace GameFramework.Inputs
{
    public class TouchPoint
    {
        public TouchPoint(int id, Vector position, float pressure, TouchPointState state)
        {
            this.Id = id;
            this.Position = position;
            this.Pressure = pressure;
            this.State = state;
        }

        public int Id { get; private set; }

        public Vector Position { get; private set; }

        public float Pressure { get; private set; }

        public TouchPointState State { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1} P:{2:f2} ({3})", this.Id, this.Position, this.Pressure, this.State);
        }
    }
}