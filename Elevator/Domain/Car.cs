using System;
using System.Threading.Tasks;

namespace Elevator.Domain
{
    class Car
    {
        const int ONE_LEVEL_MOVEMENT_TIME = 2000;

        private readonly Elevator _elevator;

        public bool IsMoving { get; private set;}
        public Level CurrentLevel { get; private set; }
        public Level Destination { get; private set;}

        public Car(Level initLevel, Elevator elevator)
        {
            CurrentLevel = initLevel;
            _elevator = elevator;
        }

        internal async Task GoTo(Level destination)
        {
            if (IsMoving)
                throw new InvalidOperationException("Already Moving");

            IsMoving = true;
            Destination = destination;

            await CurrentLevel.CloseDoor();
            await Move();
            await CurrentLevel.OpenDoor();

            IsMoving = false;
        }

        async Task Move() 
        {
            var distance = Math.Abs(Destination.LevelNumber - CurrentLevel.LevelNumber);
            for(int i = 0; i < distance; i++)
            {
                await Task.Delay(ONE_LEVEL_MOVEMENT_TIME);
                if (Destination.LevelNumber > CurrentLevel.LevelNumber)
                    CurrentLevel = _elevator.GetLevel(CurrentLevel.LevelNumber + 1);
                else
                    CurrentLevel = _elevator.GetLevel(CurrentLevel.LevelNumber - 1);
            }

            Destination = null;
        } 
    }
}
