namespace ParagonTestApplication.ApiTests.TestData
{
    using System;
    using System.Collections.Generic;
    using ParagonTestApplication.ApiTests.Helpers;
    using ParagonTestApplication.ApiTests.Models.TestDataModels.Webinars;
    using ParagonTestApplication.Models.ApiModels.Series;
    using ParagonTestApplication.Models.ApiModels.Webinars;

    /// <summary>
    /// Test data for creating or update tests.
    /// </summary>
    public static class CreateOrUpdateWebinarTestData
    {
        /// <summary>
        /// Generate validation test data list.
        /// </summary>
        /// <returns>CreateTestDataModel list.</returns>
        public static List<CreateTestDataModel> GenerateValidationTestDataList()
        {
            var list = new List<CreateTestDataModel>
            {
                new CreateTestDataModel
                {
                    TestDescription = "WithoutName",
                    ExpectedValidationMessage = "Name is required",
                    CreateWebinarRequest = GenerateRequest(withoutName: true),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithLongName",
                    ExpectedValidationMessage = "Name has to be between 1 and 50 characters long",
                    CreateWebinarRequest = GenerateRequest(new string('a', 51)),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithEmptyName",
                    ExpectedValidationMessage = "Name has to be between 1 and 50 characters long",
                    CreateWebinarRequest = GenerateRequest(string.Empty),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithZeroDuration",
                    ExpectedValidationMessage = "Duration must be equal or greater than 1 minute",
                    CreateWebinarRequest = GenerateRequest(duration: 0),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithLongDuration",
                    ExpectedValidationMessage = "Duration must be less than 24 hours",
                    CreateWebinarRequest = GenerateRequest(duration: 1441),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithoutStartDateTime",
                    ExpectedValidationMessage = "StartDateTime is required",
                    CreateWebinarRequest = GenerateRequest(withoutStartDateTime: true),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithInvalidStartDateTime",
                    ExpectedValidationMessage = "StartDateTime must be in format 2010-01-11T11:41",
                    CreateWebinarRequest = GenerateRequest(startDateTime: "datetime"),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithoutSeries",
                    ExpectedValidationMessage = "Series is required",
                    CreateWebinarRequest = GenerateRequest(withoutSeries: true),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithoutSeriesName",
                    ExpectedValidationMessage = "SeriesName is required",
                    CreateWebinarRequest = GenerateRequest(withoutSeriesName: true),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithEmptySeriesName",
                    ExpectedValidationMessage = "SeriesName has to be between 1 and 50 characters long",
                    CreateWebinarRequest = GenerateRequest(seriesName: string.Empty),
                },
                new CreateTestDataModel
                {
                    TestDescription = "WithLongSeriesName",
                    ExpectedValidationMessage = "SeriesName has to be between 1 and 50 characters long",
                    CreateWebinarRequest = GenerateRequest(seriesName: new string('s', 51)),
                },
            };
            return list;
        }

        /// <summary>
        /// Generate request.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="startDateTime">Start datetime.</param>
        /// <param name="seriesName">Series name.</param>
        /// <param name="withoutName">Without name.</param>
        /// <param name="withoutDuration">Without duration.</param>
        /// <param name="withoutStartDateTime">Without start datetime.</param>
        /// <param name="withoutSeries">Without series.</param>
        /// <param name="withoutSeriesName">Without series name.</param>
        /// <returns>CreateOrUpdateWebinarRequest.</returns>
        public static CreateOrUpdateWebinarRequest GenerateRequest(
            string name = null,
            int? duration = null,
            string startDateTime = null,
            string seriesName = null,
            bool withoutName = false,
            bool withoutDuration = false,
            bool withoutStartDateTime = false,
            bool withoutSeries = false,
            bool withoutSeriesName = false)
        {
            string webinarName;
            if (withoutName)
            {
                webinarName = null;
            }
            else
            {
                webinarName = name ?? Guid.NewGuid().ToString();
            }

            int? webinarDuration;
            if (withoutDuration)
            {
                webinarDuration = null;
            }
            else
            {
                webinarDuration = duration ?? 15;
            }

            string webinarStartDateTime;
            if (withoutStartDateTime)
            {
                webinarStartDateTime = null;
            }
            else
            {
                webinarStartDateTime = startDateTime ??
                                       DateTime.Now.RemoveSecondsAndMilliseconds().ToDateTimeWithMinutesString();
            }

            CreateOrUpdateSeriesRequest webinarSeries;
            if (withoutSeries)
            {
                webinarSeries = null;
            }
            else
            {
                if (withoutSeriesName)
                {
                    webinarSeries = new CreateOrUpdateSeriesRequest();
                }
                else
                {
                    webinarSeries = new CreateOrUpdateSeriesRequest
                    {
                        Name = seriesName ?? Guid.NewGuid().ToString(),
                    };
                }
            }

            var generatedRequest = new CreateOrUpdateWebinarRequest
            {
                Name = webinarName,
                Duration = webinarDuration.GetValueOrDefault(),
                StartDateTime = webinarStartDateTime,
                Series = webinarSeries,
            };
            return generatedRequest;
        }
    }
}