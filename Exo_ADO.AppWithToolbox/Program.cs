using Exo_ADO.AppWithToolbox.Models;
using Exo_ADO.ToolBox.Database;
using static System.Collections.Specialized.BitVector32;

#region Définition de la connexion vers la DB
string connectionString = @"Data Source=Forma300\TFTIC;Initial Catalog=Exo_ADO;User ID=Chris;Password=Test1234=;TrustServerCertificate=true;";
#endregion


// Initialize l'object "Connection" de la toolbox
Connection db = new Connection(connectionString);

// Ouverture de la connection vers la DB
db.Open();

#region Récuperation de la moyenne réel des etudiants qui ont raté => ExecuteScalar
Command cmd1 = new Command("SELECT AVG(CONVERT(REAL, YearResult)) FROM student WHERE YearResult < @MaxResult");
cmd1.AddParameter("MaxResult", 10);

double moyenne = db.ExecuteScalar<double>(cmd1);

Console.WriteLine($"La moyenne des nulls est de {moyenne}/20");
Console.WriteLine();
#endregion

#region Récuperation des etudiants qui ont moins de 10 => ExecuteReader
Command cmd2 = new Command("SELECT * FROM student WHERE YearResult < @MaxResult");
cmd2.AddParameter("MaxResult", 10);

// ↓ Warning : Traitement différé !
IEnumerable<Student> nullos = db.ExecuteReader(cmd2, (record) =>
{
    // Ceci est le contenu du délégé => En soit, c'est une simple méthode ;)
    return new Student()
    {
        Id = (int)record["Id"],
        FirstName = (string)record["FirstName"],
        LastName = (string)record["LastName"],
        BirthDate = (DateTime)record["BirthDate"],
        YearResult = (int)record["YearResult"],
        SectionID = (int)record["SectionID"]
    };
});

foreach(Student student in nullos)
{
    Console.WriteLine($"{student.Id} : {student.FirstName} {student.LastName}");
}
#endregion



db.Close();