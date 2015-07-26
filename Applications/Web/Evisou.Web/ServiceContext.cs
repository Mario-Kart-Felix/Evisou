using System;
using System.Collections.Generic;
using Evisou.Account.Contract;
using Evisou.Core.Cache;
using Evisou.Core.Service;
using Evisou.Cms.Contract;
using Evisou.Crm.Contract;
using Evisou.OA.Contract;
using Evisou.Ims.Contract;

namespace Evisou.Web
{
    public class ServiceContext
    {
        public static ServiceContext Current
        {
            get
            {
                
               
                return CacheHelper.GetItem<ServiceContext>("ServiceContext", () => new ServiceContext());
            }
        }
        
        public IAccountService AccountService
        {
            get
            {
                return ServiceHelper.CreateService<IAccountService>();
            }
        }

        public ICmsService CmsService
        {
            get
            {
                return ServiceHelper.CreateService<ICmsService>();
            }
        }

        public ICrmService CrmService
        {
            get
            {
                return ServiceHelper.CreateService<ICrmService>();
            }
        }

        public IOAService OAService
        {
            get
            {
                return ServiceHelper.CreateService<IOAService>();
            }
        }

        public IImsService ImsService
        {
            get
            {
                return ServiceHelper.CreateService<IImsService>();
            }
        
        }
    }
}
