using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Diagnostics;

namespace ORMs {   
    class Program {
        public static void Main(string[] args) {
            ResetDatabaseTable(); //Reset Database Records

            WelcomeMessage();
            Console.WriteLine("Are you ready to begin the demos ? \nPress any key to continue...");
            Console.ReadKey(true);
            Console.Clear();

            //#region Demo 1: ADO.NET -> Table Adapters

            //Console.WriteLine("First Demo: \n{0}\n{1}\n{2}\n{3}"
            //    ,"-Uses the ADO.NET Framework."
            //    ,"-First: Add a DataSet to your project. This will result in a .xsd file."
            //    ,"-Second: From the Designer view, of the DataSet, add a TableAdapter."
            //    ,"-Using this table adapter we will perfrom CRUD(Create, Read, Update, Delete) \nOperations on a Database Table");
            //Console.WriteLine("\nPress any key to continue...");
            //Console.ReadKey(true);
            //ORM_DataSet_TableAdapters_Demo();
            //ClearAndDisplayMessage("Ready for the Next Demo?");
            //Console.Clear();

            //#endregion

            //ResetDatabaseTable(); //Reset Database Records

            //#region Demo 2: DataContext -> LINQ to SQL 

            //Console.WriteLine("Second Demo: \n{0}\n{1}\n{2}\n{3}\n{4}"
            //    , "-Uses the \"LINQ to SQL\" Framework. This Framework is actually built on top of\n the ADO.Net Framework"
            //    , "-First: Add \"LINQ to SQL Classes\" to your project. This will result in a \n.dbml file."
            //    , "-Second: From the Designer view, of the DataContext (.dbml file), drag in a \nDatabase Table from the Server Explorer (View->Server Explorer)."
            //    , "-Classes the represents the DataContext (.dbml file) and the Database table \nwill now exist for you to use."
            //    ," -We will use those classes to perform CRUD (Create, Read, Update, Delete) \nOperations");
            //Console.WriteLine("\nPress any key to start the demo.");
            //Console.ReadKey(true);
            //ORM_DataContext_Demo();
            //ClearAndDisplayMessage("Ready for the Next Demo?");
            //Console.Clear();

            //#endregion

            ResetDatabaseTable(); //Reset Database Records

            #region Demo 3: ADO>NET -> SQLClient 

            Console.WriteLine("Third Demo: \n{0}\n{1}"
                , "-Uses the ADO.NET Framework"
                , "-There are no items(files) we need to add to the project to perform \nCRUD (Create, Read, Update, Delete) Operations");
            Console.WriteLine("\nPress any key to start the demo.");
            Console.ReadKey(true);
            ORM_SQLClient_Demo();

            #endregion

            ClearAndDisplayMessage("Demonstration Complete. Hope you Enjoyed :)");

            ResetDatabaseTable(); //Reset Database Records
        }

        /// <summary>
        /// Showcases the Dataset (Table Adapter) version of Object Relational Mapping
        /// </summary>
        public static void ORM_DataSet_TableAdapters_Demo() {

            using (ZeroCoolDatabaseTableAdapters.XSDTableAdapter xsdTableAdapter = new ZeroCoolDatabaseTableAdapters.XSDTableAdapter())
            {
                ClearAndDisplayMessage("Reading all records from the database and showing them here:");
                //Read Database Table Records and Display them.
                ORM_DataSet_TableAdapters_PrintTable(xsdTableAdapter);
                ContinueOn();

                ClearAndDisplayMessage("Now I will insert a record..\nPrinting Database Table Records now:");
                //Insert Database Table Record
                xsdTableAdapter.Insert("Kayla", "Ward", 19);
                ORM_DataSet_TableAdapters_PrintTable(xsdTableAdapter);
                ContinueOn();

                ClearAndDisplayMessage("Now I will Update a record..Can you guess which :)\nPrinting Database Table Records now:");
                //Update Database Table
                DataTable resultSet = xsdTableAdapter.GetDataByPrim((long)xsdTableAdapter.GetMaxPrim());
                DataRow row = resultSet.Rows[0];
                if (row != null)
                {
                    row["firstName"] += "Updated";
                    xsdTableAdapter.Update(row);
                }
                ORM_DataSet_TableAdapters_PrintTable(xsdTableAdapter);
                ContinueOn();

                ClearAndDisplayMessage("Now I will Delete a record..Can you guess which :)\nPrinting Database Table Records now:");
                //Delete Records from Databae Table
                xsdTableAdapter.Delete((long)row["Prim"]);
                ORM_DataSet_TableAdapters_PrintTable(xsdTableAdapter);
                ContinueOn();

            }
        }

        /// <summary>
        /// Prints Records in Database Table for Dataset(TableAdapter)
        /// </summary>
        public static void ORM_DataSet_TableAdapters_PrintTable(ZeroCoolDatabaseTableAdapters.XSDTableAdapter tableAdapter)
        {
            DataTable resultSet = tableAdapter.GetData();
            foreach (DataRow row in resultSet.Rows)
            {
                Console.WriteLine("Firstname: {0}\nLastname:{1}\nAge: {2}\n------------", row["Firstname"], row["Lastname"], row["Age"]);
                Console.WriteLine("\n");
            }
        }

        /// <summary>
        /// Showcases the Datacontext version of Object Relational Mapping
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
             *  ...To Be Continued
             */

            //Gets all the tables in the Data Context
            ZeroCoolTableClassesDataContext x = new ZeroCoolTableClassesDataContext();

            //Reading Database Table and Printing Records
            ClearAndDisplayMessage("Reading Database Table and Printing Records:");
            ORM_DataContext_PrintTable(x);
            ContinueOn();
            
            #region Record Insert
            ClearAndDisplayMessage("Now I will insert a record..\nPrinting Database Table Records now:");
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
                Console.WriteLine("<DataContext>: Unable to Insert Record. \n"+e);
            }
            #endregion 
            ORM_DataContext_PrintTable(x);
            ContinueOn();

            #region Record Update
            ClearAndDisplayMessage("Now I will Update a record..Can you guess which :)\nPrinting Database Table Records now:");
            XSDTable recordToModify = x.XSDTables.Where(y => y.Firstname == "Derrick" && y.Lastname == "Ward").FirstOrDefault();
            recordToModify.Lastname = "Kyle-" + recordToModify.Lastname;
            try
            {
                x.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("<DataContext>: Unable to Modify Record. \n"+e);
            }
            #endregion
            ORM_DataContext_PrintTable(x);
            ContinueOn();

            #region Record Update
            ClearAndDisplayMessage("Now I will Put the record back :)\nPrinting Database Table Records now:");
            recordToModify = x.XSDTables.Where(y => y.Firstname == "Derrick" && y.Lastname == "Kyle-Ward").FirstOrDefault();
            recordToModify.Lastname = recordToModify.Lastname.Substring(recordToModify.Lastname.IndexOf("Kyle-")+("Kyle-").Length);
            try
            {
                x.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("<DataContext>: Unable to Modify Record back to its original. \n"+e);
            }
            #endregion
            ORM_DataContext_PrintTable(x);
            ContinueOn();

            #region Delete Record
            ClearAndDisplayMessage("Now I will Delete a record..Can you guess which :)\nPrinting Database Table Records now:");
            recordToModify = x.XSDTables.Where(y => y.Firstname == "Joe" && y.Lastname == "Robert").FirstOrDefault();
            x.XSDTables.DeleteOnSubmit(recordToModify);
            try
            {
                x.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("<DataContext>: Unable to Delete Record.\n"+e);
            }
            #endregion
            ORM_DataContext_PrintTable(x);
            ContinueOn();

            //Dispose of Resources
            x.Dispose();
        }

        /// <summary>
        /// Prints Records in Database Table: For DataContext Demo
        /// </summary>
        /// <param name="dataContext"></param>
        public static void ORM_DataContext_PrintTable (ZeroCoolTableClassesDataContext dataContext)
        {
            if (dataContext != null)
            {
                int spacePadding = 15;
                var resultSet = from people in dataContext.XSDTables
                                select people;

                if (resultSet.Any())
                {
                    //Create object of POCO
                    XSDTable firstRow = new XSDTable();
                    
                    //Print Columns Headers, using the first record from the Database Table
                    Console.WriteLine("{0}*{1}*{2}*"
                        , nameof(firstRow.Firstname).PadLeft(spacePadding)
                        , nameof(firstRow.Lastname).PadLeft(spacePadding)
                        , nameof(firstRow.Age).PadLeft(spacePadding));

                    //Loop Through each database record and print it
                    foreach (var record in resultSet)
                    {
                        Console.WriteLine("{0}|{1}|{2}|"
                            , record.Firstname.PadLeft(spacePadding)
                            , record.Lastname.PadLeft(spacePadding)
                            , record.Age.ToString().PadLeft(spacePadding));
                    }
                }
            }
        }

        /// <summary>
        /// Showcases using SQLClient classes to do Object Relational Mapping
        /// </summary>
        public static void ORM_SQLClient_Demo()
        {
            /* High Level Steps:
             * 1) Create a Database in SQL Server
             * 2) Reading from a Database Table
             *    A) Instantiate your SQLConnection Object, with the connection string
             *    B) Instatiate your SQLCommand Object, with the query and SQlConnection
             * 3) Inserting Records
             *    A) Instantiate your SQLConnection Object, with the connection string
             *    B) Instatiate your SQLCommand Object, with the query and SQlConnection
             *    C) Add to the Parameters Collection in your SQL Command Object
             * 4) Updating Records
             *    A) Instantiate your SQLConnection Object, with the connection string
             *    B) Instatiate your SQLCommand Object, with the query and SQlConnection
             *    C) Add to the Parameters Collection in your SQL Command Object
             * 5) Deleting Records
             *    A) Instantiate your SQLConnection Object, with the connection string
             *    B) Instatiate your SQLCommand Object, with the query and SQlConnection
             *    C) Add to the Parameters Collection in your SQL Command Object
             */

            const string databaseName = "ZeroCool", databaseTable ="XSDTable";
            
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ZeroCoolDB_SQLAuth"].ConnectionString);
                conn.Open();

                #region Read/Print Database Table 

                //Reading Database Table and Printing Records
                ClearAndDisplayMessage("Reading Database Table and Printing Records:");
                ORM_SQLClient_PrintTable(new SqlConnection(conn.ConnectionString));
                ContinueOn();

                #endregion
                
                #region Insert Record into Database Table and Print Table Records

                ClearAndDisplayMessage("Inserting a Record into a Database and Printing Records now...");

                string sqlquery = string.Format("INSERT INTO {0} ({1},{2},{3}) VALUES ({4},{5},{6})"
                    ,(databaseName+".dbo."+databaseTable)
                    ,@"Firstname",@"Lastname", @"Age"
                    ,@"@Firstname",@"@Lastname",@"@Age");

                SqlCommand command = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = sqlquery
                };
                command.Parameters.Add(new SqlParameter("Firstname", @"Shantel"));
                command.Parameters.Add(new SqlParameter("Lastname", @"Butler"));
                command.Parameters.Add(new SqlParameter("Age", 25));

                Debug.WriteLine("Insert Query: "+ sqlquery);

                command.ExecuteNonQuery(); //Runs/Executes query

                ORM_SQLClient_PrintTable(new SqlConnection(conn.ConnectionString));
                ContinueOn();

                #endregion

                #region Update Record in Database Table and Print Table Records

                ClearAndDisplayMessage("Updating a Record in the Database. Can you guess which one? \nPrinting Records now...");

                string sqlquery2 = string.Format("UPDATE {0} SET {1}=@Firstname, {2}=@Lastname, {3}=@Age WHERE {1}=@OrigFirstname"
                    , (databaseName + ".dbo." + databaseTable)
                    , @"Firstname", @"Lastname", @"Age");

                SqlCommand command2 = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = sqlquery2
                };
                command2.Parameters.Add(new SqlParameter("Firstname", @"ShantelUpdated"));
                command2.Parameters.Add(new SqlParameter("Lastname", @"Butler"));
                command2.Parameters.Add(new SqlParameter("Age", 25));
                command2.Parameters.Add(new SqlParameter("OrigFirstname", @"Shantel"));

                Debug.WriteLine("Update Query: " + sqlquery2);

                command2.ExecuteNonQuery(); //Runs/Executes query

                ORM_SQLClient_PrintTable(new SqlConnection(conn.ConnectionString));
                ContinueOn();

                #endregion

                #region Delete Record in Database Table and Print Table Records

                ClearAndDisplayMessage("Deleting a Record in the Database. Can you guess which one? \nPrinting Records now...");

                string sqlquery3 = string.Format("DELETE FROM {0} WHERsE {1}=@OrigFirstname"
                    , (databaseName + ".dbo." + databaseTable)
                    , @"Firstname");

                SqlCommand command3 = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = sqlquery3
                };
                command3.Parameters.Add(new SqlParameter("OrigFirstname", @"ShantelUpdated"));

                Debug.WriteLine("Delete Query: " + sqlquery3);

                command3.ExecuteNonQuery(); //Runs/Executes query

                ORM_SQLClient_PrintTable(new SqlConnection(conn.ConnectionString));
                ContinueOn();

                #endregion

                #region To Dispose

                conn.Dispose();
                command.Dispose();

                #endregion

            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occured: \n"+e);
            }
            
        }

        /// <summary>
        /// Prints Records in Database Table: For SQL Client Demo
        /// </summary>
        /// <param name="conn">SqlConnetion Type, has the connection settings</param>
        public static void ORM_SQLClient_PrintTable(SqlConnection conn)
        {
            //Will Hold Database Query Result Set
            List<XSDTable> tableRecords = new List<XSDTable>();

            //String Padding
            const int spacePadding = 15;

            #region Open Database Connect, Prepare SQL

            //Open a new Database Connection
            conn.Open();

            //Configure SQL Command  to be used
            const string sqlQuery = "Select Firstname, Lastname, Age From XSDTable";
            SqlCommand command = new SqlCommand()
            {
                CommandText = sqlQuery,
                Connection = conn
            };

            #endregion
            
            #region  Execute Query, Read Result Set, Print Results

            SqlDataReader reader = command.ExecuteReader();

            //Read and Store collection of records returned
            while(reader.Read())
            {
                string firstName = reader.GetString(0);
                string lastName = reader.GetString(1);
                long age = reader.GetInt64(2);

                XSDTable record = new XSDTable()
                {
                    Firstname = firstName,
                    Lastname = lastName,
                    Age = age
                };

                tableRecords.Add(record);
            }

            //Print Datbase Records to the Screen
            if (tableRecords.Count > 0)
            {
                //Print Columns Headers, using the first record from the Database Table
                Console.WriteLine("{0}*{1}*{2}*"
                    , @"Firstname".PadLeft(spacePadding)
                    , @"Lastname".PadLeft(spacePadding)
                    , @"Age".PadLeft(spacePadding));

                foreach (var tableRecord in tableRecords)
                {
                    Console.WriteLine("{0} {1} {2}",tableRecord.Firstname.PadLeft(spacePadding)
                        ,tableRecord.Lastname.PadLeft(spacePadding)
                        ,tableRecord.Age.ToString().PadLeft(spacePadding));
                }
            }

            #endregion

            #region To Dispose

            conn.Close();
            command.Dispose();
            reader.Dispose();

            #endregion
        } 

        /// <summary>
        /// Prints Program Intro/Welcome Message(s)
        /// </summary>
        public static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Derrick's Database Object Relational Mapping Demos.\n");
            Console.WriteLine("Frameworks that will be used:\n1.{0}\n2.{1}","ADO.Net","LINQ to SQL");
            Console.WriteLine("\nFramework Subtopics that will be used:\n1.{0}\n2.{1}\n3.{2}\n4.{3}\n5.{4}",
                "DataSet","TableAdapters","DataContext", "LINQ To SQL Classes", "SQLCommand");
            Console.WriteLine("\n...Enjoy :)\n\n----------------------------------------------------------------\n");
        }

        /// <summary>
        /// Clear the Console Window, Dispay a Message, and wait for user to hit a key
        /// </summary>
        /// <param name="message"></param>
        public static void ClearAndDisplayMessage(string message)
        {
            Console.Clear();
            Console.WriteLine("\n"+message+"\nPress any key to continue..\n");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Prompt the user before continuing
        /// </summary>
        public static void ContinueOn()
        {
            Console.WriteLine("\nPress any key to continue..");
            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Sets the Database Table back to the original record
        /// </summary>
        public static void ResetDatabaseTable() {
            ZeroCoolTableClassesDataContext x = new ZeroCoolTableClassesDataContext();
            x.XSDTables.DeleteAllOnSubmit(x.XSDTables);
            x.XSDTables.InsertOnSubmit(new XSDTable() { Firstname = "Derrick", Lastname = "Ward", Age = 25 });

            try
            {
                x.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to Reset Database: " + e);
            }
        }
    }
}
