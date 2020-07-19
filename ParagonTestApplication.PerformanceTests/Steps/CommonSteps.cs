namespace ParagonTestApplication.PerformanceTests.Steps
{
    using System;
    using NBomber.Contracts;
    using NBomber.CSharp;

    /// <summary>
    /// Common steps.
    /// </summary>
    public static class CommonSteps
    {
        /// <summary>
        /// Pause step.
        /// </summary>
        /// <param name="min">minimum pause.</param>
        /// <param name="max">maximum pause.</param>
        /// <returns>Step.</returns>
        public static IStep Pause(int min, int max)
        {
            var random = new Random();
            var pause = random.Next(min, max);
            var pauseStep = Step.CreatePause(pause);
            return pauseStep;
        }

        /// <summary>
        /// Pause step.
        /// </summary>
        /// <param name="pause">Pause.</param>
        /// <returns>Step.</returns>
        public static IStep Pause(int pause)
        {
            var pauseStep = Step.CreatePause(pause);
            return pauseStep;
        }
    }
}