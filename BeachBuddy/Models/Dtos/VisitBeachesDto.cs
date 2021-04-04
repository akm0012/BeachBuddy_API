using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeachBuddy.Models.Dtos
{
    public class VisitBeachesDto
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public Beach Beach { get; set; }
    }

    public class Beach
    {
        public List<Report> LastThreeDaysOfReports { get; set; }
    }

    public class Report
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<BeachReport> BeachReport { get; set; }
    }

    public class BeachReport
    {
        public ParameterCategory ParameterCategory { get; set; }
        public List<ReportParameters> ReportParameters { get; set; }
    }

    public class ParameterCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public class ReportParameters
    {
        public List<ParameterValue> ParameterValues { get; set; }
        public string Value { get; set; }
    }

    public class ParameterValue
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
}