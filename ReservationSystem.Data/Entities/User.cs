using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ReservationSystem.Data.Entities;



namespace ReservationSystem.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }


    }
}
