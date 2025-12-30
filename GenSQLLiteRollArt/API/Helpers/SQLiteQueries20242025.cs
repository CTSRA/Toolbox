namespace SQLLiteRollArtAPI.API.Helpers
{
    public static class SQLiteQueries20242025
    {
        public static class SpecialiteMapper
        {
            public static int Map(string type)
            {
                return type.ToUpper() switch
                {
                    "PRECISION" => 10,
                    "DANSE" => 5,
                    "SOLO DANCE" => 6,
                    "LIBRE" => 1,
                    _ => throw new Exception($"Spécialité inconnue : {type}")
                };
            }
        }


        public static void CreateCompetition(
            string dbPath,
            string name,
            string place,
            string nation,
            DateTime date,
            DateTime dateEnd,
            string competitionType,
            string statusEvent)
        {
            var query =
                "INSERT INTO Competitions " +
                "(Name, Place, Nation, Date, DateEnd, CompetitionType, StatusEvent) " +
                "VALUES ($Name, $Place, $Nation, $Date, $DateEnd, $CompetitionType, $StatusEvent)";

            var parameters = new Dictionary<string, object?>
            {
                ["$Name"] = name,
                ["$Place"] = place,
                ["$Nation"] = nation,
                ["$Date"] = date,
                ["$DateEnd"] = dateEnd,
                ["$CompetitionType"] = competitionType,
                ["$StatusEvent"] = statusEvent
            };

            Helpers.SqliteHelper.Execute(dbPath, query, parameters);
        }


        public static void CreateAthlete(
            string dbPath,
            string name,
            string societe,
            string country,
            string region,
            int idSpecialita,
            string licence)
        {
            var sql = """
                      INSERT OR IGNORE INTO Athletes
                      (Name, Societa, Country, Region, ID_Specialita, Num_Licence)
                      VALUES
                      ($Name, $Societa, $Country, $Region, $ID_Specialita, $Num_Licence)
                      """;

            var p = new Dictionary<string, object?>
            {
                ["$Name"] = name,
                ["$Societa"] = societe,
                ["$Country"] = country,
                ["$Region"] = region,
                ["$ID_Specialita"] = idSpecialita,
                ["$Num_Licence"] = licence
            };

            SqliteHelper.Execute(dbPath, sql, p);
        }

        public static void CreateGaraParams(
            string dbPath,
            string name,
            string sex,
            int idSpecialita,
            int idCategory,
            int idCompetition)
        {
            var sql = """
                      INSERT OR IGNORE INTO GaraParams
                      (Name, Sex, ID_Specialita, ID_Category, ID_Competition, Completed)
                      VALUES
                      ($Name, $Sex, $ID_Specialita, $ID_Category, $ID_Competition, 'N')
                      """;

            var p = new Dictionary<string, object?>
            {
                ["$Name"] = name,
                ["$Sex"] = sex,
                ["$ID_Specialita"] = idSpecialita,
                ["$ID_Category"] = idCategory,
                ["$ID_Competition"] = idCompetition
            };

            SqliteHelper.Execute(dbPath, sql, p);
        }

        public static void CreateParticipant(
            string dbPath,
            int idAthlete,
            int idGaraParams,
            int idSegment)
        {
            var sql = """
                      INSERT OR IGNORE INTO Participants
                      (ID_Atleta, ID_GaraParams, ID_Segment)
                      VALUES
                      ($ID_Atleta, $ID_GaraParams, $ID_Segment)
                      """;

            var p = new Dictionary<string, object?>
            {
                ["$ID_Atleta"] = idAthlete,
                ["$ID_GaraParams"] = idGaraParams,
                ["$ID_Segment"] = idSegment
            };

            SqliteHelper.Execute(dbPath, sql, p);
        }

    }
}
