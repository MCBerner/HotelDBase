using System;
using System.Collections.Generic;
using System.Text;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
namespace HotelDBase
{
    class DBClient
    {
        
        

        //Database connection string - replace it with the connnection string to your own database 
        string connectionString = "";

        private int GetMaxFacility(SqlConnection connection)
        {
            Console.WriteLine("Calling -> GetMaxFacility_ID");

            //This SQL command will fetch one row from the Facility table: The one with the max Facility_ID
            string queryStringMaxFacility_ID = "SELECT  MAX(Facility_ID)  FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringMaxFacility_ID}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringMaxFacility_ID, connection);
            SqlDataReader reader = command.ExecuteReader();

            //Assume undefined value 0 for max facility_ID
            int MaxFacility_ID = 0;

            //Is there any rows in the query
            if (reader.Read())
            {
                //Yes, get max facility_ID
                MaxFacility_ID = reader.GetInt32(0); //Reading int from 1st column
            }

            //Close reader
            reader.Close();

            Console.WriteLine($"Max #: {MaxFacility_ID}");
            Console.WriteLine();

            //Return max facility_ID
            return MaxFacility_ID;
        }

        private int DeleteFacility(SqlConnection connection, int facility_ID)
        {
            Console.WriteLine("Calling -> DeleteFacility");

            //This SQL command will delete one row from the Facility table: The one with primary key facility_ID
            string deleteCommandString = $"DELETE FROM Facility  WHERE Facility_ID = {facility_ID}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            Console.WriteLine($"Deleting facility #{facility_ID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int UpdateFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> UpdateFacility");

            //This SQL command will update one row from the Facility table: The one with primary key facility_ID
            string updateCommandString = $"UPDATE Facility SET Name='{facility.FacilityName}' WHERE Facility_ID = {facility.Facility_ID}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(updateCommandString, connection);
            Console.WriteLine($"Updating facility #{facility.Facility_ID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int InsertFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> InsertFacility");

            //This SQL command will insert one row into the Facility table with primary key facility_ID
            string insertCommandString = $"INSERT INTO Facility VALUES({facility.Facility_ID}, '{facility.FacilityName}')";
            Console.WriteLine($"SQL applied: {insertCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(insertCommandString, connection);

            Console.WriteLine($"Creating facility #{facility.Facility_ID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected 
            return numberOfRowsAffected;
        }

        private List<Facility> ListAllFacilities(SqlConnection connection)
        {
            Console.WriteLine("Calling -> ListAllFacilities");

            //This SQL command will fetch all rows and columns from the Facility table
            string queryStringAllFacilities = "SELECT * FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringAllFacilities}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringAllFacilities, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Listing all facilities:");

            //NO rows in the query 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No facilites in database");
                reader.Close();

                //Return null for 'no facilities found'
                return null;
            }

            //Create list of facilities found
            List<Facility> facilities = new List<Facility>();
            while (reader.Read())
            {
                //If we reached here, there is still one facility to be put into the list 
                Facility nextFacility = new Facility()
                {
                    Facility_ID = reader.GetInt32(0), //Reading int from 1st column
                    FacilityName = reader.GetString(1),    //Reading string from 2nd column
                   
                };

                //Add facility to list
                facilities.Add(nextFacility);

                Console.WriteLine(nextFacility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return list of hotels
            return facilities;
        }

        private Facility GetFacility(SqlConnection connection, int facility_ID)
        {
            Console.WriteLine("Calling -> GetFacility");

            //This SQL command will fetch the row with primary key facility_ID from the Facility table
            string queryStringOneFacility = $"SELECT * FROM Facility WHERE facility_ID = {facility_ID}";
            Console.WriteLine($"SQL applied: {queryStringOneFacility}");

            //Prepare SQK command
            SqlCommand command = new SqlCommand(queryStringOneFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine($"Finding facility#: {facility_ID}");

            //NO rows in the query? 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No facilities in database");
                reader.Close();

                //Return null for 'no facility found'
                return null;
            }

            //Fetch facility object from the database
            Facility facility = null;
            if (reader.Read())
            {
                facility = new Facility()
                {
                    Facility_ID = reader.GetInt32(0), //Reading int fro 1st column
                    FacilityName = reader.GetString(1),    //Reading string from 2nd column
                    
                };

                Console.WriteLine(facility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return found facility
            return facility;
        }


        public void Start()
        {
            //Apply 'using' to connection (SqlConnection) in order to call Dispose (interface IDisposable) 
            //whenever the 'using' block exits
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open connection
                connection.Open();

                //List all facilities in the database
                ListAllFacilities(connection);

                //Create a new facility with primary key equal to current max primary key plus 1
                Facility newFacility = new Facility()
                {
                    Facility_ID = GetMaxFacility(connection) + 1,
                    FacilityName = "Sauna"
                   
                };

                //Insert the facility into the database
                InsertFacility(connection, newFacility);

                //List all hotels including the the newly inserted one
                ListAllFacilities(connection);

                //Get the newly inserted facility from the database in order to update it 
                Facility facilityToBeUpdated = GetFacility(connection, newFacility.Facility_ID);

                //Alter Facility Name properties
                facilityToBeUpdated.FacilityName += "(updated)";
               

                //Update the facility in the database 
                UpdateFacility(connection, facilityToBeUpdated);

                //List all hotels including the updated one
                ListAllFacilities(connection);

                //Get the updated facility in order to delete it
                Facility facilityToBeDeleted = GetFacility(connection, facilityToBeUpdated.Facility_ID);

                //Delete the facility
                DeleteFacility(connection, facilityToBeDeleted.Facility_ID);

                //List all facilities - now without the deleted one
                ListAllFacilities(connection);
            }
        }
    }
}
