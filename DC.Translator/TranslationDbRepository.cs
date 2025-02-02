using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Translator
{
    public class TranslationDbRepository
    {
        public const string dbFileName = "..\\settings\\translation.db";
        public static readonly string dbConnString = $"Data Source={dbFileName};Version=3;";
        private readonly Dictionary<string, string> _transDict = new Dictionary<string, string>(1024);
        private readonly Dictionary<LanguageType, string> _columnNameMapping = new Dictionary<LanguageType, string>()
        {
            [LanguageType.Chinese] = "chinese",
            [LanguageType.English] = "english",
            [LanguageType.German] = "german",
            [LanguageType.Vietnam] = "vietnamese",
            [LanguageType.Italian] = "italian",
            [LanguageType.French] = "french",
            [LanguageType.Japanese] = "japanese",
            [LanguageType.Korean] = "korean",
            [LanguageType.Portugal] = "portuguese",
            [LanguageType.Rassian] = "russian",
            [LanguageType.Spain] = "spainish",
            [LanguageType.TraditionalChinese] = "tradditional"
        };
        private DateTime _lastModifiedTime;
        private LanguageType _srcLang;
        private LanguageType _dstLang;

        public TranslationDbRepository()
        {
            var file = new FileInfo(dbFileName);
            _lastModifiedTime = file.LastWriteTime;
        }

        private bool HasDbChanged()
        {
            var file = new FileInfo(dbFileName);
            if (_lastModifiedTime != file.LastWriteTime)
            {
                _lastModifiedTime = file.LastWriteTime;
                return true;
            }
            return false;
        }


        public IReadOnlyDictionary<string, string> LoadStatic(LanguageType srcLang, LanguageType dstLang)
        {
            if (!HasDbChanged() && _srcLang == srcLang && _dstLang == dstLang)
            {
                return _transDict;
            }

            _srcLang = srcLang;
            _dstLang = dstLang;
            _transDict.Clear();
            LoadFromDb();

            return _transDict;

            void LoadFromDb()
            {
                var conn = new SQLiteConnection(dbConnString);
                SQLiteDataReader reader = null;
                try
                {
                    conn.Open();
                    var cmd = new SQLiteCommand($@"SELECT {_columnNameMapping[srcLang]},{_columnNameMapping[dstLang]}  FROM static_string_translation", conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var key = reader.GetString(0);
                        var value = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        _transDict.Add(key, value);
                    }
                    reader.Close();
                }
                finally
                {
                    reader?.Close();
                    conn.Dispose();
                }
            }
        }

        public (bool, string) FindDynamic(string key, LanguageType lang)
        {
            var conn = new SQLiteConnection(dbConnString);
            SQLiteDataReader reader = null;
            SQLiteCommand cmd = null;
            try
            {
                conn.Open();
                cmd = new SQLiteCommand($@"SELECT {_columnNameMapping[lang]} FROM dynamic_string_translation WHERE string_key=@string_key", conn);
                cmd.Parameters.Add(new SQLiteParameter("@string_key", key));
                reader = cmd.ExecuteReader();
                if (reader.NextResult()) { return (true, reader.IsDBNull(0) ? string.Empty : reader.GetString(0)); }
                return (false, string.Empty);
            }
            finally
            {
                cmd?.Dispose();
                reader?.Close();
                conn.Dispose();
            }
        }

        public void AddStaticKey(IEnumerable<string> keyList, LanguageType lang)
        {
            if (keyList == null || !keyList.Any()) { return; }
            var conn = new SQLiteConnection(dbConnString);
            SQLiteCommand cmd = null;
            try
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                cmd = new SQLiteCommand($@"INSERT INTO static_string_translation ({_columnNameMapping[lang]}, insert_time) VALUES (@string_key, datetime('now'))", conn);
                var @param = new SQLiteParameter("@string_key", "");
                cmd.Parameters.Add(param);
                foreach (var key in keyList)
                {
                    param.Value = key;
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
            }
            finally
            {
                cmd?.Dispose();
                conn.Dispose();
            }
        }

        public void AddDynamicKey(string key)
        {
            var conn = new SQLiteConnection(dbConnString);
            SQLiteCommand cmd = null;
            try
            {
                conn.Open();
                cmd = new SQLiteCommand($@"INSERT INTO dynamic_string_translation (string_key, insert_time) VALUES (@string_key, datetime('now'))", conn);
                cmd.Parameters.Add(new SQLiteParameter("@string_key", key));
                cmd.ExecuteNonQuery();
            }
            finally
            {
                cmd?.Dispose();
                conn.Dispose();
            }
        }

        public (bool, string) FindStatic(string key, LanguageType srcLang, LanguageType dstLang)
        {
            var conn = new SQLiteConnection(dbConnString);
            SQLiteDataReader reader = null;
            SQLiteCommand cmd = null;
            try
            {
                conn.Open();
                if (!_columnNameMapping.ContainsKey(dstLang) || !_columnNameMapping.ContainsKey(srcLang))
                {
                    throw new KeyNotFoundException("The specified language type is not found in the column name mapping.");
                }
                cmd = new SQLiteCommand($@"SELECT {_columnNameMapping[dstLang]} FROM static_string_translation WHERE {_columnNameMapping[srcLang]}=@string_key", conn);
                cmd.Parameters.Add(new SQLiteParameter("@string_key", key));
                reader = cmd.ExecuteReader();
                if (reader.Read()) { return (true, reader.IsDBNull(0) ? string.Empty : reader.GetString(0)); }
                return (false, string.Empty);
            }
            finally
            {
                cmd?.Dispose();
                reader?.Close();
                conn.Dispose();
            }
        }
    }
}
