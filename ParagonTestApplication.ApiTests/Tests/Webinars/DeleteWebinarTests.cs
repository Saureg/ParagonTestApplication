using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using ParagonTestApplication.ApiTests.TestData;
using Shouldly;

namespace ParagonTestApplication.ApiTests.Tests.Webinars
{
    public class DeleteWebinarTests : BaseWebinarsTests
    {
        [Test]
        public async Task DeleteWebinarTest()
        {
            var createdWebinarResponse =
                await WebinarApiHelper.CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest());

            await WebinarApiHelper.DeleteWebinar(createdWebinarResponse.Data.Id);

            var webinarResponse = await WebinarApiHelper.GetWebinar(createdWebinarResponse.Data.Id);

            webinarResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task DeleteWebinarWithUnknownIdTest()
        {
            const int unknownId = 0;

            var response = await WebinarApiHelper.DeleteWebinar(unknownId);

            response.ShouldSatisfyAllConditions
            (
                () => response.StatusCode.ShouldBe(HttpStatusCode.NotFound),
                () => response.Data.ShouldBeNull(),
                () => response.Message.ShouldBe($"Webinar with id={unknownId} not found")
            );
        }

        [Test]
        public async Task DeleteWebinarWithInvalidIdTest()
        {
            var response = await WebinarApiHelper.DeleteWebinarWithInvalidId("first");

            response.ShouldSatisfyAllConditions
            (
                () => response.Status.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Title.ShouldBe("One or more validation errors occurred."),
                () => response.Errors.Count.ShouldBe(1)
            );
        }
    }
}