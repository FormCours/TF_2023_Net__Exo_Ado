using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exo_ADO.AppWithToolbox.DemoLinQ
{
    internal static class DemoFilter
    {

        // Le mot clef "this" devant le premier parametre permet de créer une méthode d'extention sur le type de données
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> collection, Func<T, bool> filterCondition)
        {

            foreach(T item in collection)
            {
                // La condition est géré par le délégé
                if(filterCondition(item))
                {
                    // Le "yield return" permet un traitement différé
                    yield return item;
                }
            }

        }  

    }
}
