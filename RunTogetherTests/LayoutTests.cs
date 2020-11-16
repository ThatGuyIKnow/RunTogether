using Xunit;
using Bunit;
using RunTogether.Shared.Etc;
using RunTogether.Shared.Layouts;

namespace RunTogetherTests
{
    public class LayoutTests : TestContext
    {
        #region Checks if things render correctly
        // Check if sponsor bar renders correctly
        [Fact]
        public void SponsorBarRendersCorrectly()
        {
            // Act
            var cut = RenderComponent<SponsorBar>(); // cut = Component Under Test

            var expectedMarkup = @"<div class=""top-row pl-4 navbar navbar-dark"">
                                    <a class=""navbar-brand"" href="""">Sponsors</a>
                                </div>

                                <div class=""scrollable"">
                                    <ul diff:ignore>
                                    </ul>
                                </div>";

            // Assert
            cut.MarkupMatches(expectedMarkup);
        }

        // Check if main layout renders correctly
        [Fact]
        public void MainLayoutRendersCorrectly()
        {
            // Act
            var cut = RenderComponent<MainLayout>(); // cut = Component Under Test

            var expectedMarkup = @"<div class=""sidebar"" id=""navBarId"">
                                        <div class=""top-row pl-4 navbar navbar-dark"">
                                            <a class=""navbar-brand"" href="""">Navigation</a>
                                        </div>

                                        <div class=""scrollable"">
                                            <ul class=""nav flex-column"" diff:ignore>
                                            </ul>
                                        </div>
                                    </div>

                                    <div class=""main"">
                                        <div class=""top-row px-4 auth"">
                                            <button class=""navbar-toggler navbar-toggler-left"">
                                                <span class=""oi oi-menu""></span>
                                            </button>

                                            <img src=""https://www.runtogether.dk/wp-content/uploads/2020/03/ny-red.png"" style=""width:200px"" class=""logo"" id:ignore></img>
                                        </div>

                                        <div class=""content px-4"" diff:ignore></div>
                                    </div>";

            // Assert
            cut.MarkupMatches(expectedMarkup);
        }
        #endregion
    }
}
