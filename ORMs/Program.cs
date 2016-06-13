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
            /*
             * 1) Create Databases in SQL Server
             * 2) Add a Dataset "Item" to your Project
             *      A) Add database tables to the dataset. Feel free to create different table adapter as well.
             * 3) Instantiate the table adapter you want. 
             * 4) Call "your method" to get data from your table adapter and store it in a DataTable object.
             * 5) Iterate through the rows in your DataTable object and read your data. -> Foreach loop
             */

            //COmment Added from Desktop

            Console.WriteLine("This project will showcase the different ways Derrick knows how to do ORM.\n");

            ZeroCoolDatabaseTableAdapters.XSDTableTableAdapter xsdTableAdapter = new ZeroCoolDatabaseTableAdapters.XSDTableTableAdapter();
            DataTable resultSet = xsdTableAdapter.GetData();
            foreach (DataRow row in resultSet.Rows) {
                Console.WriteLine(String.Format("Firstname: {0}\nLastname:{1}\nAge: {2}\n------------",row["Firstname"],row["Lastname"],row["Age"]));
                Console.WriteLine("\n");
            }

            ZeroCoolTableClassesDataContext x = new ZeroCoolTableClassesDataContext();

            IQueryable<ORMs.XSDTable> queryresult = from people in x.XSDTables
                                               where SqlMethods.Like(people.Firstname, "%r%")
                                               select people;

            Console.WriteLine("The number of people with a 'r' in their first name are: "+queryresult.Count());

            Console.ReadKey();
            Console.WriteLine("...");
           }
    }
}
