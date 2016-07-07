/*Author: Derrick Ward*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.Data.Linq.SqlClient;

namespace ORMs
{
    class Program
    {
        static void Main(string[] args)
        {
            

            Console.WriteLine("This project will showcase the different ways Derrick knows how to do Object Relational Model.\n\n");
            Console.WriteLine("First Demo: Using a Dataset -> Table Adapters\n\n");
            ORM_DataSet_TableAdapters_Demo(); //Runs Demo

            Console.WriteLine("Press Any Key to move on to the next demo.");
            Console.ReadKey();

            Console.Clear();
            Console.WriteLine("Second Demo: Using a DataContext\n\n");

            Console.WriteLine("Press any key to end the program.");
            Console.ReadKey();
        }

        /// <summary>
        /// Showcases the Dataset version of Object Relational Modeling, using Table Adapters
        /// </summary>
        public static void ORM_DataSet_TableAdapters_Demo()
        {
            /* High Level Generic Steps:
             * 1) Create a Database in SQL Server
             * 2) Right-Click Project -> Add New Item -> Add a Dataset "Item" to your Project
             *      A) Add database tables to the dataset. Feel free to create different table adapter as well.
             * 3) Reading Database Table: 
             *      A) Instantiate the table adapter you want, in code. 
             *      B)Call "your method" to get data from your table adapter and store it in a DataTable object.
             *      C)Iterate through the rows in your DataTable object and read your data. -> Foreach loop
             */

            //Reading Database Table and Printing Records
            Console.WriteLine("Reading Database Table and Printing Records:");
            ZeroCoolDatabaseTableAdapters.XSDTableTableAdapter xsdTableAdapter = new ZeroCoolDatabaseTableAdapters.XSDTableTableAdapter();
            ORM_DataSet_TableAdapters_PrintTable(xsdTableAdapter);

            Console.WriteLine("Now I will insert a record..\nPrinting Database Table Records now:");
            //Insert Database Table Record
            xsdTableAdapter.Insert("Kayla", "Ward", 19);
            ORM_DataSet_TableAdapters_PrintTable(xsdTableAdapter);

            Console.WriteLine("Now I will Update a record..Can you guess which :)\nPrinting Database Table Records now:");
            //Update Database Table
            DataTable resultSet = xsdTableAdapter.GetDataByPrim((long)xsdTableAdapter.GetMaxPrim());
            DataRow row = resultSet == null ? null : resultSet.Rows[0];
            if (row != null)
            {
                row["firstName"] += "Updated";
                xsdTableAdapter.Update(row);
            }
            ORM_DataSet_TableAdapters_PrintTable(xsdTableAdapter);

            Console.WriteLine("Now I will Delete a record..Can you guess which :)\nPrinting Database Table Records now:");
            //Delete Records from Databae Table
            xsdTableAdapter.Delete((long)row["Prim"]);
            ORM_DataSet_TableAdapters_PrintTable(xsdTableAdapter);
        }

        /// <summary>
        /// Prints Records in Database Table
        /// </summary>
        public static void ORM_DataSet_TableAdapters_PrintTable(ZeroCoolDatabaseTableAdapters.XSDTableTableAdapter tableAdapter)
        {
            DataTable resultSet = tableAdapter.GetData();
            foreach (DataRow row in resultSet.Rows)
            {
                Console.WriteLine(String.Format("Firstname: {0}\nLastname:{1}\nAge: {2}\n------------", row["Firstname"], row["Lastname"], row["Age"]));
                Console.WriteLine("\n");
            }
        }

        /// <summary>
        /// Showcases the Datacontext version of Object Relational Modeling
        /// </summary>
        public static void ORM_DataContext_Demo()
        {
            /*
             *
             */
            
            //Gets all the tables in the Data Context
            ZeroCoolTableClassesDataContext x = new ZeroCoolTableClassesDataContext();

            //Reading all the Records from one of the Database Tables
            IQueryable<XSDTable> queryresult = from people in x.XSDTables                                               
                                               select people;

            foreach (XSDTable record in queryresult)
            {
                Console.WriteLine(String.Format("Firstname: {0}  Lastname: {1}  Age: {2}", record.Firstname??"", record.Lastname??"", record.Age==null ? "" : record.Age.ToString()));
            }

            //Query Database Table, using SQL Methods
            queryresult = from people in x.XSDTables
                          where SqlMethods.Like(people.Firstname, "%r%")
                          select people;

            Console.WriteLine("The number of people with a 'r' in their first name are: " + queryresult.Count());
        }
    }
}
