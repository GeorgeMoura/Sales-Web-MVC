using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await this._context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            this._context.Seller.Add(seller);
            await this._context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id); //eager loading = forçar a consulta do banco a fazer o join também nos objetos relacionados
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = _context.Seller.Find(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }catch(DbUpdateException e)
            {
                throw new IntegrityException(e.Message);  //o lançamento dessa exceção serve pra capturar o caso onde eu tento deletar um vendedor com vendas associadas, o banco lança uma exceção por deixar as vendas sem chave estrangeira
            }
        }

        public async Task UpdateAsync(Seller seller)
        {
            if(!await _context.Seller.AnyAsync(x => x.Id == seller.Id))
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException e) // captura a exceção de banco e traduz ela para exceção a nível de serviço
            {
                throw new DbConcurrencyException(e.Message);
            }
            
        }
    }
}
