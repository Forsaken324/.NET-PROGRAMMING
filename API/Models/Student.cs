using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int StudentID {get; set;}
        
        public required string LastName {get; set;}
        public required string FirstMidName {get; set;}
        public  DateTime EnrollmentDate {get; set;} = DateTime.Now;

        public ICollection<Enrollment>? Enrollments {get; set;}


    }
}