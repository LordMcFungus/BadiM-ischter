using System;
namespace BadiMeischter.Model
{
    public class BadiInfo {
        public string Name { get; set; }
        public string Adresse { get; set; }
        public string Plz { get; set; }
        public string Ort { get; set; }
        public string Tel { get; set; }
        public string Www { get; set; }
    }
    public class Badi
    {
        public BadiInfo Properties { get; set; }

    }
}
