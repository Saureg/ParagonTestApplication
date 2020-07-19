namespace ParagonTestApplication.ApiTests.Models.TestDataModels.Webinars
{
    using ParagonTestApplication.Models.ApiModels.Webinars;

    /// <summary>
    /// Test data model for CreateTest.
    /// </summary>
    public class CreateTestDataModel
    {
        /// <summary>
        /// Gets or sets.
        /// </summary>
        public string TestDescription { get; set; }

        /// <summary>
        /// Gets or sets request.
        /// </summary>
        public CreateOrUpdateWebinarRequest CreateWebinarRequest { get; set; }

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