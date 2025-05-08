namespace  Lab10CSharp.Lab10CSharpTask1 {
    
    public class City {
        public static Random rand = new Random((int)DateTime.Now.Ticks);

        static Dictionary<string, int> events = new() 
        { 
            {"Festival", 5}, 
            {"Fire", -6}, 
            {"Concert", 4}, 
            {"CarAccident", -3}, 
            {"GoodWeather", 1}, 
            {"BadWeather", -1} 
        };

        public string Name { get; init; }
        public DateOnly CurrentDate{ get; private set; }

        
        public List<CityEventArgs> futureEvents = new List<CityEventArgs>();
        public List<CityEventArgs> CurrentEvents = new List<CityEventArgs>();

        public event EventHandler<CityEventArgs> OnNewDay;

        public double CitizenHappines { get; private set; } = 0;


        public City(string name, DateOnly currentDateTime) {
            Name = name;
            CurrentDate = currentDateTime;
            OnNewDay = new EventHandler<CityEventArgs>(HandleEvent);
            NextDay();
        }

        private CityEventArgs GenerateRandomEvent() {
            string eventName = events.Keys.ToArray()[rand.NextInt64(0, events.Count)];
            DateOnly date = CurrentDate.AddDays(rand.Next(1,14));
            DateOnly endDate = date.AddDays(rand.Next(1,7));
            return GenerateEvent(eventName, date, endDate, events[eventName]);
        }
        private CityEventArgs GenerateEvent(string name, DateOnly eventDateTime, DateOnly endDate, double result) 
        {
            return new CityEventArgs(name, eventDateTime, endDate, result * (1 + 0.5 * rand.NextDouble()));
        }

        private void NextDay() {
            // Stats track
            CurrentDate = CurrentDate.AddDays(1);
            Console.WriteLine($"\n\n\n>>> Date {CurrentDate} in {Name} city. Current happiness: {CitizenHappines:F2}\n");
   
            // Chance for random event
            if(rand.NextDouble() < 0.5)
                futureEvents.Add(GenerateRandomEvent());

            // Update events
            FireEvents();

            // Output in console
            Console.WriteLine("Current Events: " + CurrentEvents.Count);
            if(CurrentEvents.Count == 0) 
                Console.WriteLine("No events...");
            else
                foreach (CityEventArgs e in CurrentEvents) 
                    Console.WriteLine(e.ToString());
                

            // Output in console
            Console.WriteLine("Future event list: " + futureEvents.Count);
            if(futureEvents.Count == 0) 
                Console.WriteLine("No events...");
            else
                foreach (CityEventArgs e in futureEvents) 
                    Console.WriteLine(e.ToString());
                

            // Wait for input to simulate next day
            Console.Write("\nEnter any value to simulate next day: ");
            Console.ReadLine();
            NextDay();
        }

        private void FireEvents() {
            // Remove ended events from
            while(CurrentEvents.Find(item => item.EndDate < CurrentDate) != null) {
                var item = CurrentEvents.Find(item => item.EndDate < CurrentDate);
                if (item == null) return;

                CurrentEvents.Remove(item);
            }

            // Change future events in list to current
            while(futureEvents.Find(item => item.StartDate <= CurrentDate) != null) {
                var item = futureEvents.Find(item => item.StartDate <= CurrentDate);
                if (item == null) return;

                futureEvents.Remove(item);
                CurrentEvents.Add(item);
            }

            // Invoke current events
            foreach(CityEventArgs e in CurrentEvents) {
                OnNewDay?.Invoke(null, e);
            }
        }

        private void HandleEvent(object? sender, CityEventArgs cityEventArgs) {
            CitizenHappines += cityEventArgs.Result;
        }
    }
}