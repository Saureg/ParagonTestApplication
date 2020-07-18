using System;
using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ParagonTestApplication.PerformanceTests.Config
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ConfigModel
    {
        public Uri Url { get; set; }
        public int Duration { get; set; }
        public int MinPause { get; set; }
        public int MaxPause { get; set; }

        // ReSharper disable once CollectionNeverUpdated.Global
        public List<InjectionRate> InjectionRates { get; set; }
    }
}