using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Translator
{
    public class TranslationDbMigration : Common2.SqliteDBMigrateBase
    {
        public static readonly string MigrationScript =
$@"CREATE TABLE IF NOT EXISTS static_string_translation(
	id integer primary key AUTOINCREMENT,
	chinese varchar(500) not null,
	english varchar(500) null,
	german varchar(500) null,
	russian varchar(500) null,
	korean varchar(500) null,
	japanese varchar(500) null,
	french varchar(500) null,
	italian varchar(500) null,
	vietnamese varchar(500) null,
	portuguese varchar(500) null,
	spainish varchar(500) null,
	traditional_chinese varchar(500) null,
	last_used_time datetime null,
	description varchar(500) null,
	insert_time datetime not null,
	update_time datetime null
);

CREATE UNIQUE INDEX IF NOT EXISTS sst_unique_chinese
ON static_string_translation(chinese);

CREATE TABLE IF NOT EXISTS dynamic_string_translation(
	id integer primary key AUTOINCREMENT,
	string_key varchar(50) not null,
	chinese varchar(1024) null,
	english varchar(1024) null,
	german varchar(1024) null,
	russian varchar(1024) null,
	korean varchar(1024) null,
	japanese varchar(1024) null,
	french varchar(1024) null,
	italian varchar(1024) null,
	vietnamese varchar(1024) null,
	portuguese varchar(1024) null,
	spainish varchar(1024) null,
	traditional_chinese varchar(1024) null,
	last_used_time datetime null,
	description varchar(1024) null,
	insert_time datetime not null,
	update_time datetime null
);

CREATE UNIQUE INDEX IF NOT EXISTS dst_unique_key
ON dynamic_string_translation(string_key);
";

        public TranslationDbMigration(ILogger logger, string dbConnString)
            : base(logger, dbConnString)
        {
            AddMigration(new Common2.Migrate(1, MigrationScript, "初始化表"));
        }

        public override int VersionNow => 1;
    }
}
