using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace Qualia.Umb.Page404
{
    internal class LastChanceFinder : IContentLastChanceFinder
    {
        private const string NOT_FOUND_PAGE_ALIAS = "notFound";

        private readonly IDomainService _domainService;
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public LastChanceFinder(IDomainService domainService, IUmbracoContextAccessor umbracoContextAccessor)
        {
            _domainService = domainService;
            _umbracoContextAccessor = umbracoContextAccessor;
        }

        public bool TryFindContent(IPublishedRequestBuilder contentRequest)
        {
            if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
                return false;

            int rootId = GetRootOfMatchingDomain(contentRequest);
            rootId = rootId > 0 ? rootId : umbracoContext.Content.GetAtRoot().FirstOrDefault().Id;

            var _404_Content = 
                umbracoContext.Content
                .GetByXPath($"//{NOT_FOUND_PAGE_ALIAS}")
                .FirstOrDefault(p => p.Path.Contains(rootId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            if (_404_Content is null)
                return false;

            contentRequest.SetPublishedContent(_404_Content);
            return true;
        }

        private int GetRootOfMatchingDomain(IPublishedRequestBuilder contentRequest)
        {
            var allDomains = _domainService.GetAll(true).ToList();
            var domain = allDomains?
                .FirstOrDefault(f =>
                f.DomainName == contentRequest.Domain.Name
                || f.DomainName == contentRequest.Uri.Authority
                || f.DomainName == $"https://{contentRequest.Uri.Authority}"
                || f.LanguageIsoCode == contentRequest.Culture
                );

            int siteId =
                domain?.RootContentId ??
                allDomains?.FirstOrDefault()?.RootContentId ??
                -1;

            return siteId;
        }
    }
}
