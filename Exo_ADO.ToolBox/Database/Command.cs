using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exo_ADO.ToolBox.Database
{
    public sealed class Command
    {
        // Field
        private readonly Dictionary<string, object> _Parameters = new Dictionary<string, object>();

        // Props
        public string Query { get; init; }
        public bool IsStoredProcedure { get; init; }

        public IReadOnlyDictionary<string, object> Parameters
        {
            // Renvoi d'une copie en lecture seul du dico -> Securisation du contenu
            get { return new ReadOnlyDictionary<string, object>(_Parameters); }
        }

        // Ctor
        public Command(string query, bool isStoredProcedure = false)
        {
            Query = query;
            IsStoredProcedure = isStoredProcedure;
        }

        // Method
        public void AddParameter(string key, object? value)
        {
            _Parameters.Add(key, value ?? DBNull.Value);

            //_Parameters.Add(key, value is null ? DBNull.Value : value);
        }
    }
}
