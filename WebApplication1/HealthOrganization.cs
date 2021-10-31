using System;

namespace WebApplication1
{
    public class HealthOrganization
    {
        public string Name { get; set; }

        public string Info { get; set; }

        public string Location { get; set; }
    }

    public abstract class People
    {
        public string Name { get; set; }
    }
    public class Workers:People
    {
        public int Id { get; set; }
        public string Specialization { get; set; }

        public int NumberOfSick { get; set; }

        public int Expirience { get; set; }
    }
    public class Sick:People
    {
        public DateTime DateOfAdmission { get; set; }

        public string Deagnose { get; set; }

        public string Treatment { get; set; }

        public  string Doctor { get; set; }
    }
}
