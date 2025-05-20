using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using ReservationSystem.Data.Entities;


namespace ReservationSystem.Data.Entities
{
    public class LogEntry
    {
        public int Id { get; set;}

        public int? UserId { get; set;}
        public User User { get; set;}

        [Required]
        [MaxLength(100)]
        public string Action { get; set; }

        public DateTime Timestamp { get; set; }= DateTime.UtcNow;

        public bool Success { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Details { get; set; }

    }
}