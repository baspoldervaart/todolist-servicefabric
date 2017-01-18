using System;

namespace Shared.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public string Prioriteit { get; set; }
        public string ToegewezenAan { get; set; }
        public DateTime VervalDatum { get; set; }
        public string Categorie { get; set; }
    }
}