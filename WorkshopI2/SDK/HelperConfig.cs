using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjetWorkshop
{
    class HelperConfig
    {

        private string CHEMIN_FICHIER_JSON = string.Concat(Directory.GetCurrentDirectory(), "/JSON/","Configuration.json");

        public Dictionary<string, List<string>> DicoTable = new Dictionary<string, List<string>>();
        public HelperConfig()
        {
            ChargerDictionnaireTable();
            foreach(KeyValuePair<string, List<string>> table in DicoTable)
            {
                

                StringBuilder sb = new StringBuilder();
                sb.Append("UPDATE ");
                sb.Append(table.Key);
                sb.Append(" SET ");
                foreach (string colonne in table.Value)
                {
                    sb.Append($" {colonne} = @aeDep , ");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" WHERE id = @id");

                string test = sb.ToString();
            }

        }

        private void ChargerDictionnaireTable()
        {
            string json = File.ReadAllText(CHEMIN_FICHIER_JSON);
            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    JArray tables = JArray.Parse(json);
                    foreach (JObject table in tables)
                    {
                        List<string> listColonnes = new List<string>();
                        if (table["table"] != null && table["Colonnes"] != null)
                        {
                            JArray colonnes = JArray.Parse(table["Colonnes"].ToString());
                            foreach (JObject colonne in colonnes)
                            {
                                if (colonne["Nom"] != null)
                                {
                                    listColonnes.Add(colonne["Nom"].ToString());
                                }
                            }
                            DicoTable.Add(table["table"].ToString(), listColonnes);

                        }
                    }

                }
            }
            catch(Exception exc)
            { throw new Exception($"Erreur lors de la lecture du fichier de configuration avec l'erreur suivant : '{exc.Message}'"); }
            
        }
    }
}
