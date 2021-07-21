using Elevator.Interfaces;
using System.Threading.Tasks;

namespace Elevator.Domain
{
    class Level : ILevel
    {
        const int OPEN_DOOR_DELAY = 1500;
        const int CLOSE_DOOR_DELAY = 1000;

        private Elevator _elevator;

        public int LevelNumber { get; }

        public Level(int levelNumber, Elevator elevator)
        {
            LevelNumber = levelNumber;
            _elevator = elevator;
        }

        public void RequestCar()
        {
            _elevator.RequestCar(LevelNumber);
        }

        internal Task OpenDoor() => Task.Delay(OPEN_DOOR_DELAY);
        internal Task CloseDoor() => Task.Delay(CLOSE_DOOR_DELAY);
    }
}
