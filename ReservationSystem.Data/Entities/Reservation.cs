using ReservationSystem.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace ReservationSystem.Data.Entities
{
    public class Reservation {

        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int TermId { get; set; }
        public Term Term { get; set; }

        [Required]
        [StringLength(50)]
        public string Classroom { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required]
        [MaxLength(20)]
        [RegularExpression("Pending|Approved|Rejected", ErrorMessage = "Lütfen geçerli bir durum seçin.")]
        public string Status { get; set; } = "Pending";

        public bool IsHolidayWarning { get; set; } = false;


    }
}

