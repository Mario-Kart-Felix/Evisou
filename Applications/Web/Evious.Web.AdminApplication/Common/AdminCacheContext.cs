using Evious.Core.Cache;
using Evious.Crm.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evious.Web.AdminApplication.Common
{
    public class AdminCacheContext
    {
        public static AdminCacheContext Current
        {
            get
            {
                return CacheHelper.GetItem<AdminCacheContext>();
            }
        }

        public Dictionary<int, City> CityDic
        {
            get
            {
                return CacheHelper.Get<Dictionary<int, City>>("CityDic", () =>
                {
                    return ServiceContext.Current.CrmService.GetCityList().ToDictionary(a => a.ID);
                });
            }
        }

        public Dictionary<int, Area> AreaDic
        {
            get
            {
                return CacheHelper.Get<Dictionary<int, Area>>("AreaDic", () =>
                {
                    return ServiceContext.Current.CrmService.GetAreaList().ToDictionary(a => a.ID);
                });
            }
        }
    }
}