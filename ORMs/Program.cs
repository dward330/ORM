using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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


            Console.WriteLine("This project will showcase the different ways Derrick knows how to do ORM.\n");

            ZeroCoolDatabaseTableAdapters.XSDTableTableAdapter xsdTableAdapter = new ZeroCoolDatabaseTableAdapters.XSDTableTableAdapter();
            int primIthink = xsdTableAdapter.Insert("Kora", "Ward", 58);
            DataTable resultSet = xsdTableAdapter.GetData();

            Console.WriteLine("Primary Key Returned: "+primIthink);

            foreach (DataRow row in resultSet.Rows) {
                Console.WriteLine(String.Format("Firstname: {0}\nLastname:{1}\nAge: {2}\n------------",row["Firstname"],row["Lastname"],row["Age"]));
                Console.WriteLine("\n");
            }

            Console.ReadKey();
            Console.WriteLine("...");
           }
    }
}
