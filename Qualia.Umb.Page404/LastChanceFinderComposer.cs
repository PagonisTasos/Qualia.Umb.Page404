using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace Qualia.Umb.Page404
{
    public class LastChanceFinderComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddUnique<IContentLastChanceFinder, LastChanceFinder>();
        }
    }
}
