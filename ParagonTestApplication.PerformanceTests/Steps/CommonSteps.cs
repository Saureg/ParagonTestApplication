using NBomber.Contracts;
using NBomber.CSharp;
using NUnit.Framework;

namespace ParagonTestApplication.PerformanceTests.Steps
{
    public static class CommonSteps
    {
        public static IStep Pause(int min, int max)
        {
            var pause = TestContext.CurrentContext.Random.Next(min, max);
            var pauseStep = Step.CreatePause(pause);
            return pauseStep;
        }

        public static IStep Pause(int pause)
        {
            var pauseStep = Step.CreatePause(pause);
            return pauseStep;
        }
    }
}