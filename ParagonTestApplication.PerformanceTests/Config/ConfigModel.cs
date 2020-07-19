// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ParagonTestApplication.PerformanceTests.Config
{
    using System;
    using System.Collections.Generic;

    // ReSharper disable once ClassNeverInstantiated.Global

    /// <summary>
    /// Config model.
    /// </summary>
    public class ConfigModel
    {
        /// <summary>
        /// Gets or sets url.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets duration.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets min pause.
        /// </summary>
        public int MinPause { get; set; }

        /// <summary>
        /// Gets or sets max pause.
        /// </summary>
        public int MaxPause { get; set; }

        // ReSharper disable once CollectionNeverUpdated.Global

        /// <summary>
        /// Gets or sets injection rates.
        /// </summary>
        public List<InjectionRate> InjectionRates { get; set; }
    }
}