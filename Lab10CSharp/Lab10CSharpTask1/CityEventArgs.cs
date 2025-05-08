namespace  Lab10CSharp.Lab10CSharpTask1 {
    public class CityEventArgs : EventArgs {
        public string EventName{ get; init; }
        public DateOnly StartDate{ get; init; }
        public DateOnly EndDate { get; init; }
        
        public double Result { get; init; } 

        public CityEventArgs(string name, DateOnly eventDateTime, DateOnly duration, double result) {
            EventName = name;
            StartDate = eventDateTime;
            EndDate = duration;
            Result = result;
        }
        public override string ToString() {
            return ($"{EventName} === Start at {StartDate}, End {EndDate}. Resulting impact: {Result:F2}");
        }
    }
}