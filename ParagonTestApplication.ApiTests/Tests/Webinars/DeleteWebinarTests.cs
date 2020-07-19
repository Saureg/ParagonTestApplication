namespace ParagonTestApplication.ApiTests.Tests.Webinars
{
    using System.Net;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using ParagonTestApplication.ApiTests.TestData;
    using Shouldly;

    /// <summary>
    /// Delete webinar tests.
    /// </summary>
    public class DeleteWebinarTests : BaseWebinarsTests
    {
        /// <summary>
        /// Check deleting webinar.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task DeleteWebinarTest()
        {
            var createdWebinarResponse =
                await this.WebinarApiHelper.CreateWebinar(CreateOrUpdateWebinarTestData.GenerateRequest());

            await this.WebinarApiHelper.DeleteWebinar(createdWebinarResponse.Data.Id);

            var webinarResponse = await this.WebinarApiHelper.GetWebinar(createdWebinarResponse.Data.Id);

            webinarResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Check deleting webinar with unknown id.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task DeleteWebinarWithUnknownIdTest()
        {
            const int unknownId = 0;

            var response = await this.WebinarApiHelper.DeleteWebinar(unknownId);

            response.ShouldSatisfyAllConditions(
                () => response.StatusCode.ShouldBe(HttpStatusCode.NotFound),
                () => response.Data.ShouldBeNull(),
                () => response.Message.ShouldBe($"Webinar with id={unknownId} not found"));
        }

        /// <summary>
        /// Check deleting webinar with invalid id.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task DeleteWebinarWithInvalidIdTest()
        {
            var response = await this.WebinarApiHelper.DeleteWebinarWithInvalidId("first");

            response.ShouldSatisfyAllConditions(
                () => response.Status.ShouldBe(HttpStatusCode.BadRequest),
                () => response.Title.ShouldBe("One or more validation errors occurred."),
                () => response.Errors.Count.ShouldBe(1));
        }
    }
}