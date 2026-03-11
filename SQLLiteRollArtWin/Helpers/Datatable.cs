using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLLiteRollArtWin.Helpers
{
    internal static class Datatable
    {

        public static void DataTableToCsv(DataTable dt, string filePath, bool includeHeader = true)
        {
            var sb = new StringBuilder();

            // ================================
            // HEADER
            // ================================
            if (includeHeader)
            {
                var columnNames = dt.Columns
                    .Cast<DataColumn>()
                    .Select(c => EscapeCsv(c.ColumnName));

                sb.AppendLine(string.Join(";", columnNames));
            }

            // ================================
            // ROWS
            // ================================
            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field =>
                {
                    if (field == null || field == DBNull.Value)
                        return "";

                    if (field is DateTime dtValue)
                        return EscapeCsv(dtValue.ToString("dd/MM/yyyy"));

                    return EscapeCsv(field.ToString());
                });

                sb.AppendLine(string.Join(";", fields));
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            // Remplace les guillemets par ""
            var escaped = value.Replace("\"", "\"\"");

            // Si contient ; ou retour ligne → on entoure de ""
            if (escaped.Contains(";") || escaped.Contains("\n") || escaped.Contains("\r"))
            {
                return $"\"{escaped}\"";
            }

            return escaped;
        }
    }
}
