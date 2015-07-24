using System;
using System.Collections.Generic;
using Evious.Account.Contract;
using Evious.Core.Cache;
using Evious.Core.Service;
using Evious.Cms.Contract;
using Evious.Crm.Contract;
using Evious.OA.Contract;
using Evious.Ims.Contract;

namespace Evious.Web
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
