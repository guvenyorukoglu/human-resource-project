﻿using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;

namespace humanResourceProject.Models.VMs
{
    public class PersonelVM
    {
        public Guid Id { get; set; }
        public string? ImagePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public Guid JobId { get; set; }
        public string Title { get; set; }
        public Leave? Leave { get; set; }
        public List<Leave>? Leaves { get; set; }
        public Advance? Advance { get; set; }
        public List<Advance>? Advances { get; set; }
        public Expense? Expense { get; set; }   
        public List<Expense>? Expenses { get; set; }
        public Company Company { get; set; }
        public string Department { get; set; }
    }
}
