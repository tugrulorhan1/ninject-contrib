using System;
using LoggingDemo.Migrations;
using RikMigrations;

[assembly: Migration(typeof(M_003_CreateBlog), 3, "LoggingDemo.Migrations")]

namespace LoggingDemo.Migrations
{
    public class M_003_CreateBlog : IMigration
    {
        #region IMigration Members

        public void Up(DbProvider db)
        {
            var table = db.AddTable("Blogs");
            table.AddColumn("Id", typeof(int)).PrimaryKey();
            table.AddColumn("Name", typeof(string), 50).NotNull().Unique();
            table.AddColumn("LockVersion", typeof(int)).NotNull().Default(0);
            table.AddColumn("CreatedOn", typeof(DateTime));
            table.AddColumn("UpdatedOn", typeof(DateTime));
            table.AddColumn("DeletedOn", typeof(DateTime));

            table.Save();
        }

        public void Down(DbProvider db)
        {
            db.DropTable("Blogs"); 
        }

        #endregion
    }
}