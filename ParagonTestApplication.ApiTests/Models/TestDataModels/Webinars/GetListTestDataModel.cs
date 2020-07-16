using ParagonTestApplication.Models.ApiModels.Webinars;
using ParagonTestApplication.Models.Common;

namespace ParagonTestApplication.ApiTests.Models.TestDataModels.Webinars
{
    public class GetListTestDataModel
    {
        public string TestDescription { get; set; }

        public WebinarFilter WebinarFilter { get; set; }

        public PaginationFilter PaginationFilter { get; set; }

        public string ExpectedValidationMessage { get; set; }

        public override string ToString()
        {
            return TestDescription;
        }
    }
}