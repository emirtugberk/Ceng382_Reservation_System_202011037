namespace ReservationSystem.Business.Services
{
    public class SmtpSettings
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public string User { get; set; } = default!;
        public string Pass { get; set; } = default!;
    }
}
