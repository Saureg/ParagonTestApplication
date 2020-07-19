// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ParagonTestApplication.PerformanceTests.Config
{
    // ReSharper disable once ClassNeverInstantiated.Global

    /// <summary>
    /// Injection rate model.
    /// </summary>
    public class InjectionRate
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets thread count.
        /// </summary>
        public int ThreadCount { get; set; }

        /// <summary>
        /// Gets or sets pause.
        /// </summary>
        public int Pause { get; set; }
    }
}