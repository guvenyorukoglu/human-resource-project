﻿namespace humanResourceProject.Models.VMs
{
    public class AppUserVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}