using Exo_ADO.App.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;

// Définition de la connexion vers la DB
//string connectionString = @"Data Source=Forma300\TFTIC;Initial Catalog=Exo_ADO;User ID=Chris;Password=Test1234=;TrustServerCertificate=true;";
string connectionString = @"Data Source=STEVEBSTORM\MSSQLSERVER01;Initial Catalog=Exo_ADO_DB;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

using(SqlConnection connection = new SqlConnection())
{
    connection.ConnectionString = connectionString;

    //Afficher l’« ID », le « Nom », le « Prenom » de chaque étudiant depuis la vue « V_Student » en utilisant la méthode connectée
    Console.WriteLine("  Mode connectée");
    Console.WriteLine("  **************");

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM [V_Student]";
        command.CommandType = CommandType.Text;

        connection.Open();
        using(SqlDataReader reader = command.ExecuteReader())
        {
            // Lecture de la DB
            while(reader.Read())
            {
                Student student = new Student
                {
                    Id = (int)reader["Id"],
                    LastName = (string)reader["LastName"],
                    FirstName = (string)reader["FirstName"],
                    YearResult = (int)reader["YearResult"],
                    BirthDate = (DateTime)reader["BirthDate"]
                };

                Console.WriteLine($"{student.Id} : {student.FirstName} {student.LastName}");
            }
        }
        connection.Close();
    }
    Console.WriteLine();


    // Afficher l’« ID », le « Nom » de chaque section en utilisant la méthode déconnectée
    Console.WriteLine("  Mode déconnectée");
    Console.WriteLine("  ****************");

    using(SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM [V_Student]";
        command.CommandType = CommandType.Text;

        // Créer l'adapter avec la config pour la récuperation de donnée
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = command;

        // Utilisation de l'adapter pour populer un "DataSet" / "DataTable"
        DataTable table = new DataTable();
        adapter.Fill(table);
        // Remarque -> Il n'est pas necessaire d'ouvrir et fermer la connexion :)

        // Parcours des resultats
        foreach(DataRow row in table.Rows)
        {
            Console.WriteLine($"{row["Id"]} : {row["LastName"]}");
        }
    }
    Console.WriteLine();

    // Afficher la moyenne annuelle des étudiants
    Console.WriteLine("  Moyenne annuelle des étudiants");
    Console.WriteLine("  ******************************");

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT AVG(CONVERT(FLOAT, [YearResult])) FROM [V_Student]";
        command.CommandType = CommandType.Text;

        connection.Open();
        double moyenne = (double)command.ExecuteScalar();
        connection.Close();

        Console.WriteLine($"La moyenne est de {moyenne} !");
    }

    //Inserer un nouveau student
    //Instancier un student
    Student moi = new Student
    {
        FirstName = "Steve",
        LastName = "Lorent",
        BirthDate = new DateTime(2000,01,01),
        YearResult = 20,
        SectionID = 1010
    };

    //Insertion en DB
    //using(SqlCommand command = connection.CreateCommand())
    //{
    //    string query = "INSERT INTO student (FirstName, LastName, BirthDate, YearResult, SectionID) " +
    //        " OUTPUT inserted.Id " +
    //        "VALUES(@prenom, '" + moi.LastName + "', '" +
    //        moi.BirthDate + "', '" + moi.YearResult +"', '" + moi.SectionID+ "')";

    //    // command.Parameters.Add
    //    SqlParameter PPrenom = new SqlParameter()
    //    {
    //        ParameterName = "prenom",
    //        Value = moi.FirstName,
    //        Direction = ParameterDirection.Input
    //    };

    //    command.Parameters.Add(PPrenom);

    //    command.Parameters.AddWithValue("prenom", moi.FirstName);

    //    command.CommandText = query;
    //    connection.Open();
    //    int id = (int)command.ExecuteScalar();
    //    connection.Close();
    //    Console.WriteLine("Nouvel Id : " + id);
    //}

    //Utilisation Requête parametrée

    //Student voisin = new Student
    //{
    //    FirstName = "Arthur",
    //    LastName = "Pendragon",
    //    BirthDate = new DateTime(2000, 01, 01),
    //    YearResult = 19,
    //    SectionID = 1010
    //};

    //using(SqlCommand command = connection.CreateCommand())
    //{
    //    command.CommandText = "INSERT INTO student (FirstName, LastName, BirthDate, YearResult, SectionID) " +
    //        "VALUES (@FirstName, @LastName, @BirthDate, @YearResult, @SectionId)";

    //    command.Parameters.AddWithValue("FirstName", voisin.FirstName);
    //    command.Parameters.AddWithValue("LastName", voisin.LastName);
    //    command.Parameters.AddWithValue("BirthDate", voisin.BirthDate);
    //    command.Parameters.AddWithValue("YearResult", voisin.YearResult);
    //    command.Parameters.AddWithValue("SectionId", voisin.SectionID);

    //    connection.Open();

    //    command.ExecuteNonQuery();
    //    connection.Close();
    //}

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "DELETE FROM student WHERE Id = 27";
        connection.Open();
        try
        {
            command.ExecuteNonQuery();
        }
        catch(SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        connection.Close();
    }

}