using System.Collections.Generic;
using ParagonTestApplication.ApiTests.Models.TestDataModels.Webinars;
using ParagonTestApplication.Models.ApiModels.Webinars;
using ParagonTestApplication.Models.Common;

namespace ParagonTestApplication.ApiTests.TestData
{
    public class GetListWebinarTestData
    {
        public static List<GetListTestDataModel> GenerateValidationTestDataList()
        {
            var list = new List<GetListTestDataModel>
            {
                new GetListTestDataModel
                {
                    TestDescription = "WithInvalidPageNumber",
                    PaginationFilter = new PaginationFilter
                    {
                        PageNumber = 0
                    },
                    ExpectedValidationMessage = "PageNumber must be equal or greater than 1"
                },
                new GetListTestDataModel
                {
                    TestDescription = "WithInvalidPageSize",
                    PaginationFilter = new PaginationFilter
                    {
                        PageSize = 0
                    },
                    ExpectedValidationMessage = "PageSize must be equal or greater than 1"
                },
                new GetListTestDataModel
                {
                    TestDescription = "WithInvalidMinDateTime",
                    WebinarFilter = new WebinarFilter
                    {
                        MinDateTime = "123"
                    },
                    ExpectedValidationMessage = "MinDateTime must be in format 2010-01-11T11:41"
                },
                new GetListTestDataModel
                {
                    TestDescription = "WithInvalidMaxDateTime",
                    WebinarFilter = new WebinarFilter
                    {
                        MaxDateTime = "123"
                    },
                    ExpectedValidationMessage = "MaxDateTime must be in format 2010-01-11T11:41"
                },
                new GetListTestDataModel
                {
                    TestDescription = "WithNegativeMinDuration",
                    WebinarFilter = new WebinarFilter
                    {
                        MinDuration = "-1"
                    },
                    ExpectedValidationMessage = "MinDuration must be a positive valid integer"
                },
                new GetListTestDataModel
                {
                    TestDescription = "WithInvalidMinDuration",
                    WebinarFilter = new WebinarFilter
                    {
                        MinDuration = "twelve"
                    },
                    ExpectedValidationMessage = "MinDuration must be a positive valid integer"
                },
                new GetListTestDataModel
                {
                    TestDescription = "WithNegativeMaxDuration",
                    WebinarFilter = new WebinarFilter
                    {
                        MaxDuration = "-1"
                    },
                    ExpectedValidationMessage = "MaxDuration must be a positive valid integer"
                },
                new GetListTestDataModel
                {
                    TestDescription = "WithInvalidMaxDuration",
                    WebinarFilter = new WebinarFilter
                    {
                        MaxDuration = "twelve"
                    },
                    ExpectedValidationMessage = "MaxDuration must be a positive valid integer"
                },
                new GetListTestDataModel
                {
                    TestDescription = "WithNegativeSeriesId",
                    WebinarFilter = new WebinarFilter
                    {
                        SeriesId = "-1"
                    },
                    ExpectedValidationMessage = "SeriesId must be a positive valid integer"
                },
                new GetListTestDataModel
                {
                    TestDescription = "WithInvalidSeriesId",
                    WebinarFilter = new WebinarFilter
                    {
                        SeriesId = "twelve"
                    },
                    ExpectedValidationMessage = "SeriesId must be a positive valid integer"
                }
            };
            return list;
        }
    }
}