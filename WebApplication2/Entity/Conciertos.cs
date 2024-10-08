﻿namespace WebApplication2.Entity
{
    public class Conciertos
    {
        public int id { get; set; }
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public Conciertos() { }
    
        public Conciertos(Conciertos concierto)
        {
            this.title = concierto.title;
            this.description = concierto.description;
            this.city = concierto.city;
            this.address = concierto.address;
        }
    }
}
