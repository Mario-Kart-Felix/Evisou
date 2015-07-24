namespace Evious.Account.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Url");
        }
    }
}
