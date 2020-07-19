namespace ParagonTestApplication.ApiTests.Models.TestDataModels.Webinars
{
    using ParagonTestApplication.Models.ApiModels.Webinars;
    using ParagonTestApplication.Models.Common;

    /// <summary>
    /// Test data model for GetListTest.
    /// </summary>
    public class GetListTestDataModel
    {
        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string TestDescription { get; set; }

        /// <summary>
        /// Gets or sets webinar filter.
        /// </summary>
        public WebinarFilter WebinarFilter { get; set; }

        /// <summary>
        /// Gets or sets pagination filter.
        /// </summary>
        public PaginationFilter PaginationFilter { get; set; }

        /// <summary>
        /// Gets or sets expected validation message.
        /// </summary>
        public string ExpectedValidationMessage { get; set; }

        /// <summary>
        /// Test description to string.
        /// </summary>
        /// <returns>Test description.</returns>
        public override string ToString()
        {
            return this.TestDescription;
        }
    }
}