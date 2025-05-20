namespace ReservationSystem.Data.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


public class Term{
    public int Id{ get; set; }

    [Required]
    [MaxLength(100)]
    public string Name{ get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}