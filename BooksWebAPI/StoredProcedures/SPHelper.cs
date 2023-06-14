using Microsoft.Data.SqlClient;
using Models.ViewModels;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace BooksWebAPI.StoredProcedures
{
    public class SPHelper
    {
        public List<T> CreateListFromTable<T>(DataTable dataTable) where T : new()
        {
            // create a new object
            List<T> values = new List<T>();

            // set the item
            foreach (DataRow row in dataTable.Rows)
            {
                T item = new T();
                SetItemFromRow(item, row);
                values.Add(item);
            }
            return values;
        }

        public void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }

        public string SnakeToPascal(string name)
        {
            var pascalString = name.ToLower().Replace("_", " ");
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            pascalString = info.ToTitleCase(pascalString).Replace(" ", string.Empty);

            return Char.ToLowerInvariant(pascalString[0]) + pascalString.Substring(1);
        }
    }
}
