﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace AppBulkCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dt = new DataTable();
            string line = null;
            int i = 0;

            using (StreamReader sr = File.OpenText(@"D:\table1.txt"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    if (data.Length > 0)
                    {
                        if (i == 0)
                        {
                            foreach (var item in data)
                            {
                                dt.Columns.Add(new DataColumn());
                            }
                            i++;
                        }
                        DataRow row = dt.NewRow();
                        row.ItemArray = data;
                        dt.Rows.Add(row);
                    }
                }
            }

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cadenaConexion"].ConnectionString))
            {
                cn.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(cn))
                {
                    copy.ColumnMappings.Add(0, 0);
                    copy.ColumnMappings.Add(1, 1);
                    copy.ColumnMappings.Add(2, 2);
                    copy.ColumnMappings.Add(3, 3);
                    copy.ColumnMappings.Add(4, 4);
                    copy.DestinationTableName = "usuarios";
                    copy.WriteToServer(dt);
                }
            }
        }
    }
}
