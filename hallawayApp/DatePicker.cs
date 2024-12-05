using System;

namespace hallawayApp
{
    public class DatePicker
    {
        public (DateTime StartDate, DateTime EndDate) PickDateRange()
        {
            Console.WriteLine("Start Date (yyyy-mm-dd):");
            DateTime startDate = GetValidDate();

            Console.WriteLine("End Date (yyyy-mm-dd):");
            DateTime endDate = GetValidDate();
            
            while (endDate < startDate)
            {
                Console.WriteLine("End Date can't be earlier than Start Date:");
                endDate = GetValidDate();
            }

            return (startDate, endDate);
        }

        private DateTime GetValidDate()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (DateTime.TryParse(input, out DateTime date))
                {
                    return date;
                }
                Console.WriteLine("Ogiltigt datum. Försök igen (yyyy-mm-dd):");
            }
        }
    }
}