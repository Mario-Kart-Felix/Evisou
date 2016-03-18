using Evisou.Core.Config;
using Evisou.Framework.Contract;
using Evisou.Framework.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Core.Log
{
    [Table("AuditLog")]
    public class AuditLog : ModelBase
    {
        public int ModelId { get; set; }
        public string UserName { get; set; }
        public string ModuleName { get; set; }
        public string TableName { get; set; }
        public string EventType { get; set; }
        public string NewValues { get; set; }
    }

    public class LogDbContext : DbContextBase, IAuditable
    {
        public LogDbContext()
            : base(CachedConfigContext.Current.DaoConfig.Log)
          //  : base("Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-WebApplication2-20150222121646.mdf;Initial Catalog=aspnet-WebApplication2-20150222121646;Integrated Security=True")
        {
            Database.SetInitializer<LogDbContext>(null);
        }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public void WriteLog(int modelId, string userName, string moduleName, string tableName, string eventType, ModelBase newValues)
        {
            this.AuditLogs.Add(new AuditLog()
            {
                ModelId = modelId,
                UserName = userName,
                ModuleName = moduleName,
                TableName = tableName,
                EventType = eventType,
                NewValues = JsonConvert.SerializeObject(newValues, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
            });

            this.SaveChanges();
            this.Dispose();
        }
    }
}
