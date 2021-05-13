using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Data
{
    public class SeedingService
    {
        private SalesWebMvcContext _context;

        public SeedingService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if(_context.Department.Any() || _context.Seller.Any() || _context.SalesRecord.Any())
            {
                return;
            }

            Department d1 = new Department("Computers");
            Department d2 = new Department("Eletronics");
            Department d3 = new Department("Fashion");
            Department d4 = new Department("Books");

            Seller s1 = new Seller("George", "George@gmail.com", 1000.0, new DateTime(1998, 4, 21), d1);
            Seller s2 = new Seller("Victor", "Victor@gmail.com", 1200.0, new DateTime(1998, 3, 23), d2);
            Seller s3 = new Seller("Nicole", "Nicole@gmail.com", 1300.0, new DateTime(1998, 4, 26), d1);
            Seller s4 = new Seller("Walter", "Walter@gmail.com", 1400.0, new DateTime(1998, 2, 22), d4);
            Seller s5 = new Seller("Kevin", "Kevin@gmail.com", 1500.0, new DateTime(1998, 1, 26), d3);
            Seller s6 = new Seller("Edgar", "Edgar@gmail.com", 1600.0, new DateTime(1998, 2, 22), d2);

            SalesRecord r1 = new SalesRecord(new DateTime(2021, 09, 23), 11000.0, Salestatus.Billed, s1);
            SalesRecord r2 = new SalesRecord(new DateTime(2021, 08, 22), 1000.0, Salestatus.Pending, s2);
            SalesRecord r3 = new SalesRecord(new DateTime(2021, 02, 21), 1100.0, Salestatus.Billed, s3);
            SalesRecord r4 = new SalesRecord(new DateTime(2021, 03, 27), 112000.0, Salestatus.Canceled, s4);
            SalesRecord r5 = new SalesRecord(new DateTime(2021, 04, 22), 1200.0, Salestatus.Billed, s5);
            SalesRecord r6 = new SalesRecord(new DateTime(2021, 08, 29), 4000.0, Salestatus.Pending, s6);
            SalesRecord r7 = new SalesRecord(new DateTime(2021, 03, 22), 200.0, Salestatus.Billed, s2);
            SalesRecord r8 = new SalesRecord(new DateTime(2021, 02, 11), 3000.0, Salestatus.Canceled, s1);
            SalesRecord r9 = new SalesRecord(new DateTime(2021, 01, 12), 1030.0, Salestatus.Billed, s3);
            SalesRecord r10 = new SalesRecord(new DateTime(2021, 06, 11), 11200.0, Salestatus.Pending, s5);
            SalesRecord r11 = new SalesRecord(new DateTime(2021, 02, 13), 12000.0, Salestatus.Billed, s6);

            _context.Department.AddRange(d1, d2, d3, d4);
            _context.Seller.AddRange(s1, s2, s3, s4, s5, s6);
            _context.SalesRecord.AddRange(r1, r2, r3 , r4, r5, r6, r7, r8, r9, r10, r11);

            _context.SaveChanges();

        }
    }
}
