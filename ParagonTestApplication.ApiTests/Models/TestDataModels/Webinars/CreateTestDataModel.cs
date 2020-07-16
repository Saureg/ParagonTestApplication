using ParagonTestApplication.Models.ApiModels.Webinars;

namespace ParagonTestApplication.ApiTests.Models.TestDataModels.Webinars
{
    public class CreateTestDataModel
    {
        public string TestDescription { get; set; }

        public CreateOrUpdateWebinarRequest CreateWebinarRequest { get; set; }

        public string ExpectedValidationMessage { get; set; }

        public override string ToString()
        {
            return TestDescription;
        }
    }
}