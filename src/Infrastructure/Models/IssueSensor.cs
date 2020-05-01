using System;

namespace Jineo.Models
{
    public class IssueSensor
    {
        public int SensorId { get; set; }
        public Sensor Sensor {get;set;}
        public int IssueId { get; set; }
        public Issue Issue {get;set;}

    }
}