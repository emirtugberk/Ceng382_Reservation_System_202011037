using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ReservationSystem.Business.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly HttpClient _httpClient;
        public HolidayService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsHolidayAsync(DateTime date)
        {
            var year = date.Year;
            var url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/TR";
            var holidays = await _httpClient.GetFromJsonAsync<List<PublicHolidayDto>>(url);
            if (holidays == null)
                return false;

            foreach (var h in holidays)
            {
                var holidayDate = DateTime.Parse(h.Date);
                if (holidayDate.Date == date.Date)
                    return true;
            }
            return false;


        }

    }
}