using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ShootRate.Models
{
    public class Game
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Date { get; set; }

        //the sequence of hits, separated by ,
        // for example, "0,5,11"
        public string Hits { get; set; }
        public int Tries { get; set; }

        public int Percentage { get; set; }

        
        public string Summary { get; set; }
    }
}
