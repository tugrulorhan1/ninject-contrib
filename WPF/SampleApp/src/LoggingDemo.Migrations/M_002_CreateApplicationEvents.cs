using System;
using LoggingDemo.Migrations;
using RikMigrations;

[assembly: Migration(typeof(M_002_CreateApplicationEvents), 2, "LoggingDemo.Migrations")]

namespace LoggingDemo.Migrations
{
    public class M_002_CreateApplicationEvents : IMigration
    {
        #region IMigration Members

        public void Up(DbProvider db)
        {
            var table = db.AddTable("ApplicationEvents");
            table.AddColumn("Id", typeof(int)).PrimaryKey();
            table.AddColumn("ApplicationContext", typeof(Guid));
            table.AddColumn("EventTime", typeof(DateTime));
            table.AddColumn("Exception", typeof(string), int.MaxValue);
            table.AddColumn("Level", typeof(string), 256);
            table.AddColumn("LoggerName", typeof(string), int.MaxValue);
            table.AddColumn("Message", typeof(string), int.MaxValue);
            table.AddColumn("Sequence", typeof(int));
            table.AddColumn("StackTrace", typeof(string), int.MaxValue);
            table.AddColumn("UserStackFrame", typeof(string), int.MaxValue);

            table.Save();

        }

        public void Down(DbProvider db)
        {
            db.DropTable("ApplicationEvents");
        }

        #endregion
    }
}