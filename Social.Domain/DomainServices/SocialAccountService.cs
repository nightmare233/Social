﻿using Framework.Core;
using Social.Domain.Entities;
using Social.Infrastructure;
using Social.Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Social.Domain.DomainServices
{
    public interface ISocialAccountService : IDomainService<SocialAccount>
    {
        Task<SocialAccount> GetAccountAsync(SocialUserType type, string originalId);
    }

    public class SocialAccountService : DomainService<SocialAccount>, ISocialAccountService
    {
        IRepository<GeneralDataContext, SiteSocialAccount> _siteSocialAccountRepo;

        public SocialAccountService(IRepository<GeneralDataContext, SiteSocialAccount> siteSocialAccountRepo)
        {
            _siteSocialAccountRepo = siteSocialAccountRepo;
        }

        public async Task<SocialAccount> GetAccountAsync(SocialUserType type, string originalId)
        {
            return await Repository.FindAll().Include(t => t.SocialUser).Where(t => t.SocialUser.OriginalId == originalId && t.SocialUser.Type == type && t.IfEnable).FirstOrDefaultAsync();
        }

        public async override Task<SocialAccount> InsertAsync(SocialAccount entity)
        {
            if (IsDupliated(entity))
            {
                throw ExceptionHelper.BadReqeust($"'{entity.SocialUser.Name}' has already been added.");
            }

            ApplyDefaultValue(entity);
            base.Insert(entity);

            await InsertSocialAccountInGeneralDb(entity);

            return entity;
        }

        private async Task InsertSocialAccountInGeneralDb(SocialAccount entity)
        {
            int? siteIdOrNull = CurrentUnitOfWork.GetSiteId();
            if (siteIdOrNull == null)
            {
                throw new InvalidOperationException("site id must have value.");
            }

            int siteId = siteIdOrNull.Value;
            if (entity.SocialUser.Type == SocialUserType.Facebook)
            {
                await UnitOfWorkManager.Run(TransactionScopeOption.Required, null, async () =>
                {
                    if (!_siteSocialAccountRepo.FindAll().Any(t => t.SiteId == siteId && t.FacebookPageId == entity.SocialUser.OriginalId))
                    {
                        await _siteSocialAccountRepo.InsertAsync(new SiteSocialAccount { SiteId = siteId, FacebookPageId = entity.SocialUser.OriginalId });
                    }
                });
            }

            if (entity.SocialUser.Type == SocialUserType.Twitter)
            {
                await UnitOfWorkManager.Run(TransactionScopeOption.Required, null, async () =>
                {
                    if (!_siteSocialAccountRepo.FindAll().Any(t => t.SiteId == siteId && t.TwitterUserId == entity.SocialUser.OriginalId))
                    {
                        await _siteSocialAccountRepo.InsertAsync(new SiteSocialAccount { SiteId = siteId, TwitterUserId = entity.SocialUser.OriginalId });
                    }
                });
            }
        }

        public async override Task DeleteAsync(SocialAccount entity)
        {
            await base.DeleteAsync(entity);
            await DeleteSocialAccountInGeneralDb(entity);
        }

        private async Task DeleteSocialAccountInGeneralDb(SocialAccount entity)
        {
            int? siteIdOrNull = CurrentUnitOfWork.GetSiteId();
            if (siteIdOrNull == null)
            {
                throw new InvalidOperationException("site id must have value.");
            }

            int siteId = siteIdOrNull.Value;
            if (entity.SocialUser.Type == SocialUserType.Facebook)
            {
                await UnitOfWorkManager.Run(TransactionScopeOption.Required, null, async () =>
                {
                    var accoutns = _siteSocialAccountRepo.FindAll().Where(t => t.SiteId == siteId && t.FacebookPageId == entity.SocialUser.OriginalId).ToList();
                    foreach (var account in accoutns)
                    {
                        await _siteSocialAccountRepo.DeleteAsync(account);
                    }
                });
            }

            if (entity.SocialUser.Type == SocialUserType.Twitter)
            {
                await UnitOfWorkManager.Run(TransactionScopeOption.Required, null, async () =>
                {
                    var accoutns = _siteSocialAccountRepo.FindAll().Where(t => t.SiteId == siteId && t.TwitterUserId == entity.SocialUser.OriginalId).ToList();
                    foreach (var account in accoutns)
                    {
                        await _siteSocialAccountRepo.DeleteAsync(account);
                    }
                });
            }
        }

        public override IQueryable<SocialAccount> FindAll()
        {
            return Repository.FindAll().Include(t => t.SocialUser).Where(t => t.IsDeleted == false);
        }

        public override SocialAccount Find(int id)
        {
            return Repository.FindAll().Include(t => t.SocialUser).Where(t => t.IsDeleted == false).FirstOrDefault(t => t.Id == id);
        }

        private void ApplyDefaultValue(SocialAccount entity)
        {
            entity.IfEnable = true;
            if (entity.SocialUser.Type == SocialUserType.Facebook)
            {
                entity.IfConvertMessageToConversation = true;
                entity.IfConvertVisitorPostToConversation = true;
                entity.IfConvertWallPostToConversation = true;
            }
            if (entity.SocialUser.Type == SocialUserType.Twitter)
            {
                entity.IfConvertMessageToConversation = true;
                entity.IfConvertTweetToConversation = true;
            }
        }

        private bool IsDupliated(SocialAccount entity)
        {
            if (entity.SocialUser.Type == SocialUserType.Facebook)
            {
                return Repository.FindAll().Any(t => t.IsDeleted == false && t.SocialUser.OriginalId == entity.SocialUser.OriginalId && entity.SocialUser.Type == SocialUserType.Facebook);
            }

            if (entity.SocialUser.Type == SocialUserType.Twitter)
            {
                return Repository.FindAll().Any(t => t.IsDeleted == false && t.SocialUser.OriginalId == entity.SocialUser.OriginalId && entity.SocialUser.Type == SocialUserType.Twitter);
            }

            return false;
        }
    }
}
