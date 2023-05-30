using System;
using System.Collections.Generic;

namespace lab12_pudov
{
    public class Model
    {
        private double[][] Q = {
            new double[]{-0.4, 0, 0.75, 0.25},
            new double[]{-0.8, 0.5, 0, 0.5 },
            new double[]{-0.5, 0.2, 0.8, 0 }
        }; 
        private Random Random = new Random();
        public double t;
        public double dt = 1;
        public List<WeatherState> states;
        public List<double> times;
        public double[] stateTimes;

        public void Init()
        {
            t = 0;
            states = new List<WeatherState> { WeatherState.Fine };
            times = new List<double> { 0 };
            stateTimes = new double[3]{ 0, 0, 0 };
            AddState();
        }

        public void NextTick()
        {
            t += dt;

            while (times[times.Count - 1] <= t)
            {
                AddState();
            }
        }

        private void AddState()
        {
            int currentState = (int)states[states.Count - 1];
            double tay = Math.Log(Random.NextDouble()) / Q[currentState][0];
            stateTimes[currentState] += tay;
            times.Add(times[times.Count - 1] + tay);
            states.Add(GenerateState(Q[currentState]));
        }

        private WeatherState GenerateState(double[] prob)
        {
            var value = Random.NextDouble();

            int i = 1;
            while (prob[i] <= value)
            {
                value -= prob[i++];
            }

            return (WeatherState)(i - 1);
        }
    }

    public enum WeatherState
    {
        Fine = 0,
        Cloudy = 1,
        Rainy = 2
    }
}
