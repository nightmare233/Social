﻿using Framework.Core;
using Social.Domain.DomainServices;
using Social.Domain.DomainServices.Facebook;
using Social.Domain.Entities;
using Social.Infrastructure.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.AppServices
{
    public interface IFacebookAppService
    {
        Task ProcessWebHookData(FbHookData fbData);
        Task PullTaggedVisitorPosts(SocialAccount socialAccount);
        Task PullVisitorPostsFromFeed(SocialAccount socialAccount);
        Task PullMassagesJob(SocialAccount socialAccount);
    }

    public class FacebookAppService : AppService, IFacebookAppService
    {
        private IWebHookService _facebookWebHookService;
        private IVisitorPostService _visitorPostService;

        public FacebookAppService(
            IWebHookService facebookWebHookService,
            IVisitorPostService visitorPostService
            )
        {
            _facebookWebHookService = facebookWebHookService;
            _visitorPostService = visitorPostService;
        }

        public async Task ProcessWebHookData(FbHookData fbData)
        {
            await _facebookWebHookService.ProcessWebHookData(fbData);
        }

        public async Task PullMassagesJob(SocialAccount socialAccount)
        {
            await _visitorPostService.PullMassagesJob(socialAccount);
        }

        public async Task PullTaggedVisitorPosts(SocialAccount socialAccount)
        {
            await _visitorPostService.PullTaggedVisitorPosts(socialAccount);
        }

        public async Task PullVisitorPostsFromFeed(SocialAccount socialAccount)
        {
            await _visitorPostService.PullVisitorPostsFromFeed(socialAccount);
        }
    }
}
