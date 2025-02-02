using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace DC.Translator.Tool
{
    public class TranslationRepository
    {
        private readonly string _dbConnString;

        public TranslationRepository(string dbPath)
        {
            _dbConnString = $"Data Source={dbPath}";
        }

        public async Task Initialize()
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand(conn);
            cmd.CommandText = TranslationDbMigration.MigrationScript;
            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<(bool, string)> VerifyDb()
        {
            try
            {
                using var conn = new SQLiteConnection(_dbConnString);
                await conn.OpenAsync();
                using var cmd = new SQLiteCommand(conn);
                cmd.CommandText = "SELECT count(1) FROM sqlite_master WHERE type='table' AND name='static_string_translation';";
                var exists = Convert.ToBoolean(await cmd.ExecuteScalarAsync());
                if (!exists) { return (false, "静态翻译表不存在!"); }
                cmd.CommandText = "SELECT count(1) FROM sqlite_master WHERE type='table' AND name='dynamic_string_translation';";
                exists = Convert.ToBoolean(await cmd.ExecuteScalarAsync());
                if (!exists) { return (false, "动态翻译表不存在!"); }
                return (true, string.Empty);
            }
            catch (SQLiteException)
            {
                return (false, "所选择的文件可能不是有效的sqlite数据库!");
            }
        }

        public async Task<List<StaticTranslationItem>> LoadStatic(string lang, bool toBeTran = false)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand(
                $@"SELECT chinese,{lang},insert_time,id  FROM static_string_translation WHERE 1=1 {(toBeTran ? $"AND ({lang} IS NULL or {lang}='')" : string.Empty)}", conn);
            using var reader = cmd.ExecuteReader();
            var result = new List<StaticTranslationItem>();
            while (await reader.ReadAsync())
            {
                var item = new StaticTranslationItem();
                item.Chinese = reader.GetString(0);
                item.Translation = reader.IsDBNull(1) ? "" : reader.GetString(1);
                item.InsertTime = reader.GetDateTime(2);
                item.Id = reader.GetInt32(3);
                result.Add(item);
            }
            return result;
        }

        public async Task<List<DynamicTranslationItem>> LoadDynamic(string lang, bool toBeTran = false)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand(
                $@"SELECT string_key,chinese,{lang},insert_time,id  FROM dynamic_string_translation WHERE 1=1 {(toBeTran ? $"AND ({lang} IS NULL or {lang}='')" : string.Empty)}", conn);
            using var reader = cmd.ExecuteReader();
            var result = new List<DynamicTranslationItem>();
            while (await reader.ReadAsync())
            {
                var item = new DynamicTranslationItem();
                item.Key = reader.GetString(0);
                item.Chinese = reader.IsDBNull(1) ? "" : reader.GetString(1);
                item.Translation = reader.IsDBNull(2) ? "" : reader.GetString(2);
                item.InsertTime = reader.GetDateTime(3);
                item.Id = reader.GetInt32(4);
                result.Add(item);
            }
            return result;
        }


        public async Task Update(HashSet<(string, bool)> keysInSrcCode)
        {
            var staticList = new HashSet<string>();
            var dynamicList = new HashSet<string>();
            foreach (var (text, isStatic) in keysInSrcCode)
            {
                if (isStatic && text.Length < 15)
                {
                    staticList.Add(text);
                }
                else { dynamicList.Add(text); }
            }

            var staticListInDb = await LoadCurrentItemsFromDb(true);
            staticList.ExceptWith(staticListInDb);
            await BatchInsertStaticKeys(staticList);

            var dynamicListInDb = await LoadCurrentItemsFromDb(false);
            dynamicList.ExceptWith(dynamicListInDb);
            await BatchInsertDynamicKeys(dynamicList);
        }

        private async Task<List<string>> LoadCurrentItemsFromDb(bool isStatic)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand("SELECT chinese FROM static_string_translation", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            var result = new List<string>();
            while (await reader.ReadAsync())
            {
                if (reader.IsDBNull(0)) { continue; }
                result.Add(reader.GetString(0));
            }

            return result;
        }

        private async Task BatchInsertStaticKeys(HashSet<string> keys)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            var tran = await conn.BeginTransactionAsync();
            using var cmd = new SQLiteCommand("INSERT INTO static_string_translation(chinese,insert_time) VALUES (@string_key,datetime('now'))");
            cmd.Connection = conn;
            var @params = new SQLiteParameter("@string_key", string.Empty);
            cmd.Parameters.Add(@params);
            foreach (var key in keys)
            {
                @params.Value = key;
                await cmd.ExecuteNonQueryAsync();
            }
            tran.Commit();
        }

        private async Task BatchInsertDynamicKeys(HashSet<string> chineseLiteralList)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            var tran = await conn.BeginTransactionAsync();
            using var cmd = new SQLiteCommand("INSERT INTO dynamic_string_translation(string_key,chinese,insert_time) VALUES (@string_key,@chinese,datetime('now'))");
            cmd.Connection = conn;
            var keyParam = new SQLiteParameter("@string_key", string.Empty);
            cmd.Parameters.Add(keyParam);
            var chineseParam = new SQLiteParameter("@chinese", string.Empty);
            cmd.Parameters.Add(chineseParam);
            foreach (var chinese in chineseLiteralList)
            {
                keyParam.Value = $"{DateTime.Now:yyyyMMddHHmmssfff}{Random.Shared.Next(10000000)}";
                chineseParam.Value = chinese;
                await cmd.ExecuteNonQueryAsync();
            }
            tran.Commit();
        }

        public async Task DeleteKey(int id, bool isStatic = true)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand(conn);
            cmd.CommandText = $"DELETE FROM {(isStatic ? "static" : "dynamic")}_string_translation WHERE id=@id;";
            cmd.Parameters.Add(new SQLiteParameter("@id", id));
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> AddStaticItem(StaticTranslationItem item, string lang)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand($"INSERT INTO static_string_translation(chinese,{lang},insert_time) VALUES (@chinese,@translation,datetime('now')) returning id");
            cmd.Connection = conn;
            cmd.Parameters.AddRange(new[]
            {
                new SQLiteParameter("@chinese", item.Chinese),
                new SQLiteParameter("@translation", item.Translation)
            });
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task UpdateStaticItem(StaticTranslationItem item, string lang)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand($"UPDATE static_string_translation SET chinese=@chinese,{lang}=@translation,update_time=datetime('now') WHERE id=@id");
            cmd.Connection = conn;
            cmd.Parameters.AddRange(new[]
            {
                new SQLiteParameter("@chinese", item.Chinese),
                new SQLiteParameter("@translation", item.Translation),
                new SQLiteParameter("@id", item.Id)
            });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateDynamicItem(DynamicTranslationItem item, string lang)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand($"UPDATE dynamic_string_translation SET string_key=@string_key, chinese=@chinese,{lang}=@translation,update_time=datetime('now') WHERE id=@id");
            cmd.Connection = conn;
            cmd.Parameters.AddRange(new[]
            {
                new SQLiteParameter("@string_key", item.Key),
                new SQLiteParameter("@chinese", item.Chinese),
                new SQLiteParameter("@translation", item.Translation),
                new SQLiteParameter("@id", item.Id)
            });
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> CheckKeyExists(string key, bool isStatic, bool excludeSelf, int id = 0)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand(isStatic
                ? $"SELECT Count(1) FROM  static_string_translation where chinese = @string_key {(excludeSelf ? "and id<>@id" : string.Empty)}"
                : $"SELECT Count(1) FROM  dynamic_string_translation where string_key = @string_key {(excludeSelf ? "and id<>@id" : string.Empty)}");
            cmd.Connection = conn;
            cmd.Parameters.Add(new SQLiteParameter("@string_key", key));
            cmd.Parameters.Add(new SQLiteParameter("@id", id));
            return Convert.ToBoolean(await cmd.ExecuteScalarAsync());
        }

        public async Task<int> AddDynamicItem(DynamicTranslationItem item, string lang)
        {
            using var conn = new SQLiteConnection(_dbConnString);
            await conn.OpenAsync();
            using var cmd = new SQLiteCommand($"INSERT INTO dynamic_string_translation(string_key,chinese,{lang},insert_time) VALUES (@string_key,@chinese,@translation,datetime('now')) RETURNING id");
            cmd.Connection = conn;
            cmd.Parameters.AddRange(new[]
            {
                new SQLiteParameter("@string_key", item.Key),
                new SQLiteParameter("@chinese", item.Chinese),
                new SQLiteParameter("@translation", item.Translation)
            });
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

    }
}
