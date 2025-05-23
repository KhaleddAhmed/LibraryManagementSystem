﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Entities.User;

namespace LibraryManagementSystem.Core.Entities.Library
{
    public class UserBorrowing
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsBorrowApproved { get; set; } = false;
        public DateTime RequestForBorrowDate { get; set; } = DateTime.Now;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }

        public double AmountOfBorrow { get; set; }

        public bool IsStillBorrowed { get; set; } = false;
    }
}
