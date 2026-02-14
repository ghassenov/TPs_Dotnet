using tp1.Models;
using System.Collections.Generic;

namespace tp1.ViewModels
{
    public class MovieCustomerViewModel
    {
        public Customer? Customer { get; set; }
        public List<Movie>? Movies { get; set; }
    }
}

