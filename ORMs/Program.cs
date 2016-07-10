using System;
using System.Data;
using System.Linq;

namespace ORMs {   
    class Program {
        public static void Main(string[] args) {
            WelcomeMessage();

            Console.WriteLine("First Demo uses a DataSet (Table Adapters):");
            Console.WriteLine("Press any key to start the demo.");
            Console.ReadKey();
            ORM_DataSet_TableAdapters_Demo();
            Console.WriteLine("Press any key to go to the next demo.");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Second Demo uses a DataContext:");
            Console.WriteLine("Press any key to start the demo.");
            Console.ReadKey();
            ORM_DataContext_Demo();

            Console.WriteLine("\nPress any key to End the program.");
            Console.ReadKey();
        }

        /// <summary>
        /// Showcases the Dataset (Table Adapter) version of Object Relational Modeling
        /// </summary>
        public static void ORM_DataSet_TableAdapters_Demo() {
            ZeroCoolDatabaseTableAdapters.XSDTableAdapter xsdTableAdapter = new ZeroCoolDatabaseTableAdapters.XSDTableAdapter();

            //Read Database Table Records and Display them.
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
        /// Prints Records in Database Table for Dataset(TableAdapter)
        /// </summary>
        public static void ORM_DataSet_TableAdapters_PrintTable(ZeroCoolDatabaseTableAdapters.XSDTableAdapter tableAdapter)
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
            /* High Level Generic Steps:
             * 1) Create a Database in SQL Server
             * 2) Right-Click Project -> Add New Item -> Add a LinqToSQL Classes "Item" to your Project
             *      A) Add database tables to the designer, by dragging what you want from the sql server object explorer
             * 3) Reading from a Database Table
             *    A) Instantiate your DataContext Object
             *    B) Use LinQ to Aggregate a SQL Result Set from the Database
             *       I) Result set will be stored in an IQueryable collection.
             *          -Collection would stored items of the type of your Database Table
             */

            //Gets all the tables in the Data Context
            ZeroCoolTableClassesDataContext x = new ZeroCoolTableClassesDataContext();

            //Reading Database Table and Printing Records
            Console.WriteLine("Reading Database Table and Printing Records:");
            ORM_DataContext_PrintTable(x);

            Console.WriteLine("Now I will insert a record..\nPrinting Database Table Records now:");
            #region Record Insert
            XSDTable testRecord = new XSDTable();
            testRecord.Firstname = "Joe";
            testRecord.Lastname = "Robert";
            testRecord.Age = 25;

            x.XSDTables.InsertOnSubmit(testRecord);

            try
            {
                x.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("<DataContext>: Unable to Insert Record.");
            }
            #endregion 
            ORM_DataContext_PrintTable(x);

            Console.WriteLine("Now I will Update a record..Can you guess which :)\nPrinting Database Table Records now:");
            
            Console.WriteLine("Now I will Delete a record..Can you guess which :)\nPrinting Database Table Records now:");
            
            
        }

        /// <summary>
        /// Prints Records in Database Table for DataContext
        /// </summary>
        /// <param name="dataContext"></param>
        public static void ORM_DataContext_PrintTable (ZeroCoolTableClassesDataContext dataContext)
        {
            if (dataContext != null)
            {
                var resultSet = from people in dataContext.XSDTables
                                select people;

                foreach (var record in resultSet)
                {
                    Console.WriteLine(String.Format("{0} | {1} | {2}",record.Firstname, record.Lastname, record.Age));
                }
            }
        }

        /// <summary>
        /// Prints Program Intro/Welcome Message(s)
        /// </summary>
        public static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Derrick's Object Relational Mapping Demos...Enjoy :)");
            Console.WriteLine("----------------------------------------------------------------\n");
        }
    }
}
