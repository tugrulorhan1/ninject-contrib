using System;
using LoggingDemo.Migrations;
using RikMigrations;

[assembly: Migration(typeof(M_004_CreatePost), 4, "LoggingDemo.Migrations")]

namespace LoggingDemo.Migrations
{
    public class M_004_CreatePost : IMigration
    {
        #region IMigration Members

        public void Up(DbProvider db)
        {
            var table = db.AddTable("Posts");
            table.AddColumn("Id", typeof(int)).PrimaryKey();
            table.AddColumn("BlogId", typeof (int)).NotNull().References("Blogs", "Id");
            table.AddColumn("Title", typeof(string), 255).NotNull();
            table.AddColumn("Description", typeof(string), int.MaxValue).NotNull();
            table.AddColumn("PostDate", typeof (DateTime)).NotNull();
            table.AddColumn("LockVersion", typeof (int)).NotNull().Default(0);
            table.AddColumn("CreatedOn", typeof (DateTime));
            table.AddColumn("UpdatedOn", typeof(DateTime));
            table.AddColumn("DeletedOn", typeof(DateTime));

            table.Save();
        }

        public void Down(DbProvider db)
        {
            db.DropTable("Posts");
        }

        #endregion
    }
}