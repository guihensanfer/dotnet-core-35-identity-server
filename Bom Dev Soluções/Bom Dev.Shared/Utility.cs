using ClosedXML.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Data
{
    public class Utility
    {
        public const string ProjectViewName = "Bomdev";        

        public static bool IsValidJson(string input)
        {
            try
            {
                JToken.Parse(input);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }        

        public static async Task ExportDataTableToExcelAsync(DataTable dataTable)
        {
            await Task.Run(() =>
            {                
                if (dataTable == null)
                    throw new ArgumentException("DataTable is invalid");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Plan1");

                    // Escrever os nomes das colunas
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dataTable.Columns[i].ColumnName;
                    }

                    // Escrever os dados
                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            worksheet.Cell(row + 2, col + 1).Value = Convert.ToString(dataTable.Rows[row][col]);
                        }
                    }

                    // AutoAjustar as colunas
                    worksheet.Columns().AdjustToContents();

                    string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    string filePathResult = null;
                    const string format = ".xlsx";
                    string fileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("d", CultureInfo.InvariantCulture);

                    fileName = RemoveSpecialCharactersForIO(fileName);
                    if (fileName.EndsWith(format))
                    {
                        filePathResult = Path.Combine(downloadsPath, fileName);
                    }
                    else
                    {
                        filePathResult = Path.Combine(downloadsPath, fileName + format);
                    }                    

                    // Salvar o arquivo Excel
                    workbook.SaveAs(filePathResult);
                }
            });
        }

        public static string RemoveSpecialCharactersForIO(string input)
        {
            // Define a lista de caracteres especiais que devem ser removidos
            char[] specialChars = { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '[', ']', '|', '\\', ':', ';', '\'', '"', '<', '>', ',', '.', '/', '?' };

            // Remove os caracteres especiais da string
            string result = new string(input.Where(c => !specialChars.Contains(c)).ToArray());

            return result;
        }


        public static DataTable ConvertListToDataTable<T>(List<T> list)
        {
            DataTable dataTable = new DataTable();

            PropertyInfo[] properties = typeof(T).GetProperties();

            // Adicionar colunas ao DataTable
            foreach (PropertyInfo property in properties)
            {
                Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                dataTable.Columns.Add(property.Name, propertyType);
            }

            // Adicionar linhas ao DataTable
            foreach (T item in list)
            {
                DataRow row = dataTable.NewRow();

                foreach (PropertyInfo property in properties)
                {
                    Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    object value = property.GetValue(item);

                    if (value != null)
                    {
                        row[property.Name] = Convert.ChangeType(value, propertyType);
                    }
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }    
}
