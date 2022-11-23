using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShootRate.Models
{
    public class TenShoot
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int Hits { get; set; }
        public int Tries { get; set; }
    }
}
