using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ReservationSystem.Data.Entities;


namespace ReservationSystem.Data.Entities
{
    public class Feedback{

        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        


        public int UserId { get; set; }
        public User User { get; set; }
        

        [Required]
        [MaxLength(50)]
        public string Classroom { get; set; }


        [Required]
        [Range(1,5)]
        public int Rating { get; set; }


        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }


        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}