using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Mpmt.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static Task<DataTable> ToDataTableAsync<T>(this IEnumerable<T> dataList) where T : class
        {
            return Task.Run(() =>
            {
                var dataTable = new DataTable(typeof(T).Name);
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo prop in properties)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.GetCustomAttribute<DisplayNameAttribute>(false)?.DisplayName ?? prop.Name);
                }

                foreach (T item in dataList)
                {
                    var values = new object[properties.Length];

                    for (int i = 0; i < properties.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = properties[i].GetValue(item, null);
                    }

                    dataTable.Rows.Add(values);
                }

                //put a breakpoint here and check datatable
                return dataTable;
            });
        }

        public static Task<List<DataTable>> ToDataTablesAsync<T>(this IEnumerable<T> dataList, int recordsPerDataTable) where T : class
        {
            return Task.Run(() =>
            {
                var dataTableList = new List<DataTable>();

                var totalRecords = dataList.Count();
                var take = recordsPerDataTable < 1 ? totalRecords : recordsPerDataTable;
                var skip = 0;

                do
                {
                    var currentTableData = dataList.Skip(skip).Take(take);

                    var dataTable = new DataTable(typeof(T).Name);
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (PropertyInfo prop in properties)
                    {
                        //Setting column names as Property names
                        dataTable.Columns.Add(prop.GetCustomAttribute<DisplayNameAttribute>(false)?.DisplayName ?? prop.Name);
                    }

                    foreach (T item in currentTableData)
                    {
                        var values = new object[properties.Length];

                        for (int i = 0; i < properties.Length; i++)
                        {
                            //inserting property values to datatable rows
                            values[i] = properties[i].GetValue(item, null);
                        }

                        dataTable.Rows.Add(values);
                    }

                    dataTableList.Add(dataTable);
                    skip += take;

                } while (skip < totalRecords);

                return dataTableList;
            });
        }
    }
}
