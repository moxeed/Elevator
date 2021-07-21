using Elevator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevator.Domain
{
    class Elevator
    {
        private Queue<int> _requests;
        private IDictionary<int, Level> _levels;
        internal IReadOnlyList<ILevel> Levels => _levels.Values.ToList();
        private Car _car;

        public Elevator(int levelCount)
        {
            _requests = new Queue<int>();
            _levels = new Dictionary<int, Level>();

            for(int i = 1; i <= levelCount; i++)
            {
                _levels[i] = new Level(i, this);
            }

            _car = new Car(_levels.Values.First(), this);
        }

        internal Level GetLevel(int levelNumber) => _levels[levelNumber];
        Action WorkerAction => () =>
        {
            while(true)
            {
                Task.Delay(500).Wait();
                _stateChangeEvent.Invoke(_car.CurrentLevel.LevelNumber, _car.Destination?.LevelNumber, _requests.ToList());

                if (_car.IsMoving || !_requests.Any())
                    continue;
                
                var request = _requests.Dequeue();
                _car.GoTo(_levels[request]).ConfigureAwait(false);
            }
        };

        Task _workerTask;
        internal void Start() => _workerTask = Task.Run(WorkerAction);
        internal void Stop() => _workerTask.Dispose();
        internal void RequestCar(int levelNumber)
        {
            _requests.Enqueue(levelNumber);
        }

        private event Action<int, int?, IList<int>> _stateChangeEvent;
        internal void AddChangeListner(Action<int, int?, IList<int>> action) => _stateChangeEvent += action;
    }
}
