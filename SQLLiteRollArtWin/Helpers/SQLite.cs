using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SQLLiteRollArtWin.Helpers
{
    public static class SqliteHelper
    {


        ///* =========================================================
        // * OUVERTURE DE CONNEXION
        // * ========================================================= */
        //private static SqliteConnection Open(string dbPath)
        //{
        //    var con = new SqliteConnection($"Data Source={dbPath}");
        //    con.Open();
        //    return con;
        //}

        ///* =========================================================
        // * SELECT (lecture)
        // * ========================================================= */
        //public static List<Dictionary<string, object>> Query(
        //    string dbPath,
        //    string sql,
        //    Dictionary<string, object?>? parameters = null)
        //{
        //    var result = new List<Dictionary<string, object?>>();

        //    using var con = Open(dbPath);
        //    using var cmd = con.CreateCommand();
        //    cmd.CommandText = sql;

        //    AddParameters(cmd, parameters);

        //    using var reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        var row = new Dictionary<string, object?>();

        //        for (int i = 0; i < reader.FieldCount; i++)
        //        {
        //            row[reader.GetName(i)] =
        //                reader.IsDBNull(i) ? null : reader.GetValue(i);
        //        }

        //        result.Add(row);
        //    }

        //    return result;
        //}

        ///* =========================================================
        // * INSERT / UPDATE / DELETE
        // * ========================================================= */
        //public static int Execute(
        //    string dbPath,
        //    string sql,
        //    Dictionary<string, object?>? parameters = null)
        //{
        //    using var con = Open(dbPath);
        //    using var cmd = con.CreateCommand();
        //    cmd.CommandText = sql;

        //    AddParameters(cmd, parameters);

        //    return cmd.ExecuteNonQuery();
        //}

        /// <summary>
        /// Exécute une requête SQL sans paramètres
        /// (CREATE, DROP, UPDATE simple, etc.)
        /// </summary>
        public static int ExecuteRaw(string dbPath, string sql)
        {
            using (var con = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        ///* =========================================================
        // * TRANSACTION (CSV / JSON / batch)
        // * ========================================================= */
        //public static void ExecuteTransaction(
        //    string dbPath,
        //    Action<SqliteCommand> action)
        //{
        //    using var con = Open(dbPath);
        //    using var tx = con.BeginTransaction();
        //    using var cmd = con.CreateCommand();
        //    cmd.Transaction = tx;

        //    action(cmd);

        //    tx.Commit();
        //}

        ///* =========================================================
        // * AJOUT DES PARAMÈTRES
        // * ========================================================= */
        //private static void AddParameters(
        //    SqliteCommand cmd,
        //    Dictionary<string, object?>? parameters)
        //{
        //    if (parameters == null)
        //        return;

        //    foreach (var p in parameters)
        //    {
        //        cmd.Parameters.AddWithValue(
        //            p.Key,
        //            p.Value ?? DBNull.Value);
        //    }
        //}

        public static HashSet<string> GetTableColumns(string dbPath, string tableName)
        {
            var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using (var con = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(
                           $"PRAGMA table_info({QuoteSqliteIdentifier(tableName)});", con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(reader["name"].ToString());
                        }
                    }
                }
            }

            return columns;
        }

        public static void InsertJsonIntoTable(
            string dbPath,
            string tableName,
            JArray jsonArray)
        {
            var tableColumns = GetTableColumns(dbPath, tableName);

            using (var con = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                con.Open();

                using (var tx = con.BeginTransaction())
                using (var cmd = new SQLiteCommand(con))
                {
                    cmd.Transaction = tx;

                    foreach (JObject obj in jsonArray)
                    {
                        var validProps = obj.Properties()
                            .Where(p => tableColumns.Contains(p.Name))
                            .ToList();

                        if (validProps.Count == 0)
                            continue;

                        var columnsSql = string.Join(", ",
                            validProps.Select(p => QuoteSqliteIdentifier(p.Name)));

                        var paramNames = validProps
                            .Select((p, i) => $"@p{i}")
                            .ToList();

                        var paramsSql = string.Join(", ", paramNames);

                        cmd.CommandText =
                            $"INSERT INTO {QuoteSqliteIdentifier(tableName)} ({columnsSql}) VALUES ({paramsSql})";

                        cmd.Parameters.Clear();

                        for (int i = 0; i < validProps.Count; i++)
                        {
                            var token = validProps[i].Value;

                            cmd.Parameters.AddWithValue(
                                paramNames[i],
                                token.Type == JTokenType.Null
                                    ? DBNull.Value
                                    : (object)token.ToString()
                            );
                        }

                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
            }
        }


        /// <summary>
        /// Quote un identifiant SQLite (table/colonne) : gère #, espaces, accents, mots réservés...
        /// </summary>
        private static string QuoteSqliteIdentifier(string identifier)
        {
            // SQLite utilise "..." pour les identifiants. On échappe " par "".
            var safe = identifier.Replace("\"", "\"\"");
            return $"\"{safe}\"";
        }

        public static void ExecuteScript(string dbPath, string sqlScript)
        {
            using (var con = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                con.Open();

                using (var tx = con.BeginTransaction())
                using (var cmd = new SQLiteCommand(sqlScript, con, tx))
                {
                    cmd.ExecuteNonQuery();
                    tx.Commit();
                }
            }
        }

        public static void ExecuteSqlScript(string dbPath, string sqlScript)
        {
            using (var con = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sqlScript, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }



}
}
