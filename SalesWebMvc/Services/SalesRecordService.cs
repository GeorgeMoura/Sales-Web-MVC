using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            this._context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;  // retorna um objeto IQueryable<SalesRecord> que me permite executar consultas linq, a consulta só é executada quando o ToList() é chamado
            
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result.Include(x => x.Seller).Include(x => x.Seller.Department).OrderByDescending(x => x.Date).ToListAsync(); //faz o join com a tabela de sallers e department e ordena de maneira decrescente por data
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate) //quando você utiliza o GroupBy do Linq, o retorno é um objeto do tipo IGrouping<TipoAgrupado, TipoTabela>
        {
            var result = from obj in _context.SalesRecord select obj;  // retorna um objeto IQueryable<SalesRecord> que me permite executar consultas linq, a consulta só é executada quando o ToList() é chamado

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result.Include(x => x.Seller).Include(x => x.Seller.Department).OrderByDescending(x => x.Date).GroupBy(x => x.Seller.Department).ToListAsync(); //faz o join com a tabela de sallers e department e ordena de maneira decrescente por data
        }
    }
}
