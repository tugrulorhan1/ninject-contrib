using LoggingDemo.Migrations;
using RikMigrations;

[assembly: Migration(typeof(M_001_CreateKeyTable), 1, "LoggingDemo.Migrations")]

namespace LoggingDemo.Migrations
{
    public class M_001_CreateKeyTable : IMigration
    {
        #region IMigration Members

        public void Up(DbProvider db)
        {
            if (db.TableExists("KeyTable"))
                db.DropTable("KeyTable");

            var keyTable = db.AddTable("KeyTable");
            keyTable.AddColumn("NextId", typeof(int)).NotNull();
            keyTable.Save();

            db.ExecuteNonQuery("Insert Into KeyTable values (1)");

        }

        public void Down(DbProvider db)
        {
            db.DropTable("KeyTable");
        }

        #endregion
    }

}