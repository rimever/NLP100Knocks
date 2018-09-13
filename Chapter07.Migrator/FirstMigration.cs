using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Chapter07.Migrator
{
    [Migration(1)]
    public class FirstMigration:Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            IfDatabase("Postgres").Execute.Sql("create extension if not exists hstore");
            Create.Table("artist")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()                
                .WithColumn("kvs").AsCustom("hstore").WithColumnDescription("key value store")
                .WithColumn("json").AsCustom("jsonb").WithColumnDescription("json");
        }

        /// <inheritdoc />
        public override void Down()
        {
        }
    }
}
